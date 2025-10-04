using UnityEngine;


public class GroundScript : MonoBehaviour
{
    /// <summary>
    /// Number objects.
    /// </summary>
    public GameObject[] Objects { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (Objects == null) return;
    }


    public void HandleObjectsReady(GameObject[] objects)
    {
        Objects = objects;
    }
}
