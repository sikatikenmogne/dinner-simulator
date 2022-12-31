using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.DiningRoom.Model.Elements;
using DinnerSimulator.DiningRoom.Model.Actors;
using DinnerSimulator.Common.Model;


namespace DinnerSimulator.DiningRoom.Controller.Actors
{
    public class LineChiefController
    {
        LineChief lineChief;
        public LineChiefController(LineChief lineChief)
        {
            this.lineChief = lineChief;
        }

        public void InstallCustomers(CustomerGroup customer, Table table)
        {
            customer.Move(table.PosX, table.PosY);
            Console.WriteLine("Chef de rang: Veuillez prendre place.");
            table.Group = customer;
            customer.CustomerState = CustomerState.Installed;
            table.State = EquipmentState.InUse;

        }
        public LineChief LineChief { get => lineChief; set => lineChief = value; }

        internal void setMenuCard(Table table, List<MenuCard> menuCards)
        {
            foreach(MenuCard menuCard in menuCards)
            {
                if(menuCard.State == EquipmentState.Available)
                {
                    menuCard.State = EquipmentState.InUse;
                    table.MenuCard = menuCard;
                    return;
                }
            }
        }
    }
}
