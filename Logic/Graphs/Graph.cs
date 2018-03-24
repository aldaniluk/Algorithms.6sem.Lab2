using System.Text;

namespace Logic.Graphs
{
    public class Graph
    {
        public int Size { get; }
        public int[,] Matrix { get; }

        public Graph(int size)
        {
            Size = size;
            Matrix = new int[size, size];
        }

        public Graph(int[,] matrix)
        {
            Size = matrix.GetLength(0);
            Matrix = matrix;
        }

        public int Get(int i, int j)
        {
            return Matrix[i, j];
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    result.Append($"{Matrix[i, j]} ");
                }

                result.Append('\n');
            }

            return result.ToString();
        }
    }
}
