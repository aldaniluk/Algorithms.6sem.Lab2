using Logic.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Algorithms
{
    public static class SimulatedAnnealing
    {
        public static double INITIAL_TEMPERATURE { get; set; } = 10;
        public static double MIN_TEMPERATURE { get; set; } = 0.0000001;

        public static ReturnData Search(Graph graph, Func<double, int, double> getTemperature)
        {
            var startTime = DateTime.Now;
            var iterations = 0;

            List<int> route = GetRoute(graph, ref iterations, getTemperature);

            int result = GetRouteValue(route, graph);

            var endTime = DateTime.Now;

            return new ReturnData("Simulated Annealing Algorithm", result, iterations, (endTime - startTime).Milliseconds);
        }

        #region GetTemperature methods
        public static double GetNewTemperature_Linear(double currentTemperature, int iterations)
        {
            return currentTemperature * 0.8;
        }

        public static double GetNewTemperature_Power(double currentTemperature, int iterations)
        {
            return currentTemperature / (iterations * iterations);
        }
        #endregion

        private static List<int> GetRoute(Graph graph, ref int iterations, Func<double, int, double> getTemperature)
        {
            List<int> currentRoute = GetDefaultRoute(graph);
            int currentRouteValue = GetRouteValue(currentRoute, graph);
            double currentTemperature = INITIAL_TEMPERATURE;

            while (currentTemperature > MIN_TEMPERATURE)
            {
                List<int> possibleRoute = GetPossibleRoute(currentRoute);
                int possibleRouteValue = GetRouteValue(possibleRoute, graph);

                if (possibleRouteValue < currentRouteValue ||
                    GoodProbability(GetProbability(possibleRouteValue - currentRouteValue, currentTemperature)))
                {
                    currentRoute = possibleRoute;
                    currentRouteValue = possibleRouteValue;
                }

                currentTemperature = getTemperature(currentTemperature, ++iterations);
            }

            return currentRoute;
        }

        private static double GetProbability(int deltaRouteValue, double temperature)
        {
            var probability = Math.Exp(-deltaRouteValue / temperature);
            
            return probability;
        }

        private static bool GoodProbability(double probability)
        {
            double randValue = GetRandom(isInt: false);

            return randValue <= probability;
        }

        private static List<int> GetDefaultRoute(Graph graph)
        {
            var route = new List<int>();
            for (int i = 0; i < graph.Size; i++)
            {
                route.Add(i);
            }

            return route;
        }

        private static List<int> GetPossibleRoute(List<int> route)
        {
            List<int> possibleRoute = route.Select(pr => pr).ToList();

            int firstIndex = GetRandom(0, route.Count - 1, isInt: true);
            int secondIndex = GetRandom(0, route.Count - 1, isInt: true);

            possibleRoute[firstIndex] = route[secondIndex];
            possibleRoute[secondIndex] = route[firstIndex];

            return possibleRoute;
        }

        private static int GetRouteValue(List<int> route, Graph graph)
        {
            var result = 0;

            for (int i = 0; i < route.Count - 1; i++)
            {
                result += graph.Matrix[route[i], route[i + 1]];
            }

            result += graph.Matrix[route.Last(), route.First()];

            return result;
        }

        //REAL random
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static dynamic GetRandom(int min = 0, int max = 1, bool isInt = true)
        {
            lock (syncLock)
            {
                if (isInt)
                {
                    return random.Next(min, max);
                }
                else
                {
                    return random.NextDouble();
                }
            }
        }
    }
}
