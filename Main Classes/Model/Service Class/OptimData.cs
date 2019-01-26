using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;

namespace ChessComponent.Main_Classes.Model.Service_Class
{
    sealed class FloatData : Individual
    {
        public FloatData(int row, int col)
            :base()
        {
            Mat.AddRange(new List<double>[row]);
            for (int i = 0; i < row; i++)
            {
                Mat[i] = new List<double>();
                Mat[i].AddRange(new double[col]);
            }
        }

        public FloatData(int row, int col, int startV, int endV, ITask task)
            : base()
        {
            Mat.AddRange(new List<double>[row]);
            for (int i = 0; i < row; i++)
            {
                Mat[i] = new List<double>();
                Mat[i].AddRange(new double[col]);
            }

            _task = task;
            SetMatrixRandom(startV, endV);

        }

        public FloatData(double[][] mat)
            : base()
        {
            Mat.AddRange(new List<double>[mat.Length]);
            for (byte i = 0; i < mat.Length; i++)
            {
                Mat[i] = new List<double>();
                Mat[i].AddRange(mat[i]);
            }
        }

        public FloatData(Individual initializer)
            : base()
        {
            Mat.AddRange(new List<double>[initializer.Row]);
            for (int i = 0; i < initializer.Row; i++)
            {
                Mat[i] = new List<double>();
                Mat[i].AddRange(((FloatData)initializer).Mat[i].ToArray());
            }
            _task = ((FloatData)initializer)._task;
        }

