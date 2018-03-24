using Logic.Graphs;
using System;

namespace Logic.Algorithms
{
    public static class LocalSearch3
    {
        public static ReturnData Search(Graph graph)
        {
            var startTime = DateTime.Now;
            var result = 0;
            var iterations = 0;



            var endTime = DateTime.Now;

            return new ReturnData("Local Search 3 Algorithm", result, iterations, (endTime - startTime).Milliseconds);
        }
    }
}
