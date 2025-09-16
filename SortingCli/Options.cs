using CommandLine;
using SortingLib;


namespace SortingCli
{
    internal sealed class Options
    {
        [Option('n', "number", Required = false, HelpText = "Specify the size of the array to be sorted.")]
        public int? Number { get; set; }


        [Option('a', "algorithm", Required = false, HelpText = "Specify the sorting algorithm to use.")]
        public SortingAlgorithms? Algorithm { get; set; }
    }
}
