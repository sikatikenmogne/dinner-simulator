using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DinnerSimulator.Common.Model
{
    public class MenuCard : Equipment
    {
        public MenuCard()
        {

        }
        public MenuCard(Dictionary<string, List<Recipe>> menu, DateTime date)
        {
            this.Menu = menu;
            this.Date = date;
            this.State = EquipmentState.Available;
        }
        public MenuCard(Dictionary<string, List<Recipe>> menu)
        {
            this.Menu = menu;
            this.State = EquipmentState.Available;
        }
        public Dictionary<string, List<Recipe>> Menu { get; set; }
        public DateTime Date { get; set; }

    }

}
