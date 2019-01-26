using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessComponent.Main_Classes.Model.Service_Class
{
    interface ITask
    {
        double AimFunction(Individual arg);

        bool PreSetCondition(Individual arg);

        bool PostSelectoinCondition(Individual arg);
    }
}
