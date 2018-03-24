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

            int[,] matrix = graph.Matrix;
            int matrixH = SubstractMinValues(matrix);
            Tuple<int, int, int> ijFine = GetIJFine(matrix);
            var iForZeroWithMaxFine = ijFine.Item1;
            var jForZeroWithMaxFine = ijFine.Item2;
            var ijs = new List<Tuple<int, int>>();
            var allMatricesAndHs = new List<Tuple<int[,], int, List<Tuple<int, int>>>>();
            List<Tuple<int, int>> route = GetRoute(matrix, matrixH, ijs, allMatricesAndHs);

            foreach (Tuple<int, int> vertex in route)
            {
                result += matrix[vertex.Item1, vertex.Item2];
            }

            var endTime = DateTime.Now;

            return new ReturnData("Branch&Bound Algorithm", result, iterations, (endTime - startTime).Milliseconds);
        }

        private static List<Tuple<int, int>> GetRoute(int[,] matrix, int matrixH, List<Tuple<int, int>> ijs,
            List<Tuple<int[,], int, List<Tuple<int, int>>>> allMatricesAndHs)
        {
            if (matrix.Length == 4)
            {
                ijs.Add(Tuple.Create(matrix[0, 0], matrix[1, 1]));
                ijs.Add(Tuple.Create(matrix[1, 0], matrix[0, 1]));

                return ijs;
            }

            allMatricesAndHs.Remove(Tuple.Create(matrix, matrixH, ijs));

            Tuple<int, int, int> ijFine = GetIJFine(matrix);
            var iForZeroWithMaxFine = ijFine.Item1;
            var jForZeroWithMaxFine = ijFine.Item2;
            var maxFine = ijFine.Item3;

            int[,] newMatrixWithoutIJ = FormNewMatrixWithoutIJ(matrix, iForZeroWithMaxFine, jForZeroWithMaxFine);
            int newMatrixWithoutIJH = matrixH + GetIJFine(matrix).Item3;
            List<Tuple<int, int>> ijsWithoutIJ = ijs.ConvertAll(ij => ij);


            int[,] newMatrixWithIJ = FormNewMatrixWithIJ(matrix, iForZeroWithMaxFine, jForZeroWithMaxFine);
            int newMatrixWithIJH = matrixH + SubstractMinValues(newMatrixWithIJ);
            List<Tuple<int, int>> ijsWithIJ = ijs.ConvertAll(ij => ij);
            ijsWithIJ.Add(Tuple.Create(iForZeroWithMaxFine, jForZeroWithMaxFine));

            allMatricesAndHs.Add(Tuple.Create(newMatrixWithoutIJ, newMatrixWithoutIJH, ijsWithoutIJ));
            allMatricesAndHs.Add(Tuple.Create(newMatrixWithIJ, newMatrixWithIJH, ijsWithIJ));

            Tuple<int[,], int, List<Tuple<int, int>>> matrixWithMinH =
                allMatricesAndHs.Last(m => m.Item2 == allMatricesAndHs.Select(mm => mm.Item2).Min());

            return GetRoute(matrixWithMinH.Item1, matrixWithMinH.Item2, matrixWithMinH.Item3, allMatricesAndHs);
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

                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    matrix[i, j] -= min;
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

                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    matrix[j, i] -= min;
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

        private static int[,] FormNewMatrixWithIJ(int[,] matrix, int iForZeroWithMaxFine, int jForZeroWithMaxFine)
        {
            var newMatrix = new int[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            var ii = 0;
            var jj = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    ii = i >= iForZeroWithMaxFine && i != 0 ? i - 1 : i;
                    jj = j >= jForZeroWithMaxFine && j != 0 ? j - 1 : j;

                    if (i != iForZeroWithMaxFine && j != jForZeroWithMaxFine)
                    {
                        if (newMatrix[ii, jj] != inf)
                        {
                            newMatrix[ii, jj] = matrix[i, j];
                        }
                    }

                    if (i == iForZeroWithMaxFine && j == jForZeroWithMaxFine)
                    {
                        newMatrix[ii, jj] = inf;
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
