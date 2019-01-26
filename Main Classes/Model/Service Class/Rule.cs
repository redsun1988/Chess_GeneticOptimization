namespace ChessComponent.Main_Classes.Model.Service_Class
{
    class ChessHorsRule : ITask
    {
        /// <summary>
        /// То как бъёт конь
        /// </summary>
        private static readonly double[][] Hb = new[]
                                                    {new double [] {0f,1f,0f,1f,0f},
                                                       new double [] {1f,0f,0f,0f,1f},
                                                       new double [] {0f,0f,1f,0f,0f},
                                                       new double [] {1f,0f,0f,0f,1f},
                                                       new double [] {0f,1f,0f,1f,0f}};

        private readonly FloatData _horseBurned = new FloatData(Hb);

        // целевая функция
        public double AimFunction(Individual arg)
        {
            double result = 0;
            for (byte i = 0; i < arg.Row; i++)
            {
                for (byte j = 0; j < arg.Col; j++)
                {
                    if (arg[i, j] != 0)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public bool PreSetCondition(Individual arg)
        {
            var burnedMap = new FloatData(arg.Row, arg.Col);

            for (int i = 0; i < arg.Row; i++)
            {
                for (int j = 0; j < arg.Col; j++)
                {
                    if (arg[i, j] != 0)
                    {
                        burnedMap.Add((i - 2),
                            (j - 2),
                            _horseBurned);
                    }
                }
            }

            for (byte i = 0; i < burnedMap.Row; i++)
            {
                for (byte j = 0; j < burnedMap.Col; j++)
                {
                    if (burnedMap[i, j] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public bool PostSelectoinCondition(Individual arg)
        {
            var burnedMap = new FloatData(arg.Row, arg.Col);

            for (int i = 0; i < arg.Row; i++)
            {
                for (int j = 0; j < arg.Col; j++)
                {
                    if (arg[i, j] != 0)
                    {
                        burnedMap.Add((i - 2),
                            (j - 2),
                            _horseBurned);
                    }
                }
            }

            for (byte i = 0; i < burnedMap.Row; i++)
            {
                for (byte j = 0; j < burnedMap.Col; j++)
                {
                    if (burnedMap[i, j] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
