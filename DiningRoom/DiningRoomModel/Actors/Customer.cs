using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinnerSimulator.DiningRoom.Model.Actors
{
/*
    enum CustomerChoiceMenu
    {
        entry,
        dish,
        dessert,
        entryAndDish,
        entryAndDessert,
        dishAndDessert,
        all
    };
*/
    class Customer
    {
        private CustomerChoiceMenu choiceMenu;
        private List<string> preferenceList;
        private int presenceTime;
    }
}
