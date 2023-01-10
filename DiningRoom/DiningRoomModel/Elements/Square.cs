using DinnerSimulator.DiningRoom.Model.Actors;
using System.Collections.Generic;

namespace DinnerSimulator.DiningRoom.Model.Elements
{
    public class Square
    {
        
        public List<Line> Lines { get; set; }

        public List<LineChief> LineChiefs { get; set; }

        public List<Waiter> Waiters { get; set; }

        public Square()
        {
            this.LineChiefs = new List<LineChief>();
            this.Lines = new List<Line>();
            this.Waiters = new List<Waiter>();
        }

    }
}
