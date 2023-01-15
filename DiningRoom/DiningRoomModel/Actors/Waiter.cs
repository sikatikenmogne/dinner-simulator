using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinnerSimulator.DiningRoom.Model.Elements;

namespace DinnerSimulator.DiningRoom.Model.Actors
{
    public class Waiter
    {
        public bool Available { get; set; } = true;

        //<summary>Put a basket of bread on the table located with the tablePostion</summary>
        public void SetBreadBasketOnTable(int[] tablePosition, Queue<BreadBasket> breadBaskets, int count)
        {
            int diff = count;

            if (breadBaskets.Count < count)
                diff = count - breadBaskets.Count;

            for (int i = 0; i < diff; i++)
                DiningRoomModel.Squares[tablePosition[0]].Lines[tablePosition[1]].Tables[tablePosition[2]].Items.Enqueue(breadBaskets.Dequeue());
        }

        public void SetBottleOfWaterOnTable(int[] tablePosition, Queue<BottleOfWater> bottleOfWaters, int count)
        {
            int diff = count;

            if (bottleOfWaters.Count < count)
                diff = count - bottleOfWaters.Count;

            for (int i = 0; i < diff; i++)
                DiningRoomModel.Squares[tablePosition[0]].Lines[tablePosition[1]].Tables[tablePosition[2]].Items.Enqueue(bottleOfWaters.Dequeue());
        }
    }
}
