using DinnerSimulator.DiningRoom.Model;
using DinnerSimulator.DiningRoom.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Model.Factory;

namespace DiningRoom
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            /*
                DiningRoomModel diningRoomModel = new DiningRoomModel();
                DiningRoomController diningRoomController = new DiningRoomController(diningRoomModel);
                diningRoomController.Run();
            */

            Dictionary<string, List<Recipe>> menu = new Dictionary<string, List<Recipe>>();

            menu.Add("Entry", new List<Recipe>() { new Recipe(RecipeType.Entry, "Feuillete de crabe", 4, 1000), new Recipe(RecipeType.Entry, "Oeufs cocotte", 4, 200) });
            menu.Add("Dish", new List<Recipe>() { new Recipe(RecipeType.Dish, "Bouillinade anguilles ou poissons", 4, 345), new Recipe(RecipeType.Dish, "Boles de picoulats", 25, 560) });
            menu.Add("Dessert", new List<Recipe>() { new Recipe(RecipeType.Dessert, "Tiramisu", 4, 1200), new Recipe(RecipeType.Dessert, "Creme brule", 3, 450) });

            DiningRoomModel diningRoomModel = new DiningRoomModel(menu: menu);
            diningRoomModel.Run(new CustomersFactory(5, 5));
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
