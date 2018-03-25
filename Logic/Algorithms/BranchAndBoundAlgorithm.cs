using Logic.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Algorithms
{
    public class BranchAndBoundAlgorithm
    {
        private static int inf = int.MaxValue;

        public static ReturnData Search(Graph graph)
        {
            var startTime = DateTime.Now;
            var result = 0;
            var iterations = 0;

            List<Tuple<int, int>> route = GetRoute(graph, ref iterations);

            foreach (Tuple<int, int> vertex in route)
            {
                result += graph.Matrix[vertex.Item1, vertex.Item2];
            }

            var endTime = DateTime.Now;

            return new ReturnData("Branch&Bound Algorithm", result, iterations, (endTime - startTime).Milliseconds);
        }

        private static List<Tuple<int, int>> GetRoute(Graph graph, ref int iterations)
        {
            int[,] matrix = new int[graph.Matrix.GetLength(0), graph.Matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    matrix[i, j] = graph.Matrix[i, j];
                }
            }

            int matrixH = SubstractMinValues(matrix);
            var ijs = new List<Tuple<int, int>>();
            var allMatricesAndHs = new List<Tuple<int[,], int, List<Tuple<int, int>>>>();

            return GetRoute(matrix, matrixH, ijs, allMatricesAndHs, ref iterations);
        }

        private static List<Tuple<int, int>> GetRoute(int[,] matrix, int matrixH, List<Tuple<int, int>> ijs,
            List<Tuple<int[,], int, List<Tuple<int, int>>>> allMatricesAndHs, ref int iterations)
        {
            iterations++;

            Tuple<int, int, int> numbersIJNumber = GetNumbersIJNumber(matrix);
            if (numbersIJNumber.Item1 == 1)
            {
                ijs.Add(Tuple.Create(numbersIJNumber.Item2, numbersIJNumber.Item3));

                return ijs;
            }
            if (numbersIJNumber.Item1 == 0)
            {
                return ijs;
            }

            allMatricesAndHs.Remove(Tuple.Create(matrix, matrixH, ijs));

            Tuple<int, int, int> ijFine = GetIJFine(matrix);
            var iForZeroWithMaxFine = ijFine.Item1;
            var jForZeroWithMaxFine = ijFine.Item2;
            var matrixFine = ijFine.Item3;

            int[,] newMatrixWithoutIJ = FormNewMatrixWithoutIJ(matrix, iForZeroWithMaxFine, jForZeroWithMaxFine);
            int newMatrixWithoutIJH = matrixH + SubstractMinValues(newMatrixWithoutIJ);
            List<Tuple<int, int>> ijsWithoutIJ = ijs.ConvertAll(ij => ij);

            List<Tuple<int, int>> ijsWithIJ = ijs.ConvertAll(ij => ij);
            ijsWithIJ.Add(Tuple.Create(iForZeroWithMaxFine, jForZeroWithMaxFine));
            int[,] newMatrixWithIJ = FormNewMatrixWithIJ(matrix, iForZeroWithMaxFine, jForZeroWithMaxFine, ijsWithIJ);
            int newMatrixWithIJH = matrixH + SubstractMinValues(newMatrixWithIJ);

            allMatricesAndHs.Add(Tuple.Create(newMatrixWithoutIJ, newMatrixWithoutIJH, ijsWithoutIJ));
            allMatricesAndHs.Add(Tuple.Create(newMatrixWithIJ, newMatrixWithIJH, ijsWithIJ));

            Tuple<int[,], int, List<Tuple<int, int>>> matrixWithMinH =
                allMatricesAndHs.OrderBy(m => m.Item3.Count)
                .Last(m => m.Item2 == allMatricesAndHs.Select(mm => mm.Item2).Min());

            return GetRoute(matrixWithMinH.Item1, matrixWithMinH.Item2, matrixWithMinH.Item3, allMatricesAndHs,
                ref iterations);
        }

        private static Tuple<int, int, int> GetNumbersIJNumber(int[,] matrix)
        {
            var numbers = 0;
            var numberI = 0;
            var numberJ = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (matrix[i, j] != int.MaxValue)
                    {
                        numbers++;
                        numberI = i;
                        numberJ = j;
                    }
                }
            }

            return Tuple.Create(numbers, numberI, numberJ);
        }

        private static int SubstractMinValues(int[,] matrix)
        {
            //horizontal
            var iMin = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var min = int.MaxValue;
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (matrix[i, j] < min)
                    {
                        min = matrix[i, j];
                    }
                }

                if (min == int.MaxValue)
                {
                    min = 0;
                }

                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (matrix[i, j] != int.MaxValue)
                    {
                        matrix[i, j] -= min;
                    }
                }

                iMin += min;
            }

            //vertical
            var jMin = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var min = int.MaxValue;
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (matrix[j, i] < min)
                    {
                        min = matrix[j, i];
                    }
                }

                if (min == int.MaxValue)
                {
                    min = 0;
                }

                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (matrix[j, i] != int.MaxValue)
                    {
                        matrix[j, i] -= min;
                    }
                }

                jMin += min;
            }

            return iMin + jMin;
        }

        private static Tuple<int, int, int> GetIJFine(int[,] matrix)
        {
            var iForZeroWithMaxFine = 0;
            var jForZeroWithMaxFine = 0;
            var maxFine = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    var fine = 0;
                    if (matrix[i, j] == 0)
                    {
                        var iMin = int.MaxValue;
                        for (int ii = 0; ii < matrix.GetLength(0); ii++)
                        {
                            if (matrix[ii, j] < iMin && ii != i)
                            {
                                iMin = matrix[ii, j];
                            }
                        }

                        var jMin = int.MaxValue;
                        for (int jj = 0; jj < matrix.GetLength(0); jj++)
                        {
                            if (matrix[i, jj] < jMin && jj != j)
                            {
                                jMin = matrix[i, jj];
                            }
                        }

                        fine = iMin + jMin;
                    }

                    if (fine > maxFine)
                    {
                        maxFine = fine;
                        iForZeroWithMaxFine = i;
                        jForZeroWithMaxFine = j;
                    }
                }
            }

            return Tuple.Create(iForZeroWithMaxFine, jForZeroWithMaxFine, maxFine);
        }

        private static int[,] FormNewMatrixWithIJ(int[,] matrix, int iForZeroWithMaxFine, int jForZeroWithMaxFine,
            List<Tuple<int, int>> ijs)
        {
            var newMatrix = new int[matrix.GetLength(0), matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (i == jForZeroWithMaxFine && j == iForZeroWithMaxFine)
                    {
                        newMatrix[i, j] = inf;
                    }
                    else if (i != iForZeroWithMaxFine && j != jForZeroWithMaxFine)
                    {
                        newMatrix[i, j] = matrix[i, j];
                    }
                    else
                    {
                        newMatrix[i, j] = inf;
                    }
                }
            }

            return newMatrix;
        }

        private static int[,] FormNewMatrixWithoutIJ(int[,] matrix, int iForZeroWithMaxFine, int jForZeroWithMaxFine)
        {
            var newMatrix = new int[matrix.GetLength(0), matrix.GetLength(0)];
            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(0); j++)
                {
                    newMatrix[i, j] = matrix[i, j];
                }
            }

            newMatrix[iForZeroWithMaxFine, jForZeroWithMaxFine] = inf;
            newMatrix[jForZeroWithMaxFine, iForZeroWithMaxFine] = inf;

            return newMatrix;
        }
    }
}
