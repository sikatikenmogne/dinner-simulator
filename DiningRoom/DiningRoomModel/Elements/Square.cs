using DinnerSimulator.DiningRoom.Model.Actors;
using System.Collections.Generic;

namespace DinnerSimulator.DiningRoom.Model.Elements
{
    public class Square
    {
        
        public List<Line> Lines { get; set; }

        public List<LineChief> LineChiefs { get; set; }
        public Square()
        {
            this.LineChiefs = new List<LineChief>();
            this.Lines = new List<Line>();
        }

        public Square(List<Line> lines)
        {
            this.Lines = lines;
        }

    }
}
