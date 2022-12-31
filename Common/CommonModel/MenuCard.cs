using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.Common.Model;
using DinnerSimulator.Common.Model.Move;

namespace DinnerSimulator.Common.Model
{
    public class MenuCard : Equipment, IPosition
    {
        private DateTime date;
        private Dictionary<string, List<Recipe>> menu;
        private int posX;
        private int posY;

        public MenuCard(Dictionary<string, List<Recipe>> menu, DateTime date)
        {
            this.menu = menu;
            this.date = date;
            this.State = EquipmentState.Available;
        }
        public MenuCard(Dictionary<string, List<Recipe>> menu)
        {
            this.menu = menu;
            this.State = EquipmentState.Available;
        }
        public MenuCard()
        {

        }
        public int PosX
        {
            get
            {
                return posX;
            }
            set
            {
                posX = value >= 0 ? value : 0;
            }
        }

        public int PosY
        {
            get
            {
                return posY;
            }
            set
            {
                posY = value >= 0 ? value : 0;
            }
        }
        public Dictionary<string, List<Recipe>> Menu { get; set; }
        public DateTime Date { get => date; set => date = value; }
    }

}
