namespace Logic.Algorithms
{
    public class ReturnData
    {
        public string AlgorithmName { get; set; }

        public int Result { get; set; }

        public int Iterations { get; set; }

        public int TimeInMs { get; set; }

        public ReturnData(string name, int result, int iterations, int time)
        {
            AlgorithmName = name;
            Result = result;
            Iterations = iterations;
            TimeInMs = time;
        }
    }
}
