using UnityEngine;


public class GroundScript : MonoBehaviour
{
    /// <summary>
    /// Number objects.
    /// </summary>
    public GameObject[] Objects { get; private set; }


    /// <summary>
    /// General state of all number objects.
    /// </summary>
    public ObjectStates AllState { get; private set; } = ObjectStates.Falling;


    /// <summary>
    /// Force multiplier applied to the objects when they need to vertically align.
    /// </summary>
    public float AlignForce = 0.02f;


    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (Objects == null) return;

        switch (AllState)
        {
            case ObjectStates.Falling:
                if (GroundUtilities.AllObjectsSettled(Objects))
                    AllState = ObjectStates.VertAligning;
                break;

            case ObjectStates.VertAligning:
                if (GroundUtilities.VertAlignedObjects(Objects, AlignForce))
                {
                    Debug.Log("All aligned");
                    AllState = ObjectStates.Flattening;
                }

                break;
        }
    }


    public void HandleObjectsReady(GameObject[] objects)
    {
        Objects = objects;
    }
}
