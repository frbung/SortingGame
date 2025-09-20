namespace SortingLib
{
    /// <summary>
    /// Possible operations during sorting.
    /// Includes also decision making and comment steps (like choosing a pivot).
    /// </summary>
    public enum StepTypes
    {
        /// <summary>
        /// Default value, not to be used.
        /// </summary>
        None,


        /// <summary>
        /// Operation: compare two elements (see <see cref="SortingStep.Left"/>
        /// and <see cref="SortingStep.Right"/>).
        /// </summary>
        Compare,


        /// <summary>
        /// Operation: swap two elements (see <see cref="SortingStep.Left"/>
        /// and <see cref="SortingStep.Right"/>).
        /// </summary>
        Swap,


        /// <summary>
        /// Start sorting the array from index <see cref="SortingStep.Left"/> till
        /// the index <see cref="SortingStep.Right"/> (both including).
        /// </summary>
        SortingStart,


        /// <summary>
        /// Shift the elements between index <see cref="SortingStep.Left"/> and
        /// <see cref="SortingStep.Right"/> by one position to "the right".
        /// </summary>
        ShiftRight
    }
}
