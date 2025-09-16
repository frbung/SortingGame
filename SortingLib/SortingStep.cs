namespace SortingLib;

/// <summary>
/// A single action performed by the algorithm.
/// </summary>
public sealed class SortingStep
{
    #region Properties
    
    public StepTypes StepType { get; init; }

    public int Left { get; init; }

    public int Right { get; init; }
    
    public int[] ListAfterTheStep { get; init; } = [];

    #endregion


    public SortingStep(StepTypes stepType, int left, int right, int[] listAfterTheStep)
    {
        StepType = stepType;
        Left = left;
        Right = right;
        ListAfterTheStep = listAfterTheStep.ToArray();
    }
}
