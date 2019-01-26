using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessComponent.Main_Classes.Model.Service_Class
{
    public enum OptimGoal
    {
        glMin = 0, 
        glMax = 1
    }

    public class Issue
    {
        public int Interation
        {
            get
            {
                return _Interation;
            }
            set
            {
                _Interation = value;
            }
        }

        public int CountOfIterations
        {
            get
            {
                return _CountOfIterations;
            }
            set
            {
                _CountOfIterations = value;
            }
        }

        public Individual Lider 
        {
            get
            {
                return _Lider;
            }
            set
            {
                _Lider = value;
            }
        }

        private Individual _Lider;
        private int _Interation;
        private int _CountOfIterations;

    }
}
