using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinnerSimulator.DiningRoom.Controller.Observer
{
    public class DRUnsubscriber<Order> : IDisposable
    {
        private List<IObserver<Order>> _observers;
        private IObserver<Order> _observer;

        internal DRUnsubscriber(List<IObserver<Order>> observers, IObserver<Order> observer)
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
