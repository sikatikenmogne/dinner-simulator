using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Model.Strategy;

namespace DinnerSimulator.DiningRoom.Model.Actors
{

    public class CustomerGroup : ICustomer
    {
        
        IOrderStrategy customerStrategy;

        public List<string> PreferenceList { get; set; }

        public CustomerChoiceMenuEnum ChoiceMenu { get; set; }

        public List<Order> Orders { get; set; }

        public CustomerStateEnum CustomerState { get; set; }

        public int Count { get; set; }

        public int[] TablePosition { get; set; }

        public CustomerGroup()
        {
            this.CustomerState = CustomerStateEnum.WaitTableAttribution;
            this.Count = 1;
        }
        public CustomerGroup(int count) {
            this.Count = count;
            this.CustomerState = CustomerStateEnum.WaitTableAttribution;
        }

        public CustomerGroup(int count, IOrderStrategy customerStrategy)
        {
            this.Count = count;
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


    }
}
