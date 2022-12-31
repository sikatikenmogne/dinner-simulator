using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.Common.Model;
using DinnerSimulator.Common.Model.Move;
using DinnerSimulator.DiningRoom.Model.Elements;

namespace DinnerSimulator.DiningRoom.Model.Actors
{
    public class HotelMaster
    {
        private List<LineChief> lineChiefs;

        public List<LineChief> LineChiefs
        {
            get => lineChiefs;
            set => lineChiefs = value;
        }

        public HotelMaster()
        {
            this.lineChiefs = new List<LineChief>();
            this.lineChiefs.Add(new LineChief());
        }

        public int[] ReceiveCustomers(CustomerGroup customerGroup)
        {   
            Console.WriteLine("Maitre d'hotel: Comnbien etes vous ?");

            Console.WriteLine("Clients: " + customerGroup.Count + ".");

            Console.WriteLine("Maitre d'hotel: Veuillez patienter s'il vous plait...");

            //int[] tablePosition = DiningRoomModel.GetSuitableTablePosition(customerGroup.Count);

            return DiningRoomModel.GetSuitableTablePosition(customerGroup.Count);

        }
    }
}
