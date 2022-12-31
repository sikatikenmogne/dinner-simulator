using DinnerSimulator.DiningRoom.Model.Actors;
using System.Collections.Generic;

namespace DinnerSimulator.DiningRoom.Model.Elements
{
    public class Square
    {
        private List<Line> lines;
        private List<Waiter> waiters;
        public List<LineChief> lineChiefs;

        public List<Waiter> Waiters { get => waiters; set => waiters = value; }

        public List<Line> Lines { get => lines; set => lines = value; }

        public List<LineChief> LineChiefs { get => lineChiefs; set => lineChiefs = value; }
        public Square()
        {
            this.lineChiefs = new List<LineChief>();
            this.waiters = new List<Waiter>();
            this.lines = new List<Line>();
        //    for (int i = 0; i < Param.WAITER_BY_SQUARE; i++)
        //        this.waiters.Add(new Waiter());
        }

        public Square(List<Waiter> waiters, List<Line> lines)
        {
            this.waiters = waiters;
            this.lines = lines;
            //    for (int i = 0; i < Param.WAITER_BY_SQUARE; i++)
            //        this.waiters.Add(new Waiter());
        }

    }
}
