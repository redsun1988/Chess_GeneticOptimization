using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using ChessComponent.Main_Classes.Model.Service_Class;

namespace ChessComponent.Main_Classes.Model
{
    class GenOptimization : IUserSubject
    {

        public GenOptimization()
        {
            InstanceId = _classNextInstanceId++;     // Присваевает новое значение InstanceID; Увеличивает значение NextInstanceID на 1
            _classInstanceCount++;                   // Увеличивает значение ClassInstanceCount на 1
        }

        public GenOptimization(
            int countOfIndividual,
            int row,
            int col,
            int startV,
            int endV,
            ITask task,
            OptimGoal aimGoal)
            : this()
            {
                AimGoal = aimGoal;
                _startVariation = startV;
                _endVariation = endV;

                for (byte i = 0; i < countOfIndividual; i++)
                {   // Созданние объектов массива
                    _population.Add(new FloatData(    
                        row,col,
                        startV, endV,
                        task));
                }
            }

        public readonly int InstanceId;

        public static long ClassInstanceCount
        {
            get
            {
                return _classInstanceCount;
            }
        }

        public Individual this[int indexer]
        {
            get { return _population[indexer]; }
            set { _population[indexer] = value; }
        }

        public int СountIndividual
        {
            get
            {
                return _population.Count;
            }

            set
            {
                try
                {
                    if (value > _population.Count)
                    {
                        for (int i = _population.Count; i < value; i++)
                        {
                            _population.Add(new FloatData(_population[1]));
                        }
                    }
                    if (value < _population.Count)
                    {
                        _population.RemoveRange(value - 1, _population.Count - value);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }

            }
        }

        public int StartV
        {
            get
            {
                return _startVariation;
            }
            set
            {
                _startVariation = value;
            }
        }

        public int EndV
        {
            get
            {
                return _endVariation;
            }
            set
            {
                _endVariation = value;
            }
        }

        public bool IsStoped
        {
            get
            {
                return _isStopedValue;
            }
            set
            {
                _isStopedValue = value;
            }
        }

        public OptimGoal AimGoal;

        public Issue OutData = new Issue();

        public void Next(int iterationCount)
        {
            if (IsStoped)
            {
                _evolutionThread = new Thread(new ParameterizedThreadStart(_Next))
                                       {
                                           Priority = ThreadPriority.Highest,
                                           IsBackground = true
                                       };
                _evolutionThread.Start(iterationCount);
            }
        }

        public void RegistrObserver(IUserObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("Observer");
            OnNotifyObsirvers += new NotifyObsirversHandler(observer.Update);
        }

        public void RemoveObserver(IUserObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("Observer");
            OnNotifyObsirvers -= new NotifyObsirversHandler(observer.Update);
        }

        public void NotifyObservers(Issue outData)
        {
            if (OnNotifyObsirvers == null)
                throw new ArgumentNullException("outData");
            OnNotifyObsirvers(outData);
        }

        ~GenOptimization()
        {
            _classInstanceCount--;
        }

        private static int _classNextInstanceId;
        public static long _classInstanceCount;
        private List<Individual> _population = new List<Individual>();
        private int _startVariation;
        private int _endVariation;
        private int _indexLider;
        private bool _isStopedValue = true;
        private int IndexLider
        {
            get
            {
                return _indexLider;
            }
            set
            {
                if (_indexLider != value)
                {
                    _indexLider = value;
                    OutData.Lider = this[_indexLider];
                    NotifyObservers(OutData);
                }
            }
        }
        private void Evolution()
        {
            var r = new Random(DateTime.Now.Millisecond);
            int localIndexLider = 0;
            double lider = this[0].Goal;

            for (byte i = 1; i < СountIndividual; i++)
            {
                if (lider == this[i].Goal)
                {
                    if (r.Next(2) == 1)
                    {
                        localIndexLider = i;
                    }
                }
                if (AimGoal == OptimGoal.glMax)
                {
                    if (lider < this[i].Goal)
                    {
                        lider = this[i].Goal;
                        localIndexLider = i;
                    }
                }
                if (AimGoal == OptimGoal.glMin)
                {
                    if (lider > this[i].Goal)
                    {
                        lider = this[i].Goal;
                        localIndexLider = i;
                    }
                }
            }
            IndexLider = localIndexLider;
            Individual kid;
            for (byte i = 0; i < СountIndividual; i++)
            {
                if (i != IndexLider)
                {
                    kid = new FloatData(_population[i]);
                    kid.Selection(_population[IndexLider]);
                    kid.Mutation(0.05);
                    if (AimGoal == OptimGoal.glMin)
                        if ((kid.Goal <= _population[i].Goal) && kid.IsPostSelectionCondition)
                        {
                            _population[i] = kid;
                            if (_population[i].ProCompare(_population[IndexLider], 0))
                                _population[i].SetMatrixRandom(0, 1);
                        }
                    if (AimGoal == OptimGoal.glMax)
                        if ((kid.Goal >= _population[i].Goal) && kid.IsPostSelectionCondition)
                        {
                            _population[i] = kid;
                            if (_population[i].ProCompare(_population[IndexLider], 0))
                                _population[i].SetMatrixRandom(0, 1);
                        }
                }
            }
        }

        private void _Next(Object iterationCount)
        {
            if (iterationCount == null) throw new ArgumentNullException("iterationCount");
            
            IsStoped = false;
            var iterationCountLikeInt = (int)iterationCount;

            OutData.CountOfIterations = iterationCountLikeInt;
            for (int i = 1; i <= iterationCountLikeInt; i++)
            {
                Evolution();

                OutData.Interation = i;
                NotifyObservers(OutData);
            }

            IsStoped = true; 
        }
        private delegate void NotifyObsirversHandler(Issue outData);
        private event NotifyObsirversHandler OnNotifyObsirvers;
        private Thread _evolutionThread;

    }
}
