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
        private List<Table> tables;
        //private LineChief lineChief;

        public List<Table> Tables { get => tables; set => tables = value; }
        //public LineChief LineChief { get => lineChief; set => lineChief = value; }
        public Line(int nbTables, int nbSeatsPerTable)
        {
            tables = new List<Table>();
            for (int i = 0; i < nbTables; i++)
            {
                tables.Add(new Table(nbSeatsPerTable));
            }
            //    this.tables = new List<Table>();
        }

        public Line(int nbTables)
        {
            Random rand = new Random();

            int nbSeatsPerTable = rand.Next(1, 6);
            Console.WriteLine("Nomber of seat per tables : " + nbSeatsPerTable * 2);
            tables = new List<Table>();
            for (int i = 0; i < nbTables; i++)
            {
                tables.Add(new Table(nbSeatsPerTable*2));
            }
        }
    }
}
