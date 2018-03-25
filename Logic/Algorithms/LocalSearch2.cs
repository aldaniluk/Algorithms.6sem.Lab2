using Logic.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Algorithms
{
    public static class LocalSearch2
    {
        public static ReturnData Search(Graph graph)
        {
            var startTime = DateTime.Now;
            var result = 0;
            var iterations = 0;

            result = SearchInternal(graph, ref iterations);

            var endTime = DateTime.Now;

            return new ReturnData("Local Search 2 Algorithm", result, iterations, (endTime - startTime).Milliseconds);
        }

        private static int SearchInternal(Graph graph, ref int iterations)
        {
            var route = new List<Tuple<int, int>>();
            for (int i = 0; i < graph.Matrix.GetLength(0) - 1; i++)
            {
                route.Add(Tuple.Create(i, i + 1));
            }
            route.Add(Tuple.Create(graph.Matrix.GetLength(0) - 1, 0));
            var changedCounter = 0;

            while (true)
            {

                for (int i = 0; i < graph.Matrix.GetLength(0); i++)
                {
                    iterations++;
                    while (route[0].Item1 != i)
                    {
                        for (int j = 0; j < route.Count - 1; j++)
                        {
                            Tuple<int, int> temp = route[j];
                            route[j] = route[j + 1];
                            route[j + 1] = temp;
                        }
                    }

                    Change(i, route, graph, ref changedCounter);
                }

                if (changedCounter > route.Count * 2)
                {
                    break;
                }
            }

            return CountRouteLength(graph, route);
        }

        private static void Change(int vertexToChange1, List<Tuple<int, int>> route, Graph graph,
            ref int changedCounter)
        {
            var vertexToChange2 = route.Single(r => r.Item1 == vertexToChange1).Item2;
            var edgeToChange = Tuple.Create(vertexToChange1, vertexToChange2);

            changedCounter++;
            List<Tuple<int, int>> possibleVertices = GetPossibleVertices(edgeToChange, route);
            if (ChangeVertex(edgeToChange, route, possibleVertices, graph))
            {
                changedCounter = 0;
            }
        }

        private static List<Tuple<int, int>> GetPossibleVertices(Tuple<int, int> edgeToChange,
            List<Tuple<int, int>> route)
        {
            var possibleVerticesToChange = new List<Tuple<int, int>>();
            foreach (Tuple<int, int> possibleVertex in route)
            {
                if (possibleVertex.Item1 != edgeToChange.Item2 && possibleVertex.Item2 != edgeToChange.Item1
                    && possibleVertex.Item1 != edgeToChange.Item1 && possibleVertex.Item2 != edgeToChange.Item2)
                {
                    possibleVerticesToChange.Add(possibleVertex);
                }
            }

            return possibleVerticesToChange;
        }

        private static bool ChangeVertex(Tuple<int, int> edgeToChange, List<Tuple<int, int>> route,
            List<Tuple<int, int>> possibleVertices, Graph graph)
        {
            var changed = false;

            var minValue = int.MaxValue;
            var newVertexToChange = Tuple.Create(int.MaxValue, int.MaxValue);
            foreach (Tuple<int, int> possibleVertex in possibleVertices)
            {
                int oldValue = graph.Matrix[edgeToChange.Item1, edgeToChange.Item2] +
                    graph.Matrix[possibleVertex.Item1, possibleVertex.Item2];
                int newValue = graph.Matrix[edgeToChange.Item1, possibleVertex.Item1] +
                    graph.Matrix[edgeToChange.Item2, possibleVertex.Item2];
                if (newValue < oldValue && newValue < minValue)
                {
                    minValue = newValue;
                    newVertexToChange = possibleVertex;
                }
            }

            if (newVertexToChange.Item1 != int.MaxValue && newVertexToChange.Item2 != int.MaxValue)
            {
                Tuple<int, int> vertex1 = route.First(r => r.Item1 == edgeToChange.Item1
                    && r.Item2 == edgeToChange.Item2);
                int vertex1Index = route.IndexOf(route.First(r => r.Item1 == edgeToChange.Item1
                    && r.Item2 == edgeToChange.Item2));

                Tuple<int, int> vertex2 = route.First(r => r.Item1 == newVertexToChange.Item1
                    && r.Item2 == newVertexToChange.Item2);
                int vertex2Index = route.IndexOf(route.First(r => r.Item1 == newVertexToChange.Item1
                    && r.Item2 == newVertexToChange.Item2));

                route[vertex1Index] = Tuple.Create(vertex1.Item1, vertex2.Item1);
                route[vertex2Index] = Tuple.Create(vertex1.Item2, vertex2.Item2);
                int start = vertex1Index == route.Count - 1 ? 0 : vertex1Index;
                List<Tuple<int, int>> changeOrder = route.Skip(vertex1Index + 1).Take(vertex2Index - vertex1Index - 1)
                    .Select(i => i).ToList();
                var newOrder = new List<Tuple<int, int>>();
                foreach (Tuple<int, int> edge in changeOrder)
                {
                    int edgeIndex = route.IndexOf(route.Single(r => r.Item1 == edge.Item1 && r.Item2 == edge.Item2));
                    newOrder.Add(Tuple.Create(edge.Item2, edge.Item1));
                }
                if (changeOrder.Count != 0)
                {
                    int firstEdgeIndex = route.IndexOf(route.Single(r => r.Item1 == changeOrder[0].Item1
                        && r.Item2 == changeOrder[0].Item2));
                    for (int i = firstEdgeIndex; i < changeOrder.Count + firstEdgeIndex; i++)
                    {
                        route[i % route.Count] = newOrder.Last();
                        newOrder.RemoveAt(newOrder.Count - 1);
                    }
                }

                changed = true;
            }

            return changed;
        }

        private static int CountRouteLength(Graph graph, List<Tuple<int, int>> route)
        {
            int result = 0;
            for (int i = 0; i < route.Count; i++)
            {
                result += graph.Matrix[route[i].Item1, route[i].Item2];
            }

            return result;
        }
    }
}
