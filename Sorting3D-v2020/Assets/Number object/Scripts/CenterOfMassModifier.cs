using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[ExecuteAlways]
public class CenterOfMassModifier : MonoBehaviour
{
    public Vector3 CenterOfMassOffset;

    public Color gizmoColor = Color.red;

    public float gizmoSize = 0.1f;


    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CenterOfMassOffset;
    }


    void OnDrawGizmosSelected()
    {
        if (!TryGetComponent<Rigidbody>(out var rb)) return;

        Gizmos.color = gizmoColor;
        Vector3 worldCOM = transform.TransformPoint(rb.centerOfMass);
        Gizmos.DrawSphere(worldCOM, gizmoSize);
        Gizmos.DrawLine(transform.position, worldCOM);
        Debug.Log(worldCOM);
    }
}
