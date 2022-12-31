using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinnerSimulator.DiningRoom.Controller.Observer
{
    public class CFUnsubscriber<Customer> : IDisposable
    {
        private List<IObserver<Customer>> _observers;
        private IObserver<Customer> _observer;

        public CFUnsubscriber(List<IObserver<Customer>> observers, IObserver<Customer> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}
