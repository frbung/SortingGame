using System.Collections;
using SortingLib;
using UnityEngine;


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
    public float AlignForce = 0.02f;

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


    // Update is called once per frame
    void Update()
    {
    }

    #endregion


    #region Utility - asynchronous generation

    IEnumerator SpawnObjects()
    {
        Objects = new GameObject[Numbers.Length];

        for (var i = 0; i < Numbers.Length; i++)
        {
            var number = Numbers[i];
            var spawnPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(2, 15), Random.Range(-0.5f, 0.5f));
            var spawnRotation = Quaternion.Euler(0, number, number);
            var obj = Instantiate(Prefab, spawnPosition, spawnRotation);

            obj.name = $"Cube {number}";
            var objScript = obj.GetComponent<NumberObjectScript>();
            objScript.Number = number;
            objScript.AlignForce = AlignForce;

            Objects[i] = obj;

            yield return new WaitForSeconds(SpawnInterval);
        }

        IsGenerated = true;
        OnObjectsReady?.Invoke(Objects);
    }

    #endregion
}
