using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Model.Actors;
using DinnerSimulator.DiningRoom.Model.Elements;
using DinnerSimulator.DiningRoom.Model.Factory;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DinnerSimulator.DiningRoom.Model
{
    public class DiningRoomModel
    {

        private MenuCard menuCard;

        public HotelMaster HotelMaster { get; set; }
        public static List<Square> Squares { get; set; }
        public static List<LineChief> LineChiefs { get; set; }
        public static Queue<MenuCard> MenuCards { get; set; }
        public MenuCard MenuCard { get => menuCard; set => menuCard = value; }
        public Dictionary<string, List<Recipe>> Menu { get; set; }
        public Queue<Order> OrderList { get; set; }
        
        public Queue<BreadBasket> BreadBaskets { get; set; }
        public Queue<BottleOfWater> BottleOfWaters { get; set; }


        static int costomerCount = 0;

        private static Queue<CustomerGroup> customerQueue = new Queue<CustomerGroup>();

        private static ManualResetEvent customerQueueMre = new ManualResetEvent(false);
        private static Mutex customerQueueMtx = new Mutex();

        private static ManualResetEvent orderQueueMre = new ManualResetEvent(false);
        private static Queue<CustomerGroup> orderQueue = new Queue<CustomerGroup>();

        private static List<Thread> orderThreads = new List<Thread>();

        static bool stillordering = true;

        public static int nbBottleOfWaterSetting;

        public static int nbBreadBasketSetting;

        private int waiterCustomerCount;

        public DiningRoomModel(int nbSquares = 2, int nbLines = 2, int nbTablesPerLine = 10, int nbSeatsPerTable = 0, int nbLineChiefs = 1, int nbWaiters = 2, int nbMenuCard = 40,int waiterCustomerCount = 6, int nbBreadBasket = 40, int nbBreadBasketSetting = 2, int nbBottleOfWater = 40, int nbBottleOfWaterSetting = 2, Dictionary<string, List<Recipe>> menu = null)
        {
            HotelMaster = new HotelMaster();

            Squares = new List<Square>();

            OrderList = new Queue<Order>();

            for (int i = 0; i < nbSquares; i++)
                Squares.Add(new Square());

            for (int i = 0; i < nbSquares; i++)
            {
                for (int j = 0; j < nbLines; j++)
                {
                    int nbSPerTable = 0;

                    if (nbSeatsPerTable == 0)
                    {
                        Random rand = new Random();
                        nbSPerTable = rand.Next(1, 6);
                        nbSeatsPerTable = nbSPerTable;
                    }

                    Line line = new Line(nbTablesPerLine, nbSeatsPerTable);

                    Squares[i].Lines.Add(line);
                }
            }

            for (int i = 0; i < nbSquares; i++)
            {
                for (int j = 0; j < nbLineChiefs; j++)
                    Squares[i].LineChiefs.Add(new LineChief());

                for (int k = 0; k < nbWaiters; k++)
                    Squares[k].Waiters.Add(new Waiter());
            }

            MenuCards = new Queue<MenuCard>();
            for (int i = 0; i < nbMenuCard; i++)
            {
                MenuCard menuCard = new MenuCard();
                menuCard.Menu = menu;
                MenuCards.Enqueue(menuCard);
            }


            BreadBaskets = new Queue<BreadBasket>();

            BottleOfWaters = new Queue<BottleOfWater>();

            for (int i = 0; i < nbBreadBasket; i++)
                BreadBaskets.Enqueue(new BreadBasket());

            for (int i = 0; i < nbBottleOfWater; i++)
                BottleOfWaters.Enqueue(new BottleOfWater());

            DiningRoomModel.nbBottleOfWaterSetting = nbBottleOfWaterSetting;

            DiningRoomModel.nbBreadBasketSetting = nbBreadBasketSetting;

            this.waiterCustomerCount = waiterCustomerCount;
        }



        public void RunSimulation(CustomersFactory factory)
        {

            Thread customerArrivalThread = new Thread(() => CustomersArrival(factory));
            customerArrivalThread.Name = "Customers_arrival";
            customerArrivalThread.Start();

            Thread customerOrderingThread = new Thread(DiningRoomService);
            customerOrderingThread.Name = "Dining_room_service";
            customerOrderingThread.Start();

            Thread.Sleep(1000 * 5);

            customerOrderingThread.Join();

            CustomersOrderRecap();

            Thread.Sleep(1000);

        }


        private void CustomersArrival(CustomersFactory factory)
        {
            for (int i = 0; i < CustomersFactory.MaxCustomersCount; i++)
            {
                int add = CustomersFactory.CustomersArrivalRythm;
                if (CustomersFactory.CustomersArrivalRythm != 0)
                    Thread.Sleep(1000 * add);
                else
                {
                    Random rnd = new Random();
                    Thread.Sleep(1000 * rnd.Next(1, 4));
                }

                CustomerGroup customerGroup = factory.CreateCustomers(factory.DefaultCustomersCountPerGroup);
                customerQueue.Enqueue(customerGroup);
                Console.WriteLine("=============== " + i + "===============");
                customerQueueMre.Set();
            }
            stillordering = false;
        }


        private void DiningRoomService()
        {
            while (stillordering)
            {
                customerQueueMtx.WaitOne();
                if (customerQueue.Count != 0)
                {
                    CustomerGroup customers = customerQueue.Dequeue();
                    int[] tablePosition = HotelMaster.ReceiveCustomers(customers);
                    Console.WriteLine("==== ===");
                    if (tablePosition != null)
                    {
                        customers.CustomerState = CustomerState.WaitLineChief;

                        LineChief lineChief = null;
                        
                        do
                        {
                            lineChief = DiningRoomModel.GetAviableLineChief(tablePosition[0]);
                        } while (lineChief == null);

                        Waiter waiter = null;

                        lineChief.Available = false;

                        Console.WriteLine("Chef de rang: Oui");

                        Console.WriteLine("Chef de rang: Si vous voulez bien me suivre.");

                        lineChief.InstallCustomers(customers, tablePosition);
                        Console.WriteLine("=========Clients installé=========");

                        lineChief.SetMenuCardOnTable(DiningRoomModel.MenuCards, tablePosition);
                        Console.WriteLine("=======Carte de menu deposée=======");

                        lineChief.Available = true;

                        CustomersOrder(customers);

                        orderQueue.Enqueue(customers);

                        do
                        {
                            waiter = DiningRoomModel.GetAviableWaiter(tablePosition[0]);
                        } while (waiter == null);

                        waiter.Available = false;
                        if (DiningRoomModel.Squares[tablePosition[0]].Lines[tablePosition[1]].Tables[tablePosition[2]].Group.Count >= waiterCustomerCount)
                        {
                            waiter.SetBreadBasketOnTable(tablePosition, this.BreadBaskets, DiningRoomModel.nbBreadBasketSetting);
                            Console.WriteLine($"======= {DiningRoomModel.nbBreadBasketSetting} Corbeilles de pain dépossé=======");

                            waiter.SetBottleOfWaterOnTable(tablePosition, this.BottleOfWaters, DiningRoomModel.nbBottleOfWaterSetting);
                            Console.WriteLine($"======= {DiningRoomModel.nbBreadBasketSetting} Bouteilles d'eau dépossé=======");
                        }
                        waiter.Available = true;
               
                        
                    }
                    else
                        Console.WriteLine("Error: : no line chief aviable");
                    costomerCount++;
                }
                customerQueueMtx.ReleaseMutex();
                orderQueueMre.Set();
            }
        }

        private void CustomersOrder(CustomerGroup customers)
        {

            customers.CustomerState = CustomerState.Ordering;
            Console.WriteLine("=========Client n_" + costomerCount + " commande=======");

            orderThreads.Add(new Thread(() =>
            {
                customerQueueMtx.WaitOne();

                int[] tablePosition = customers.TablePosition;

                if (tablePosition != null)
                {
                    OrderList.Enqueue(customers.Order(DiningRoomModel.Squares[tablePosition[0]].Lines[tablePosition[1]].Tables[tablePosition[2]].MenuCard));

                    customers.CustomerState = CustomerState.Ordered;
                    Thread thread = Thread.CurrentThread;
                    Console.WriteLine("=========" + thread.Name + " prise========");
                }

                customerQueueMtx.ReleaseMutex();
            }));

            orderThreads[orderThreads.Count - 1].Name = "Commande n_" + (orderThreads.Count - 1) + " du client n_" + (costomerCount + 1);

            Console.WriteLine("orderThreads.Count : " + orderThreads.Count);

            orderThreads[orderThreads.Count - 1].Start();
        }

        private void CustomersOrderRecap()
        {

            Console.WriteLine("========== " + OrderList.Count + " Commande(s) ont ete prise. ==========");

            foreach (Order comm in OrderList.ToArray())
            {
                foreach (KeyValuePair<Recipe, int> dic in comm.orderLine)
                {
                    Console.WriteLine("======= " + dic.Key.RecipeTitle + " : " + dic.Value + " =======");
                }
                Console.WriteLine("---------- OrderLine end ----------");
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

        public static Waiter GetAviableWaiter(int squareId)
        {
            foreach (Waiter waiter in DiningRoomModel.Squares[squareId].Waiters)
            {
                if (waiter.Available)
                    return waiter;
            }

            return null;
        }


    }
}