        override public double this[int index1, int index2]
        {
            get
            {
                try
                {
                    return Mat[index1][index2];
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                    return 0;
                }
            }
            set
            {
                try
                {
                    Mat[index1][index2] = value;
                    _isModified = true; 
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private List<List<double>> Mat = new List<List<double>>();

        override public int Row
        {
            get
            {
                try
                {
                    return Mat.Count;
                }
                catch
                {
                    return 1;
                }
            }

            set
            {
                try
                {
                    if (Mat.Count < value)
                    {
                        for (int i = Mat.Count; i < value; i++)
                        {
                            Mat.Add(new List<double>());
                            Mat[i].AddRange(new double[Col]);
                        }
                    }
                    if (Mat.Count > value)
                        Mat.RemoveRange(value - 1, Mat.Count - value);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        override public int Col
        {
            get
            {
                try
                {
                    return Mat[0].Count;
                }
                catch
                {
                    return 1;
                }
            }

            set
            {
                try
                {
                    if (Mat[0].Count < value)
                    {
                        foreach (List<double> t in Mat)
                        {
                            t.AddRange(new double[value - t.Count]);
                        }
                    }
                    if (Mat[0].Count > value)
                    {
                        for (byte i = 0; i < Mat.Count; i++)
                        {
                            Mat[i].RemoveRange(value - 1, Mat[i].Count - value);
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        public void Add(int startRow, int startCol, FloatData summand)
        {
            int locCol = summand.Col;
            for (int i = 0; i < summand.Row; i++)
            {
                int locI = i + startRow;
                if ((locI < Row) && (locI >= 0))
                    for (int j = 0; j < locCol; j++)
                    {
                        int locJ = j + startCol;
                        if ((locJ < Col) && (locJ >= 0))
                            this[locI, locJ] = this[locI, locJ] + summand[i, j];
                    }
            }
        }

        public static FloatData operator *(FloatData val1, FloatData val2)
        {
            var result = new FloatData(val1.Row, val2.Col);

            if (val1.Col != val2.Row)
            {
                MessageBox.Show("");
                throw new Exception();
            }

            for (int i = 0; i < result.Row; i++)
            {
                for (int j = 0; i < result.Col; j++)
                {
                    double sum = 0;
                    for (int r = 0; r < result.Row; r++)
                    {
                        try
                        {
                            sum = checked(sum
                                + checked(val1[i, r] * val2[r, j]));
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }

                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }

        public static FloatData operator +(FloatData oper1, FloatData oper2)
        {

            var result = new FloatData(oper1);

            if ((oper1.Row != oper2.Row) && (oper1.Col != oper2.Col))
            {
                MessageBox.Show("");
                throw new IndexOutOfRangeException("Выход за границы массива");
            }
            result.Add(0, 0, oper2);
            return result;
        }

        override public double Goal
        {
            get
            {
                if (_isModified)
                    _goal = _task.AimFunction(this);
                _isModified = false;

                return _goal;
            }
        }

        override protected bool IsPresetCondition
        {
            get
            {
                return _task.PreSetCondition(this);
            }
        }

        override public bool IsPostSelectionCondition
        {
            get
            {
                return _task.PostSelectoinCondition(this);
            }
        }

        override public bool ProCompare(Individual partner, double threshold)
        {
            double result = 0;

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    result += Math.Abs(this[i, j] - partner[i, j]);
                }
            }
            if (result > threshold) return false;
            return true;
        }

        override public void Mutation(double probability)
        {
            var r = new Random(DateTime.Now.Millisecond);
            if (probability < 1)
            {
                for (byte i = 0; i < Row; i++)   
                {
                    for (byte j = 0; j < Col; j++)     
                    {
                        if (r.NextDouble() < probability)
                        {
                            this[i, j] *= r.Next(0, 2); 
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("");
                throw
                    new IndexOutOfRangeException("Ввод неверного значения");
            }
        }

        override public void SetMatrixRandom(int start, int end)
        {
            var r = new Random(DateTime.Now.Millisecond);
            Thread.Sleep(1);
            for (int i = 0; i < Row; i++)          
            {
                for (int j = 0; j < Col; j++)      
                {
                    this[i, j] = (end - start) * (r.NextDouble()) + start;    
                }
            }
            while (!(_task.PreSetCondition(this)))
            {
                SetItemRandom(0, 1);
            }
        }

        override public void SetItemRandom(int start, int end)
        {
            int i;
            int j;
            var r = new Random(DateTime.Now.Millisecond);

            do
            {
                i = r.Next(Row);
                j = r.Next(Col);
            }
            while (this[i, j] == 0);
            this[i, j] = (end - start) * (r.NextDouble()) + start;
        }

        override public void Selection(Individual partner)
        {
            var r = new Random(DateTime.Now.Millisecond);
            Thread.Sleep(1);
            for (byte i = 0; i < Row; i++)
            {
                for (byte j = 0; j < Col; j++)
                {
                    if (r.NextDouble() > 0.50)
                        this[i, j] = this[i, j];
                    else
                        this[i, j] = partner[i, j];
                }
            }
        }

        private readonly ITask _task;
        private double _goal;
        private bool _isModified = true;

    }

    public abstract class Individual
    {
        protected Individual()       
        {
            InstanceId = _classNextInstanceId++;     // Присваевает новое значение InstanceID; Увеличивает значение NextInstanceID на 1
            _classInstanceCount++;                   // Увеличивает значение ClassInstanceCount на 1
        }
        public readonly int InstanceId;
        public static long InstanceCount
        {
            get
            {
                return _classInstanceCount;
            }
        }

        abstract public double this[int index1, int index2]
        {
            get; set;
        }

        virtual public int Row
        {
            get;
            set;
        }
        virtual public int Col
        {
            get;
            set;
        }
        virtual public double Goal
        {
            get;
            set;
        }
        virtual protected bool IsPresetCondition
        {
            get;
            set;
        }

        public virtual bool IsPostSelectionCondition
        {
            get;
            set;
        }
        abstract public bool ProCompare(Individual partner, double threshold);
        abstract public void Mutation(double probability);
        abstract public void SetMatrixRandom(int start, int end);
        abstract public void SetItemRandom(int start, int end);
        abstract public void Selection(Individual partner);

        ~Individual()
        {
            _classInstanceCount--;
        }
        private static int _classNextInstanceId;
        private static long _classInstanceCount;
    }

}
