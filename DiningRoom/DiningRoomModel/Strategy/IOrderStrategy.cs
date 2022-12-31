using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Model.Actors;

namespace DinnerSimulator.DiningRoom.Model.Strategy
{
    public interface IOrderStrategy
    {
        Order Order(CustomerGroup customer,MenuCard menuCard);
    }
}
