using System.Collections.Generic;
using Logic.Graphs;
using System.Linq;
using System;

namespace Logic.Algorithms
{
    public static class BruteForceSearch
    {
        public static ReturnData Search(Graph graph)
        {
            var startTime = DateTime.Now;
            var result = int.MaxValue;
            var iterations = 0;

            var route = new List<int>();

            //create all possible routes ~ (graph's size - 1)!
            for (int count = 0; count < Factorial(graph.Size - 1); count++)
            {
                iterations++;

                route = GetNewRoute(route, graph);
                int routeValue = GetRouteValue(route, graph);

                if (routeValue < result)
                {
                    result = routeValue;
                }
            }

            var endTime = DateTime.Now;

            return new ReturnData("Brute-Force Search", result, iterations, (endTime - startTime).Milliseconds);
        }

        private static List<int> GetNewRoute(List<int> route, Graph graph)
        {
            if (route.Count == 0)
            {
                for (int i = 0; i < graph.Size; i++)
                {
                    route.Add(i);
                }

                return route;
            }

            var index = 0;
            for (int i = graph.Size - 2; i >= 0; i--)
            {
                if (route[i] < route[i + 1])
                {
                    index = i;
                    break;
                }
            }

            var newRoute = new List<int>();
            for (int i = 0; i < route.Count; i++)
            {
                if (i < index)
                {
                    newRoute.Add(route[i]);
                }
                else if (i == index)
                {
                    newRoute.Add(route.Where(e => e > route[i]).OrderBy(e => e)
                        .First(e => !newRoute.Contains(e)));
                }
                else
                {
                    newRoute.Add(route.OrderBy(e => e).First(e => !newRoute.Contains(e)));
                }
            }

            return newRoute;
        }

        private static int GetRouteValue(List<int> route, Graph graph)
        {
            int routeValue = 0;
            for (int i = 0; i < route.Count - 1; i++)
            {
                routeValue += graph.Get(route[i], route[i + 1]);
            }

            routeValue += graph.Get(route.First(), route.Last());

            return routeValue;
        }

        private static int Factorial(int number)
        {
            return number != 0 ? number * Factorial(number - 1) : 1;
        }
    }
}
