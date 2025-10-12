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
    /// THe vertical alignment is done.
    /// </summary>
    VertAligned,

    /// <summary>
    /// Jumping off other objects to the ground level.
    /// </summary>
    LiningUp,

    /// <summary>
    /// Reached the destination.
    /// </summary>
    LinedUp,

    /// <summary>
    /// Stay at the destination until next command.
    /// </summary>
    WaitingForSorting,
    
    Sorted,
}
