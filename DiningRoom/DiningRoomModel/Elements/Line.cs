using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.DiningRoom.Model.Actors;

namespace DinnerSimulator.DiningRoom.Model.Elements
{
    public class Line
    {

        public List<Table> Tables { get; set; }
        public Line(int nbTables, int nbSeatsPerTable)
        {
            Tables = new List<Table>();
            for (int i = 0; i < nbTables; i++)
            {
                Tables.Add(new Table(nbSeatsPerTable));
            }
        }

        public Line(int nbTables)
        {
            Random rand = new Random();

            int nbSeatsPerTable = rand.Next(1, 6);
            Console.WriteLine("Number of seat per tables : " + nbSeatsPerTable * 2);
            Tables = new List<Table>();
            for (int i = 0; i < nbTables; i++)
            {
                Tables.Add(new Table(nbSeatsPerTable*2));
            }
        }
    }
}
