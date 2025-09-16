using CommandLine;
using SortingLib;


namespace SortingCli
{
    class Program
    {
        static void Main(string[] args)
        {
             // Default values
            var number = 10;
            var alg = SortingAlgorithms.MergeSort;

            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(opts =>
                {
                    if (opts.Number.HasValue)
                    {
                        number = opts.Number.Value;
                    }
                    if (opts.Algorithm.HasValue)
                    {
                        alg = opts.Algorithm.Value;
                    }
                });

            var arr = GameUtil.GenerateList(number);
            Console.WriteLine($"Original list: [{string.Join(", ", arr)}]");

            var steps = GameUtil.Sort(arr, alg);
            var comparisons = 0;
            var swaps = 0;

            foreach (var step in steps)
            {
                if (arr.SequenceEqual(step.ListAfterTheStep))
                {
                    Console.WriteLine($"{step.StepType} index {step.Left} and {step.Right}.");
                }
                else
                {
                    Console.WriteLine($"{step.StepType} index {step.Left} and {step.Right}, result: [{string.Join(", ", step.ListAfterTheStep)}]");
                    arr = step.ListAfterTheStep;
                }

                if (step.StepType == StepTypes.Compare)
                    comparisons++;
                if (step.StepType == StepTypes.Swap)
                    swaps++;
            }

            Console.WriteLine($"Sorted list: [{string.Join(", ", arr)}]");
            Console.WriteLine($"Total comparisons: {comparisons}, total swaps: {swaps}");
        }
    }
}
