using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Model.Actors;
using DinnerSimulator.DiningRoom.Model.Elements;
using DinnerSimulator.DiningRoom.Model.Factory;
using DinnerSimulator.DiningRoom.Model.Strategy;

using System.Threading;

namespace DinnerSimulator.DiningRoom.Model
{
    public class DiningRoomModel
    {
        private HotelMaster hotelMaster;
        private  List<RoomClerk> roomclerks;
        private  List<Waiter> waiters;
        private static List<Square> squares;
        private static List<LineChief> lineChiefs;
        private MenuCard menuCard;
        private static Queue<MenuCard> menuCards;

        Dictionary<string, List<Recipe>> menu;

        public HotelMaster HotelMaster { get => hotelMaster; set => hotelMaster = value; }
        public List<RoomClerk> RoomClerks { get => roomclerks; set => roomclerks = value; }
        public List<Waiter> Waiters { get => waiters; set => waiters = value; }
        public static List<Square> Squares { get => squares; set => squares = value; }
        public static List<LineChief> LineChiefs { get => lineChiefs; set => lineChiefs = value; }
        public static Queue<MenuCard> MenuCards { get => menuCards; set => menuCards = value; }
        public MenuCard MenuCard { get => menuCard; set => menuCard = value; }
        public Dictionary<string, List<Recipe>> Menu { get => menu; set => menu = value; }


        static int costomerCount = 0;

        private static Queue<CustomerGroup> customerQueue = new Queue<CustomerGroup>();

        private static ManualResetEvent customerQueueMre = new ManualResetEvent(false);
        private static Mutex customerQueueMtx = new Mutex();

        private static ManualResetEvent orderQueueMre = new ManualResetEvent(false);
        private static Mutex orderQueueMtx = new Mutex();

        private static Queue<CustomerGroup> orderQueue = new Queue<CustomerGroup>();

        private static Queue<Order> orderList = new Queue<Order>();
        private static List<Thread> orderThreads = new List<Thread>();

        private static AutoResetEvent Are = new AutoResetEvent(false);
        private static Mutex orderRecapMtx = new Mutex();


        public Queue<Order> OrderList { get => orderList; set => orderList = value; }


        public DiningRoomModel()
        {
            menu = new Dictionary<string, List<Recipe>>();
            
            menu.Add("Entry",new List<Recipe>() { new Recipe(RecipeType.Entry, "Feuillete de crabe",4, 1000), new Recipe(RecipeType.Entry, "Oeufs cocotte",4,200) });
            menu.Add("Dish", new List<Recipe>() { new Recipe(RecipeType.Dish, "Bouillinade anguilles ou poissons",4,345), new Recipe(RecipeType.Dish, "Boles de picoulats", 25, 560) });
            menu.Add("Dessert", new List<Recipe>() { new Recipe(RecipeType.Dessert, "Tiramisu", 4, 1200), new Recipe(RecipeType.Dessert,"Creme brule", 3, 450) });

            menuCard = new MenuCard();

            menuCard.Menu = menu;

            hotelMaster = new HotelMaster();
            
            squares = new List<Square>();

            roomclerks = new List<RoomClerk>();

            squares.Add(new Square());

            menuCards = new Queue<MenuCard>();

            for(int i = 0; i < 40; i++)
            {
                MenuCard mCard = new MenuCard();
                mCard.Menu = menu;
                menuCards.Enqueue(mCard);
            }

            squares[0].Lines.Add(new Line(4,6));
            //squares[0].Lines.Add(new Line());
            squares[0].Waiters.Add(new Waiter());

            squares[0].LineChiefs.Add(new LineChief());
            
            roomclerks.Add(new RoomClerk());
        }
        public DiningRoomModel(int nbSquares = 2, int nbLines = 2, int nbTablesPerLine = 10, int nbSeatsPerTable = 0, int nbLineChiefs = 1, int nbWaiters = 2, int nbRoomClerks = 1, int nbMenuCard = 40, Dictionary<string, List<Recipe>> menu = null)
        {
            hotelMaster = new HotelMaster();

            squares = new List<Square>();

            roomclerks = new List<RoomClerk>();

            for (int i=0; i < nbSquares; i++)
                squares.Add(new Square());

            for (int i = 0; i < nbSquares; i++)
            {
                for (int j = 0; j < nbLines; j++)
                {
                    Line line;

                    if (nbSeatsPerTable == 0)
                        line = new Line(nbTablesPerLine);
                    else
                        line = new Line(nbTablesPerLine, nbSeatsPerTable);

                    squares[i].Lines.Add(line);
                }
            }

            for (int i = 0; i < nbSquares; i++)
            {
                for (int j = 0; j < nbLineChiefs; j++)
                    squares[i].LineChiefs.Add(new LineChief());
           
                for (int k = 0; k < nbWaiters; k++)
                    squares[i].Waiters.Add(new Waiter());
            }

            for (int i = 0; i < nbRoomClerks; i++)
                roomclerks.Add(new RoomClerk());

            menuCards = new Queue<MenuCard>();
            for (int i = 0; i < nbMenuCard; i++)
            {
                MenuCard menuCard = new MenuCard();
                menuCard.Menu = menu;
                menuCards.Enqueue(menuCard);
            }
        }


