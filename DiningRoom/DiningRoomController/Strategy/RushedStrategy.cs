using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Controller.Actors;
using DinnerSimulator.DiningRoom.Controller.Strategy;

namespace DinnerSimulator.DiningRoom.Controller.Strategy
{
    class RushedStrategy : IOrderStrategy
    {
        Order IOrderStrategy.Order(CustomerController customer, MenuCard menuCard)
        {
            throw new NotImplementedException();
        }
    }
}
