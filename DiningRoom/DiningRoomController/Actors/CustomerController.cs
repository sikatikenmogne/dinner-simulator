using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.DiningRoom.Controller.Strategy;
using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Model.Actors;

namespace DinnerSimulator.DiningRoom.Controller.Actors
{
    public class CustomerController : ICustomer
    {
        IOrderStrategy customerStrategy;
        CustomerGroup customer;
        public CustomerController(CustomerGroup customer, IOrderStrategy customerStrategy)
        {
            this.customer = customer;
            this.customerStrategy = customerStrategy;
        }
        public void ChangeOrderStrategy(IOrderStrategy customerStrategy)
        {
            this.customerStrategy = customerStrategy;
        }

        public Order Order(MenuCard menuCard)
        {
            return this.customerStrategy.Order(this, menuCard);
        }

        public CustomerGroup Customer { get => customer; set => customer = value; }
    }
}
