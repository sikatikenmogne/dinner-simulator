using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Model.Actors;
using System.Runtime.InteropServices;

namespace DinnerSimulator.DiningRoom.Model.Strategy
{
    class RandomOrderStrategy : IOrderStrategy
    {
        public Order Order(CustomerGroup customer, MenuCard menuCard)
        {

            Random rand = new Random();

            int choiceRange = System.Enum.GetNames(typeof(CustomerChoiceMenuEnum)).Length;
            
            int MenuChoice = rand.Next(0, choiceRange);

            CustomerChoiceMenuEnum customerChoiceMenu = (CustomerChoiceMenuEnum)MenuChoice;

            string[] keyList = null;
            Order order = new Order();
            //order.orderLine = new Dictionary<Recipe, int>();
            switch (customerChoiceMenu)
            {
                case CustomerChoiceMenuEnum.all:

                    keyList = new string[3] { "Entry", "Dish", "Dessert" };
                            
                    break;

                case CustomerChoiceMenuEnum.dishAndDessert:

                    keyList = new string[2] { "Dish", "Dessert" };

                    break;
                case CustomerChoiceMenuEnum.entryAndDish:

                    keyList = new string[2] { "Entry", "Dish" };


                    break;
                case CustomerChoiceMenuEnum.entryAndDessert:

                    keyList = new string[2] { "Entry", "Dessert" };


                    break;
                case CustomerChoiceMenuEnum.dish:

                    keyList = new string[1] { "Dish" };


                    break;
                case CustomerChoiceMenuEnum.entry:

                    keyList = new string[1] { "Entry" };

                    break;
                case CustomerChoiceMenuEnum.dessert:

                    keyList = new string[1] { "Dessert" };

                    break;
            }

            if(menuCard != null)
            {

                foreach (string key in keyList)
                {
                    Dictionary<string, List<Recipe>> menu = menuCard.Menu;

                    /*  
                            Dictionary<string, List<Recipe>> menu = new Dictionary<string, List<Recipe>>();

                            menu.Add("Entry", new List<Recipe>() { new Recipe(RecipeType.Entry, "Feuillete de crabe", 4, 1000), new Recipe(RecipeType.Entry, "Oeufs cocotte", 4, 200) });
                            menu.Add("Dish", new List<Recipe>() { new Recipe(RecipeType.Dish, "Bouillinade anguilles ou poissons", 4, 345), new Recipe(RecipeType.Dish, "Boles de picoulats", 25, 560) });
                            menu.Add("Dessert", new List<Recipe>() { new Recipe(RecipeType.Dessert, "Tiramisu", 4, 1200), new Recipe(RecipeType.Dessert, "Creme brule", 3, 450) });

                            menuCard.Menu = menu;
                    */

                    if (menu.ContainsKey(key))
                    {
                        List<Recipe> recipeList = menu[key];
                        int choice = rand.Next(0, recipeList.Count);
                        
                        order.orderLine.Add(recipeList[choice], 1);
                    }
                }
            }

            
            return order;
        }
    }
}
