using DinnerSimulator.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.DiningRoom.Controller;
using DinnerSimulator.DiningRoom.Controller.Actors;

namespace DinnerSimulator.DiningRoom.Controller.Strategy
{
    class CoolStrategy : IOrderStrategy
    {
        Order IOrderStrategy.Order(CustomerController customer, MenuCard menuCard)
        {
            throw new NotImplementedException();
        }
    }
}
