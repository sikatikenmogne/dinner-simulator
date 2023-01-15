using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Model.Actors;
using System.Collections.Generic;

namespace DinnerSimulator.DiningRoom.Model.Elements
{
    public class Table : Equipment
    {
        public Table(int nbPlaces)
        {
            this.NbPlaces = nbPlaces;
            this.State = EquipmentState.Available;
            this.Items = new Queue<IItem>();
        }

        public int NbPlaces { get; set; }
        public CustomerGroup Group { get; set; }
        public MenuCard MenuCard { get; set; }
        public Queue<IItem> Items { get; set; }
    }
}
