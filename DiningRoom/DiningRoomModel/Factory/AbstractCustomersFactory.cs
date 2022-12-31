using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.DiningRoom.Model.Actors;

namespace DinnerSimulator.DiningRoom.Model.Factory
{
    public abstract class AbstractCustomersFactory
    {
        public abstract CustomerGroup CreateCustomers(int nbCustomers);
    }
}
