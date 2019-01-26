using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessComponent.Main_Classes.Model.Service_Class
{
    interface IUserSubject
    {
        void RegistrObserver(IUserObserver observer);
        void RemoveObserver(IUserObserver observer);
        void NotifyObservers(Issue outData);
    }

    interface IUserObserver
    {
        void Update(Issue outData);
    }
}
