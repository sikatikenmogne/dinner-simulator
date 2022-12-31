using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.Common.Model;
using DinnerSimulator.Common.Model.Move;
using DinnerSimulator.DiningRoom.Model.Strategy;

namespace DinnerSimulator.DiningRoom.Model.Actors
{

    public enum CustomerState
    {
        WaitTableAttribution,
        WaitLineChief,
        Installed,
        Ordering,
        TableDispose,
        Ordered,
        WaitEntree,
        WaitPlate,
        WaitDessert,
        WaitBill,
        Leave
    };

    public enum CustomerChoiceMenu
    {
        entry,
        dish,
        entryAndDish,
        dishAndDessert,
        all,
        entryAndDessert,
        dessert,
    };


    public class CustomerGroup : Position, IMove, ICustomer
    {
        private List<Order> orders;
        private CustomerState state;
        private CustomerChoiceMenu choiceMenu;
        private List<string> preferenceList;
        private int count = 0;

        IOrderStrategy customerStrategy;


        public List<string> PreferenceList { get => preferenceList; set => preferenceList = value; }

        public CustomerChoiceMenu ChoiceMenu { get => choiceMenu; set => choiceMenu = value; }

        public List<Order> Orders { get => orders; set => orders = value; }

        public CustomerState CustomerState { get => state; set => state = value; }

        public int Count { get => count; set => count = value; }

        public int[] TablePosition { get; set; }

        public CustomerGroup()
        {
            this.CustomerState = CustomerState.WaitTableAttribution;
            this.count = 1;
        }
        public CustomerGroup(int count) {
            this.count = count;
            this.CustomerState = CustomerState.WaitTableAttribution;
        }

        public CustomerGroup(int count, IOrderStrategy customerStrategy)
        {
            this.count = count;
            this.customerStrategy = customerStrategy;
        }

        public void Move(int posX, int posY)
        {
            this.PosX = posX;
            this.PosY = posY;
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
