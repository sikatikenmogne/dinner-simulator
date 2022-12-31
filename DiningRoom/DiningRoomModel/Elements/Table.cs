using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Model.Actors;
using DinnerSimulator.Common.Model.Move;

namespace DinnerSimulator.DiningRoom.Model.Elements
{
    public class Table : Equipment, IPosition
    {
        private int nbPlaces;
        private CustomerGroup group;
        private bool entree = false;
        private bool plate = false;
        private bool dessert = false;
        private int posX = 0;
        private int posY = 0;
        private MenuCard menuCard;

        public Table(int nbPlaces)
        {
            this.nbPlaces = nbPlaces;
            this.State = EquipmentState.Available;
        }

        public Table(int nbPlaces, int PosX, int posY)
        {
            this.nbPlaces = nbPlaces;
            this.PosX = PosX;
            this.posY = posY;
            this.State = EquipmentState.Available;

        }

        public Table(int nbPlaces, Position position)
        {
            this.nbPlaces = nbPlaces;
            this.PosX = position.PosX;
            this.posY = position.PosY;
            this.State = EquipmentState.Available;

        }

        public int NbPlaces { get => nbPlaces; set => nbPlaces = value; }
        public CustomerGroup Group { get => group; set => group = value; }
        public bool Entree { get => entree; set => entree = value; }
        public bool Plate { get => plate; set => plate = value; }
        public bool Dessert { get => dessert; set => dessert = value; }
        public int PosX { get => posX; set => posX = value; }
        public int PosY { get => posY; set => posY = value; }
        public MenuCard MenuCard { get => menuCard; set => menuCard = value; }
    }
}
