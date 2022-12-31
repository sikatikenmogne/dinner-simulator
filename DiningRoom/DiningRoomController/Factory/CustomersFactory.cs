using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinnerSimulator.DiningRoom.Model.Actors;
using DinnerSimulator.DiningRoom.Controller.Observer;

namespace DinnerSimulator.DiningRoom.Controller.Factory
{
    public class CustomersFactory : AbstractCustomersFactory, IObservable<CustomerGroup>
    {
        private readonly List<IObserver<CustomerGroup>> observers;
        public CustomersFactory()
        {
            observers = new List<IObserver<CustomerGroup>>();
        }
        public override CustomerGroup CreateCustomers(int nbCustomers)
        {
            CustomerGroup customer_ = new CustomerGroup(nbCustomers);

            foreach (var observer in observers)
                observer.OnNext(customer_);

            return customer_;
        }

        public IDisposable Subscribe(IObserver<CustomerGroup> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                // Provide observer with existing data.
                //foreach (var item in flights)
                //    observer.OnNext(item);
            }
            return new CFUnsubscriber<CustomerGroup>(observers, observer);
        }

    }
}
