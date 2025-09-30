using UnityEngine;


public static class GroundUtilities
{
    #region Constants

    private const float SettledSpeed = 0.01f;

    private const float SettledAngle = 1f;

    #endregion


    #region API

    /// <summary>
    /// Check that all cubes are stationary.
    /// </summary>
    /// <param name="objects"></param>
    public static bool AllObjectsSettled(GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            if (obj.GetComponent<Rigidbody>().velocity.magnitude > SettledSpeed)
                return false;
        }

        return true;
    }


    /// <summary>
    /// Push the objects to be vertically aligned if necessary.
    /// </summary>
    /// <returns><see langword="true"/> if the objects are vertically aligned.</returns>
    public static bool VertAlignedObjects(GameObject[] objects, float force)
    {
        var res = true;

        foreach (var obj in objects)
        {
            var rb = obj.GetComponent<Rigidbody>();
            var up = rb.transform.up;

            if (Vector3.Angle(Vector3.up, up) > SettledAngle)
            {
                PushObjectUp(rb, force);
                res = false;
            }
        }

        return res;
    }

    #endregion


    private static void PushObjectUp(Rigidbody rb, float forceMultiplier)
    {
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
        rb.AddForceAtPosition(force, rb.transform.position + Vector3.up, ForceMode.Impulse);
    }
}
