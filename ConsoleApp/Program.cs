using Logic.Algorithms;
using Logic.Graphs;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var inf = int.MaxValue;
            var arr = new int[,]
            {
                {inf,2,5,1,7,1,5,2,6,5},
                {2,inf,3,2,9,1,9,5,7,2},
                {5,3,inf,8,6,2,8,1,3,4},
                {1,2,8,inf,3,4,1,2,4,1},
                {7,9,6,3,inf,7,5,7,4,6},
                {1,1,2,4,7,inf,4,9,1,3},
                {5,9,8,1,5,4,inf,1,7,8},
                {2,5,1,2,7,9,1,inf,4,4},
                {6,7,3,4,4,1,7,4,inf,9},
                {5,2,4,1,6,3,8,4,9,inf}
            };
            var graph = new Graph(arr);

            Console.WriteLine($"{"algorithm name",25} {"iterations",15} {"result",10} {"time (ms)",15}");

            ReturnData bruteForceSearch = BruteForceSearch.Search(graph);
            Console.WriteLine($"{bruteForceSearch.AlgorithmName,25} {bruteForceSearch.Iterations,15} " +
                $"{bruteForceSearch.Result,10} {bruteForceSearch.TimeInMs,15}");

            ReturnData greedyAlgorithm = GreedyAlgorithm.Search(graph);
            Console.WriteLine($"{greedyAlgorithm.AlgorithmName,25} {greedyAlgorithm.Iterations,15} " +
                $"{greedyAlgorithm.Result,10} {greedyAlgorithm.TimeInMs,15}");

            ReturnData branchAndBoundAlgorithm = BranchAndBoundAlgorithm.Search(graph);
            Console.WriteLine($"{branchAndBoundAlgorithm.AlgorithmName,25} {branchAndBoundAlgorithm.Iterations,15} " +
                $"{branchAndBoundAlgorithm.Result,10} {branchAndBoundAlgorithm.TimeInMs,15}");

            ReturnData localSearch2Algorithm = LocalSearch2.Search(graph);
            Console.WriteLine($"{localSearch2Algorithm.AlgorithmName,25} {localSearch2Algorithm.Iterations,15} " +
               $"{localSearch2Algorithm.Result,10} {localSearch2Algorithm.TimeInMs,15}");
        }
    }
}
