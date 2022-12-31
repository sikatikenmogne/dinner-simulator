using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinnerSimulator.DiningRoom.Model.Actors;

using DinnerSimulator.DiningRoom.Model.Strategy;

namespace DinnerSimulator.DiningRoom.Model.Factory
{
    public class CustomersFactory : AbstractCustomersFactory
    {
        public static int MaxCustomersCount { get; set; }
        public int DefaultCustomersCountPerGroup { get; set; }
        public static int CustomerArrivalRythm { get; set; } = 1;

        public CustomersFactory(int maxCustomersCount, int defaultCustomersCountPerGroup)
        {
            MaxCustomersCount = maxCustomersCount;
            DefaultCustomersCountPerGroup = defaultCustomersCountPerGroup;
        }

        public CustomersFactory(int maxCustomersCount, int defaultCustomersCountPerGroup, int customerArrivalRythm)
        {
            MaxCustomersCount = maxCustomersCount;
            DefaultCustomersCountPerGroup = defaultCustomersCountPerGroup;
            CustomerArrivalRythm = customerArrivalRythm;
        }

        public override CustomerGroup CreateCustomers(int nbCustomers)
        {
            return new CustomerGroup(nbCustomers, new RandomOrderStrategy());
        }
    }
}
