using Logic.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Algorithms
{
    public static class GreedyAlgorithm
    {
        public static ReturnData Search(Graph graph)
        {
            var startTime = DateTime.Now;
            var result = 0;
            var iterations = 0;

            var route = new List<int>() { 0 };

            var possibleVertices = new List<int>();
            for (int i = 1; i < graph.Size; i++)
            {
                possibleVertices.Add(i);
            }

            for (int i = 1; i < graph.Size; i++)
            {
                iterations++;

                Tuple<int, int> minRouteValueAndVertex = GetMinRouteValueAndVertex(possibleVertices, route, graph);

                result += minRouteValueAndVertex.Item1;
                route.Add(minRouteValueAndVertex.Item2);
                possibleVertices.Remove(minRouteValueAndVertex.Item2);
            }

            result += graph.Get(0, graph.Size - 1);
            var endTime = DateTime.Now;

            return new ReturnData("Greedy Search", result, iterations, (endTime - startTime).Milliseconds);
        }

        private static Tuple<int, int> GetMinRouteValueAndVertex(List<int> possibleVertices, List<int> route,
            Graph graph)
        {
            var minRouteValue = int.MaxValue;
            var chosenVertex = int.MaxValue;
            foreach (int vertex in possibleVertices)
            {
                int currentValue = graph.Get(route.Last(), vertex);
                if (currentValue < minRouteValue)
                {
                    minRouteValue = currentValue;
                    chosenVertex = vertex;
                }
            }

            return Tuple.Create(minRouteValue, chosenVertex);
        }
    }
}
