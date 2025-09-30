/// <summary>
/// Number object state machine.
/// </summary>
public enum ObjectStates
{
    /// <summary>
    /// Initial state: falling under gravity.
    /// </summary>
    Falling,

    /// <summary>
    /// Aligning vertically (bottom down).
    /// </summary>
    VertAligning,

    /// <summary>
    /// Jumping off other objects to the ground level.
    /// </summary>
    Flattening,

    Sorting,

    Sorted
}
