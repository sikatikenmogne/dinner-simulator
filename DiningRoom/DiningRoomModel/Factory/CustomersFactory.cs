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
        public static int CustomersArrivalRythm { get; set; } = 1;

        public CustomersFactory(int maxCustomersCount, int defaultCustomersCountPerGroup)
        {
            MaxCustomersCount = maxCustomersCount;
            DefaultCustomersCountPerGroup = defaultCustomersCountPerGroup;
        }

        public CustomersFactory(int maxCustomersCount, int defaultCustomersCountPerGroup, int customersArrivalRythm)
        {
            MaxCustomersCount = maxCustomersCount;
            DefaultCustomersCountPerGroup = defaultCustomersCountPerGroup;
            CustomersArrivalRythm = customersArrivalRythm;
        }

        public override CustomerGroup CreateCustomers(int nbCustomers)
        {
            if(nbCustomers == 0)
            {
                Random rnd = new Random();
                nbCustomers = rnd.Next(1, 11);
            }
            return new CustomerGroup(nbCustomers, new RandomOrderStrategy());
        }
    }
}
