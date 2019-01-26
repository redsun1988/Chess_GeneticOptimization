using ChessComponent.Main_Classes.Model;
using ChessComponent.Main_Classes.Model.Service_Class;

namespace ChessComponent.Main_Classes.Control
{
    class ChessControl
    {
        public ChessControl()
        {
            _model = new GenOptimization(2,
                                        3, 3,
                                        0, 1,
                                        _rule, OptimGoal.glMin);
        }

        public int IndividualCount
        {
            get { return _model.СountIndividual; }
            set
            {
                if (_model.IsStoped)
                    if (value >= 2) _model.СountIndividual = value;
            }
        }


        public int ChessBoardSize
        {
            get { return _chessBoardSize; }
            set
            {
                if (_model.IsStoped)
                    if (value >= 3)                
                    {
                        _chessBoardSize = value;
                        for (int i = 0; i < IndividualCount; i++)
                        {
                            _model[i].Row = _chessBoardSize;
                            _model[i].Col = _chessBoardSize;
                            _model[i].SetMatrixRandom(_model.StartV, _model.EndV);
                        }
                    }
            }
        }

        public OptimGoal OptGoal
        {
            get
            {
                return _model.AimGoal;
            }
            set
            {
                _model.AimGoal = value;
            }
        }

        public void Bind(IUserObserver obs)
        {
            _model.RegistrObserver(obs);
        }

        public void Next(int iterationCount)
        {
            if (iterationCount>0)
            _model.Next(iterationCount);
        }

        private GenOptimization _model = new GenOptimization();
        private ITask _rule = new ChessHorsRule();
        private int _individualCount = 2;
        private int _chessBoardSize = 3;

    }
}
