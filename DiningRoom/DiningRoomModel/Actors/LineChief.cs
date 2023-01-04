using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinnerSimulator.Common.Model;

namespace DinnerSimulator.DiningRoom.Model.Actors
{
    public class LineChief
    {
        
        public bool Available { get; set; } = true;

        public void InstallCustomers(CustomerGroup customer, int[] tablePosition)
        {
            Console.WriteLine("Chef de rang: Veuillez prendre place.");

            DiningRoomModel.Squares[tablePosition[0]].Lines[tablePosition[1]].Tables[tablePosition[2]].State = EquipmentState.InUse;
            DiningRoomModel.Squares[tablePosition[0]].Lines[tablePosition[1]].Tables[tablePosition[2]].Group = customer;
            customer.CustomerState = CustomerStateEnum.Installed;
            customer.TablePosition = tablePosition;
        }

        internal void SetMenuCardOnTable(Queue<MenuCard> menuCards, int[] tablePosition)
        {
            if (menuCards != null)
            {
                menuCards.Peek().State = EquipmentState.InUse;
                DiningRoomModel.Squares[tablePosition[0]].Lines[tablePosition[1]].Tables[tablePosition[2]].MenuCard = menuCards.Dequeue();
            }
        }
    }
}
