using System.Collections;
using System.Linq;
using SortingLib;
using UnityEngine;
using UnityEngine.AI;


public class Spawner : MonoBehaviour
{
    #region Fields for Inspector

    /// <summary>
    /// Which object to spawn.
    /// It's expected to have a <see cref="NumberObjectScript"/> component.
    /// Assign this in the Inspector.
    /// </summary>
    public GameObject Prefab;


    /// <summary>
    /// Where should the objects go to.
    /// Align them there in a row along local X axis.
    /// </summary>
    public GameObject Target;


    /// <summary>
    /// Relative size of the space between the objects when placed on the target object.
    /// </summary>
    public float Margin = 0.8f;


    /// <summary>
    /// Interval between spawning objects, in seconds.
    /// </summary>
    public float SpawnInterval = 0.5f;


    /// <summary>
    /// When the objects are ready, send them to the next step.
    /// Assign this in the Inspector.
    /// </summary>
    public GameObjectArrayEvent OnObjectsReady;


    /// <summary>
    /// Force multiplier applied to the objects when they need to vertically align.
    /// </summary>
    public float AlignForce = 0.5f;

    #endregion


    #region Properties for possible reading from code

    /// <summary>
    /// The numbers generated in <see cref="Start"/>, input to object generation.
    /// </summary>
    public int[] Numbers { get; private set; }


    /// <summary>
    /// Set to <see langword="true"/> when all the objects have been generated.
    /// </summary>
    public bool IsGenerated { get; private set; }


    /// <summary>
    /// The number objects generated for <see cref="Numbers"/>.
    /// The generation is asynchronous an process, so this list will be populated
    /// only after <see cref="IsGenerated"/> is set to <see langword="true"/>.
    /// </summary>
    public GameObject[] Objects { get; private set; }

    #endregion


    #region Unity life cycle

    // Start is called before the first frame update
    void Start()
    {
        Numbers = GameUtil.GenerateList(10);
        IsGenerated = false;
        StartCoroutine(SpawnObjects());
    }

    #endregion


    #region Utility - asynchronous generation

    IEnumerator SpawnObjects()
    {
        Objects = new GameObject[Numbers.Length];
        var boundsList = Prefab.GetComponentsInChildren<Renderer>(true).
            Select(r => r is SkinnedMeshRenderer sr ? sr.sharedMesh.bounds : r.bounds);
        var bounds = boundsList.Aggregate(new Bounds(), (a, r) => { a.Encapsulate(r); return a; }, a => a.size);
        var margin = bounds.x * Margin;
        var length = Numbers.Length * bounds.x + (Numbers.Length - 1) * margin;
        var leftOffset = -(length / 2);
        var targetCenterPos = Target.transform.position;
        var dest0PosAtTarget = new Vector3(targetCenterPos.x + leftOffset, targetCenterPos.y, targetCenterPos.z);

        for (var i = 0; i < Numbers.Length; i++)
        {
            var number = Numbers[i];
            var spawnPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(2, 15), Random.Range(-0.5f, 0.5f));
            var spawnRotation = Quaternion.Euler(0, number, number);
            var obj = Instantiate(Prefab, spawnPosition, spawnRotation);

            // Object properties
            obj.name = $"Cube {number}";
            var objScript = obj.GetComponent<NumberObjectScript>();
            objScript.Number = number;
            objScript.AlignForce = AlignForce;

            // Navigation properties
            var nav = obj.GetComponent<NavMeshAgent>();
            nav.avoidancePriority = Random.Range(30, 70);
            nav.speed = Random.Range(6, 8);

            // Object target
            var destPosAtTarget = new Vector3(dest0PosAtTarget.x + i * (bounds.x + margin), dest0PosAtTarget.y, dest0PosAtTarget.z);
            var destPosWorld = Target.transform.TransformPoint(destPosAtTarget);
            var destPosAtGround = transform.InverseTransformPoint(destPosWorld);
            objScript.TargetPosition = destPosAtGround;

            Objects[i] = obj;

            leftOffset += length + margin;
            yield return new WaitForSeconds(SpawnInterval);
        }

        IsGenerated = true;
        OnObjectsReady?.Invoke(Objects);
    }

    #endregion
}
