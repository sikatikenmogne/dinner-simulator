using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinnerSimulator.Common.Model
{
    public enum OrderStatus
    {
        inCooking,
        updated,
        registred,
        delivered,
        ready,
    }

    public class Order
    {

        private OrderStatus orderStatus;
        private Dictionary<Recipe, int> plats = new Dictionary<Recipe, int>();

        public OrderStatus State { get => orderStatus; set => orderStatus = value; }
        public Dictionary<Recipe, int> orderLine { get => plats; set => plats = value; }

        public Order()
        {
            plats = new Dictionary<Recipe, int>();
        }


        public int tableNumber { get; set; }
        public int nbOrders { get; set; }
        public double price { get; set; }
        public Recipe recipe { get; set; }
        public Order(int tablenumber, int nborders, double price, Recipe recipe)
        {
            this.tableNumber = tablenumber;
            this.nbOrders = nborders;
            this.price = price;
            this.recipe = recipe;
        }
    }
}
