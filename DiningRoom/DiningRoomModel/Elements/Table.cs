using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Model.Actors;

namespace DinnerSimulator.DiningRoom.Model.Elements
{
    public class Table : Equipment
    {
        public Table(int nbPlaces)
        {
            this.NbPlaces = nbPlaces;
            this.State = EquipmentState.Available;
        }

        public int NbPlaces { get; set; }
        public CustomerGroup Group { get; set; }
        public MenuCard MenuCard { get; set; }
    }
}
