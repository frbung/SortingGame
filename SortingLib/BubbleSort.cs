namespace SortingLib;

internal sealed class BubbleSort
{
    internal static IEnumerable<SortingStep> Sort(int[] orig)
    {
        var arr = orig.ToArray();
        var n = arr.Length;

        for (var i = 0; i < n - 1; i++)
        {
            yield return new SortingStep(StepTypes.SortingStart, 0, n - i - 1, arr);

            for (var j = 0; j < n - i - 1; j++)
            {
                yield return new SortingStep(StepTypes.Compare, j, j + 1, arr);

                if (arr[j] > arr[j + 1])
                {
                    var temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;

                    yield return new SortingStep(StepTypes.Swap, j, j + 1, arr);
                }
            }
        }
    }
}
