using System.Linq;


namespace SortingLib
{
    /// <summary>
    /// A single action performed by the algorithm.
    /// </summary>
    public sealed class SortingStep
    {
        #region Properties

        public StepTypes StepType { get; set; }

        public int Left { get; set; }

        public int Right { get; set; }

        public int[] ListAfterTheStep { get; set; } = new int[0];

        #endregion


        public SortingStep(StepTypes stepType, int left, int right, int[] listAfterTheStep)
        {
            StepType = stepType;
            Left = left;
            Right = right;
            ListAfterTheStep = listAfterTheStep.ToArray();
        }
    }
}
