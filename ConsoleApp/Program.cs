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
                {inf,21,53,11,72,13,54,25,66,57},
                {21,inf,32,28,97,16,95,54,73,23},
                {53,32,inf,86,65,24,83,12,31,40},
                {11,28,86,inf,31,42,13,24,45,16},
                {72,97,65,31,inf,78,57,76,45,65},
                {13,16,24,42,78,inf,4,95,15,35},
                {54,95,83,13,57,4,inf,11,72,83},
                {25,54,12,24,76,95,11,inf,44,44},
                {66,73,31,45,45,15,72,44,inf,59},
                {57,23,40,16,65,35,83,44,95,inf}
            };
            var graph = new Graph(arr);

            Console.WriteLine($"{"algorithm name",40} {"iterations",15} {"result",10} {"time (ms)",15}");

            ReturnData bruteForceSearch = BruteForceSearch.Search(graph);
            Console.WriteLine($"{bruteForceSearch.AlgorithmName,40} {bruteForceSearch.Iterations,15} " +
                $"{bruteForceSearch.Result,10} {bruteForceSearch.TimeInMs,15}");

            ReturnData greedyAlgorithm = GreedyAlgorithm.Search(graph);
            Console.WriteLine($"{greedyAlgorithm.AlgorithmName,40} {greedyAlgorithm.Iterations,15} " +
                $"{greedyAlgorithm.Result,10} {greedyAlgorithm.TimeInMs,15}");

            ReturnData branchAndBoundAlgorithm = BranchAndBoundAlgorithm.Search(graph);
            Console.WriteLine($"{branchAndBoundAlgorithm.AlgorithmName,40} {branchAndBoundAlgorithm.Iterations,15} " +
                $"{branchAndBoundAlgorithm.Result,10} {branchAndBoundAlgorithm.TimeInMs,15}");

            ReturnData localSearch2Algorithm = LocalSearch2.Search(graph);
            Console.WriteLine($"{localSearch2Algorithm.AlgorithmName,40} {localSearch2Algorithm.Iterations,15} " +
               $"{localSearch2Algorithm.Result,10} {localSearch2Algorithm.TimeInMs,15}");

            ReturnData simulatedAnnealingAlgorithm_lin = SimulatedAnnealing.Search(graph, SimulatedAnnealing.GetNewTemperature_Linear);
            Console.WriteLine($"{simulatedAnnealingAlgorithm_lin.AlgorithmName + "(lin)",40} {simulatedAnnealingAlgorithm_lin.Iterations,15} " +
               $"{simulatedAnnealingAlgorithm_lin.Result,10} {simulatedAnnealingAlgorithm_lin.TimeInMs,15}");

            ReturnData simulatedAnnealingAlgorithm_pow = SimulatedAnnealing.Search(graph, SimulatedAnnealing.GetNewTemperature_Power);
            Console.WriteLine($"{simulatedAnnealingAlgorithm_pow.AlgorithmName + "(pow)",40} {simulatedAnnealingAlgorithm_pow.Iterations,15} " +
               $"{simulatedAnnealingAlgorithm_pow.Result,10} {simulatedAnnealingAlgorithm_pow.TimeInMs,15}");
        }
    }
}
