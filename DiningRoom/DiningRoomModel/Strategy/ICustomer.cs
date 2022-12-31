using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.Common.Model;

namespace DinnerSimulator.DiningRoom.Model.Strategy
{
    public interface ICustomer
    {
        Order Order(MenuCard menuCard);

        void ChangeOrderStrategy(IOrderStrategy customerStrategy);
    }
}