        public static int[] GetSuitableTablePosition(int customersCount)
        {
            for (int i = 0; i < Squares.Count; i++)
            {
                for (int j = 0; j < Squares[i].Lines.Count; j++)
                {
                    for (int k = 0; k < Squares[i].Lines[j].Tables.Count; k++)
                    {
                        if (customersCount <= Squares[i].Lines[j].Tables[k].NbPlaces && Squares[i].Lines[j].Tables[k].State == EquipmentState.Available)
                        {
                            Console.WriteLine(customersCount + "  :  " + Squares[i].Lines[j].Tables[k].NbPlaces);
                            return new int[3] { i, j, k };
                        }
                    }
                }
            }
            return null;
        }

        public static LineChief GetAviableLineChief(int squareId)
        {
            foreach (LineChief lineChief in DiningRoomModel.Squares[squareId].LineChiefs)
            {
                if (lineChief.Available)
                    return lineChief;
            }
            
            return null;
        }

        public void Run(CustomersFactory factory)
        { 
            int waiter = 0;
            Thread customerArrivalThread = new Thread(() => {
                for (int i = 0; i < CustomersFactory.MaxCustomersCount; i++)
                {
                    int add = CustomersFactory.CustomerArrivalRythm;
                    if (CustomersFactory.CustomerArrivalRythm != 0)
                        Thread.Sleep(1000 * add);
                    else
                    {
                        Random rnd = new Random();
                        Thread.Sleep(1000 * rnd.Next(1, 4));
                    }
                    waiter += add;
                    CustomerGroup customerGroup = factory.CreateCustomers(factory.DefaultCustomersCountPerGroup);
                    customerQueue.Enqueue(customerGroup);
                    customerQueueMre.Set();
                }
            });
            customerArrivalThread.Name = "Customers_arrival";
            customerArrivalThread.Start();


            Thread customerOrderingThread = new Thread(CustomerHome);
            customerOrderingThread.Name = "Customers_home";
            customerOrderingThread.Start();


            Thread customerOrderThread = new Thread(CustomersOrder);
            customerOrderThread.Name = "Customers_order";
            customerOrderThread.Start();

           
            Thread.Sleep(1000 * 9);

            // Thread to synchronize
            
            /*
            Thread customerOrderingRecapThread = new Thread(()=> {

                while (true)
                {
                    orderRecapMtx.WaitOne();
                    if (CustomersFactory.MaxCustomersCount == costomerCount)
                    {
            */
                        Console.WriteLine("========== " + OrderList.Count + " Commande(s) ont ete prise. ==========");
            
                        foreach (Order comm in OrderList.ToArray())
                        {
                            foreach (KeyValuePair<Recipe, int> dic in comm.orderLine)
                            {
                                Console.WriteLine("======= " + dic.Key.RecipeTitle + " : " + dic.Value + " =======");
                            }
                            Console.WriteLine("---------- OrderLine end ----------");
                        }
            /*
                    }
                    orderRecapMtx.ReleaseMutex();
                }

            });
            
            customerOrderingRecapThread.Name = " customerOrderingRecapThread";
            customerOrderingRecapThread.Start();
            */

        }

        private void CustomerHome()
        {
            while (true)
            {
                customerQueueMtx.WaitOne();
                if (customerQueue.Count != 0)
                {
                    CustomerGroup customers = customerQueue.Dequeue();
                    int[] tablePosition = hotelMaster.ReceiveCustomers(customers);
                    Console.WriteLine("==== ===");
                    if (tablePosition != null)
                    {
                        customers.CustomerState = CustomerState.WaitLineChief;

                        LineChief lineChief = DiningRoomModel.GetAviableLineChief(tablePosition[0]);

                        if (lineChief != null)
                        {
                            lineChief.Available = false;

                            Console.WriteLine("Chef de rang: Oui");

                            Console.WriteLine("Chef de rang: Si vous voulez bien me suivre.");

                            lineChief.InstallCustomers(customers, tablePosition);
                            Console.WriteLine("=========Clients installé=========");

                            lineChief.SetMenuCardOnTable(DiningRoomModel.MenuCards, tablePosition);
                            Console.WriteLine("=======Carte de menu deposée=======");

                            lineChief.Available = true;

                            orderQueue.Enqueue(customers);
                        }

                    }
                    else
                        Console.WriteLine("Error: : no line chief aviable");
                    costomerCount++;
                }
                customerQueueMtx.ReleaseMutex();
                orderQueueMre.Set();
            }
        }

        private void CustomersOrder()
        {
            while (true)
            {
                orderQueueMtx.WaitOne();

                if(orderQueue.Count > 0)
                {
                    CustomerGroup customers = orderQueue.Dequeue();
                    customers.CustomerState = CustomerState.Ordering;
                    Console.WriteLine("=========Client n_" + costomerCount + " commande=======");

                    orderThreads.Add(new Thread(() => {
                        customerQueueMtx.WaitOne();

                        int[] tablePosition = customers.TablePosition;

                        if (tablePosition != null)
                        {
                            orderList.Enqueue(customers.Order(DiningRoomModel.Squares[tablePosition[0]].Lines[tablePosition[1]].Tables[tablePosition[2]].MenuCard));

                            customers.CustomerState = CustomerState.Ordered;
                            Thread thread = Thread.CurrentThread;
                            Console.WriteLine("=========" + thread.Name + " prise========");
                        }

                        customerQueueMtx.ReleaseMutex();
                    }));

                    orderThreads[orderThreads.Count - 1].Name = "Commande n_" + (orderThreads.Count - 1) + " du client n_" + (costomerCount + 1);
                    orderThreads[orderThreads.Count - 1].Start();
                }
                orderQueueMtx.ReleaseMutex();
                //Are.Set();
            }
        }
    }
}
