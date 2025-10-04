using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public class NumberObjectScript : MonoBehaviour
{
    #region Constants

    private const float SettledSpeed = 0.01f;

    private const float SettledAngle = 1f;

    private const float TouchDistance = 1f;

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

    #endregion


    #region GameObject API

    void Start()
    {
        // Assign text material
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<TMPro.TextMeshPro>(out var text))
            {
                if (TextMaterial != null)
                {
                    text.fontMaterial = TextMaterial;
                }

                text.text = Number.ToString();
            }
        }

        // STart the state machine
        StartCoroutine(LifeCycle());
    }


    void Update()
    {
    }

    #endregion


    #region Utility

    private IEnumerator LifeCycle()
    {
        State = ObjectStates.Falling;
        while (!IsStationary())
            yield return null;

        State = ObjectStates.VertAligning;
        while (!IsVertical())
        {
            yield return OrientUp(AlignForce, AlignDuration);
        }
        FreezeRotation();
        State = ObjectStates.Flattening;
    }


    /// <summary>
    /// Try get the object to stand up. If fails, repulse.
    /// </summary>
    private IEnumerator OrientUp(float forceMultiplier, float duration)
    {
        var elapsed = 0f;

        while (!IsVertical() && elapsed < duration)
        {
            OrientUpStep(forceMultiplier);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!IsVertical())
        {
            ApplyRepulsion();
        }
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


    private void OrientUpStep(float forceMultiplier)
    {
        // We're still moving
        if (IsStationary()) return;

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


    private void FreezeRotation()
    {
         GetComponent<Rigidbody>().freezeRotation = true;
    }


    /// <summary>
    /// We are stuck, do somthing.
    /// </summary>
    private void ApplyRepulsion()
    {
        var neighbours = Physics.OverlapSphere(transform.position, RepulsionRadius);
        var numbers = neighbours
            .Select(n => n.transform.root.gameObject.GetComponent<NumberObjectScript>())
            .Where(s => s != null && s.gameObject != gameObject)
            .ToArray();

        foreach (var neighbour in neighbours)
        {
            if (neighbour.attachedRigidbody != null && neighbour.gameObject != gameObject)
            {
                var direction = (neighbour.transform.position - transform.position).normalized;
                neighbour.attachedRigidbody.AddForce(direction * RepulsionForce, ForceMode.Impulse);
            }
        }
    }


    private bool IsNumberObjectThere(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, out var hit, TouchDistance))
        {
            Debug.Log("Object detected: " + hit.collider.name);
            return true;
        }

        return false;
    }

    #endregion
}
