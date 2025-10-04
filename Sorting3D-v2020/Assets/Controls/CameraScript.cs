using UnityEngine;

public class CameraScript : MonoBehaviour
{
    #region Public fields

    public GameObject Borders;

    public float MinHeight = 1f;

    public float MaxHeight = 50f;

    public float ZoomSpeed = 100f;
    
    public float RotationSpeed = 5f;
    
    public float PanSpeed = 0.5f;

    #endregion


    #region Private fields

    private float minX, maxX, minZ, maxZ;

    #endregion


    void Start()
    {
        var bounds = Borders.GetComponent<Renderer>().bounds;
        minX = bounds.min.x;
        maxX = bounds.max.x;
        minZ = bounds.min.z;
        maxZ = bounds.max.z;
    }


    void Update()
    {
        var position = transform.position;
        var angles = transform.eulerAngles;
        var yaw = angles.y;
        var pitch = angles.x;

        // Orbit: Alt + Left Mouse Button
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt))
        {
            yaw += Input.GetAxis("Mouse X") * RotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * RotationSpeed;
        }

        // Zoom: Mouse wheel
        var distance = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        if (distance != 0)
            position += transform.forward * distance;

        // Pan: Alt + Right Mouse Button
        if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftAlt))
        {
            position -= Input.GetAxis("Mouse X") * PanSpeed * transform.right;
            position -= Input.GetAxis("Mouse Y") * PanSpeed * transform.up;
        }

        // Apply transform
        var rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = new Vector3(Mathf.Clamp(position.x, minX, maxX),
                                         Mathf.Clamp(position.y, MinHeight, MaxHeight),
                                         Mathf.Clamp(position.z, minZ, maxZ));
        transform.rotation = rotation;
    }
}
