using UnityEngine;


public class AlwaysUpScript : MonoBehaviour
{
    void LateUpdate()
    {
        // Forces world-up alignment
        transform.rotation = Quaternion.identity;
    }
}
