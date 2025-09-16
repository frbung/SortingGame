namespace SortingLib;

internal sealed class MergeSort
{
    internal static IEnumerable<SortingStep> Sort(int[] orig)
    {
        var arr = orig.ToArray();
        var n = arr.Length;

        int[] aux = new int[n];

        return MergeSortRec(arr, 0, n - 1);
    }


    private static IEnumerable<SortingStep> MergeSortRec(int[] arr, int left, int right)
    {
        if (left >= right) yield break;
        yield return new SortingStep(StepTypes.SortingStart, left, right, arr);

        var mid = left + (right - left) / 2;

        foreach (var lstep in MergeSortRec(arr, left, mid))
            yield return lstep;

        foreach (var rstep in MergeSortRec(arr, mid + 1, right))
            yield return rstep;

        foreach (var mstep in Merge(arr, left, mid, right))
            yield return mstep;
    }


    /// <summary>
    /// Merge two sorted subarrays arr[left..mid] and arr[mid+1..right].
    /// </summary>
    private static IEnumerable<SortingStep> Merge(int[] arr, int left, int mid, int right)
    {
        int lhead = left, rhead = mid + 1;

        while (lhead <= mid && rhead <= right)
        {
            yield return new SortingStep(StepTypes.Compare, lhead, rhead, arr);

            if (arr[lhead] <= arr[rhead])
            {
                lhead++;
            }
            else
            {
                var temp = arr[rhead];
                var index = rhead;

                while (index != lhead)
                {
                    arr[index] = arr[index - 1];
                    index--;
                }
                
                yield return new SortingStep(StepTypes.ShiftRight, lhead, rhead - 1, arr);

                arr[lhead] = temp;
                yield return new SortingStep(StepTypes.Swap, lhead, rhead, arr);

                lhead++;
                mid++;
                rhead++;
            }
        }
    }
}
