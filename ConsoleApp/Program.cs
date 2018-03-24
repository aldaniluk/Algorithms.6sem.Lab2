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
                {inf,2,5,6,5,3},
                {2,inf,3,5,5,4},
                {5,3,inf,2,4,5},
                {6,5,2,inf,2,5},
                {5,5,4,2,inf,3},
                {3,4,5,5,3,inf}
            };
            var graph = new Graph(arr);

            ReturnData bruteForceSearch = BruteForceSearch.Search(graph);
            ReturnData greedyAlgorithm = GreedyAlgorithm.Search(graph);
            ReturnData branchAndBoundAlgorithm = BranchAndBoundAlgorithm.Search(graph);
            ReturnData localSearch2Algorithm = LocalSearch2.Search(graph);
            ReturnData localSearch3Algorithm = LocalSearch3.Search(graph);

            Console.WriteLine($"{"algorithm name",20} {"iterations",15} {"result",10} {"time (ms)",15}");
            Console.WriteLine($"{bruteForceSearch.AlgorithmName,20} {bruteForceSearch.Iterations,15} " +
                $"{bruteForceSearch.Result,10} {bruteForceSearch.TimeInMs,15}");
            Console.WriteLine($"{greedyAlgorithm.AlgorithmName,20} {greedyAlgorithm.Iterations,15} " +
                $"{greedyAlgorithm.Result,10} {greedyAlgorithm.TimeInMs,15}");
            Console.WriteLine($"{branchAndBoundAlgorithm.AlgorithmName,20} {branchAndBoundAlgorithm.Iterations,15} " +
                $"{branchAndBoundAlgorithm.Result,10} {branchAndBoundAlgorithm.TimeInMs,15}");
        }
    }
}
