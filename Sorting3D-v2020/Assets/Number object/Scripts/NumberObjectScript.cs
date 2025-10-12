using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using TMPro;


public class NumberObjectScript : MonoBehaviour
{
    #region Constants

    private const float SettledSpeed = 0.01f;

    private const float SettledAngle = 1f;

    private const float TouchDistance = 1f;

    private const float PushDuration = 0.5f;

    private const float DestinationReachedDistance = 0.1f;

    private const string IdleTrigger = "Idle";

    private const string PushOthersTrigger = "PushOthers";

    private const string WalkTrigger = "Walk";

    #endregion


    #region Public fields

    [Header("General")]

    /// <summary>
    /// INitial number assigned to this object.
    /// </summary>
    public int Number = 42;


    /// <summary>
    /// Main material for the object.
    /// </summary>
    public Material TextMaterial;


    [Header("Vertical alignment")]

    /// <summary>
    /// Force multiplier applied to the objects when they need to vertically align.
    /// </summary>
    public float AlignForce = 0.02f;


    /// <summary>
    /// How long to try applying the force [seconds].
    /// </summary>
    public float AlignDuration = 2f;


    [Header("Neighbour repulsion")]

    /// <summary>
    /// Radius within the objects are repulsed.
    /// </summary>
    public float RepulsionRadius = 3f;


    /// <summary>
    /// Force used for repulsing neighbouring objects or self if we get stuck.
    /// </summary>
    public float RepulsionForce = 5f;

    #endregion


    #region Not for the Inspector

    public ObjectStates State { get; private set; }


    /// <summary>
    /// Where should the object "go to".
    /// </summary>
    public Vector3 TargetPosition { get; set; }

    #endregion


    #region GameObject API

    void Start()
    {
        // Assign text material
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<TextMeshPro>(out var text))
            {
                if (TextMaterial != null)
                {
                    text.fontMaterial = TextMaterial;
                }

                text.text = Number.ToString();
            }
        }

        // STart the state machine
        StartCoroutine(SetMeUp());
    }


    void Update()
    {
        if (State == ObjectStates.LinedUp && Vector3.Distance(transform.position, TargetPosition) < DestinationReachedDistance)
        {
            GetComponent<Animator>().SetTrigger(IdleTrigger);
            State = ObjectStates.WaitingForSorting;
        }

        if (State == ObjectStates.WaitingForSorting&& Vector3.Distance(transform.position, TargetPosition) > DestinationReachedDistance)
        {
            GetComponent<Animator>().SetTrigger(WalkTrigger);
            GetComponent<NavMeshAgent>().SetDestination(TargetPosition);
        }
    }


    public void OnClicked()
    {
        StartCoroutine(PushOthers());
    }

    #endregion


    #region Utility: life cycle and basics

    private IEnumerator SetMeUp()
    {
        State = ObjectStates.Falling;
        yield return new WaitUntil(() => IsStationary());

        State = ObjectStates.VertAligning;
        yield return OrientVertically();
        FreezeRotation();

        State = ObjectStates.LiningUp;
        yield return GoToTarget();

        State = ObjectStates.LinedUp;
    }


    private bool IsStationary()
    {
        return GetComponent<Rigidbody>().IsSleeping();
    }


    private bool IsVertical()
    {
        if (State == ObjectStates.VertAligned) return true;

        if (Vector3.Angle(Vector3.up, GetComponent<Rigidbody>().transform.up) < SettledAngle)
        {
            State = ObjectStates.VertAligned;
            return true;
        }

        return false;
    }


    private void FreezeRotation()
    {
         GetComponent<Rigidbody>().freezeRotation = true;
    }

    #endregion


    #region Utility: vertical orientation FSM

    private IEnumerator OrientVertically()
    {
        var step = 1;
        while (!IsVertical())
        {
            switch (step)
            {
                case 1:
                case 3:
                    yield return TryOrientUp(AlignForce, AlignDuration);
                    step++;
                    break;
                case 2:
                    yield return PushOthers();
                    step++;
                    break;
                case 4:
                    yield return PushMe();
                    step = 1;
                    break;
            }
        }
    }


    /// <summary>
    /// Try get the object to stand up by turning for some time.
    /// </summary>
    private IEnumerator TryOrientUp(float forceMultiplier, float duration)
    {
        var elapsed = 0f;

        while (!IsVertical() && elapsed < duration)
        {
            OrientUpStep(forceMultiplier);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }


    /// <summary>
    /// We are stuck, push surrounding objects outwards.
    /// </summary>
    private IEnumerator PushOthers()
    {
        var ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }

        var neighbours = Physics.OverlapSphere(transform.position, RepulsionRadius);
        var numbers = neighbours
            .Where(n => n.attachedRigidbody != null)
            .Select(n => new {
                obj = n.attachedRigidbody.gameObject,
                scr = n.attachedRigidbody.gameObject.GetComponent<NumberObjectScript>()
            })
            .Where(o => o.scr != null && o.scr != this)
            .Select(o => o.obj)
            .ToArray();

        if (numbers.Length > 0)
        {
            GetComponent<Animator>().SetTrigger(PushOthersTrigger);

            foreach (var neighbour in numbers)
            {
                var diff = neighbour.transform.position - transform.position;
                var direction = new Vector3(diff.x, 0f, diff.z).normalized;
                neighbour.GetComponent<Rigidbody>().AddForce(direction * RepulsionForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(PushDuration);
        }
    }


    /// <summary>
    /// We are still stuck, push myself away from the walls or from the centre.
    /// </summary>
    private IEnumerator PushMe()
    {
        var neighbours = Physics.OverlapSphere(transform.position, RepulsionRadius);
        var walls = neighbours
            .Where(n => n.attachedRigidbody == null)
            .Where(n => n.name.StartsWith("Wall"))
            .ToArray();

        if (walls.Length == 0)
        {
            // Push from centre
            var direction = transform.position.normalized;
            GetComponent<Rigidbody>().AddForce(direction * RepulsionForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(PushDuration);
    }


    private void OrientUpStep(float forceMultiplier)
    {
        // We're still moving
        if (!IsStationary()) return;

        var rb = GetComponent<Rigidbody>();
        var angleToUp = Vector3.Angle(Vector3.up, rb.transform.up);

        Vector3 force;

        if (angleToUp < 90 + SettledAngle)
        {
            // It's lying on its side; push sideways towards the legs
            force = -rb.transform.up * forceMultiplier;
        }
        else
        {
            // It's staying on its head; push away from the centre sideways
            force = rb.transform.position.normalized * forceMultiplier;
        }

        // Take by the upper side and push
        rb.AddForceAtPosition(force, rb.transform.position + Vector3.up * 2, ForceMode.Impulse);
    }

    #endregion


    #region Moving around

    private IEnumerator GoToTarget()
    {
        // Check navigation agent
        var nav = GetComponent<NavMeshAgent>();        
        nav.enabled = true;
        
        if (NavMesh.SamplePosition(nav.transform.position, out var hit, 15f, NavMesh.AllAreas))
        {
            if (hit.distance > 1f)
            {
                nav.transform.position = hit.position;
            }
        }
        else
        {
            Debug.LogWarning($"Object {name} is not on NavMesh, cannot navigate.");
            yield break;
        }

        nav.SetDestination(TargetPosition);
        GetComponent<Animator>().SetTrigger(WalkTrigger);
        yield return null;
    }

    #endregion
}
