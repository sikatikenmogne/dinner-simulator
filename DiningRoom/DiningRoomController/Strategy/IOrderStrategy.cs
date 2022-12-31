using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Controller.Actors;

namespace DinnerSimulator.DiningRoom.Controller.Strategy
{
    public interface IOrderStrategy
    {
        Order Order(CustomerController customer,MenuCard menuCard);
    }
}
