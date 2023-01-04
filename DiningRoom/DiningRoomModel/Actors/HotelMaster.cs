using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DinnerSimulator.DiningRoom.Model.Actors
{
    public class HotelMaster
    {
        
        public List<LineChief> LineChiefs { get; set; }

        public int[] ReceiveCustomers(CustomerGroup customerGroup)
        {   

            Console.WriteLine("Maitre d'hotel: Comnbien etes vous ?");

            Console.WriteLine("Clients: " + customerGroup.Count + ".");

            Console.WriteLine("Maitre d'hotel: Veuillez patienter s'il vous plait...");

            return DiningRoomModel.GetSuitableTablePosition(customerGroup.Count);

        }
    }
}
