using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DinnerSimulator.DiningRoom.Model;
using DinnerSimulator.DiningRoom.Model.Actors;
using DinnerSimulator.Common.Model;
using DinnerSimulator.DiningRoom.Controller.Actors;
using DinnerSimulator.DiningRoom.Controller.Strategy;
using DinnerSimulator.DiningRoom.Controller.Observer;
using DinnerSimulator.DiningRoom.Controller.Factory;
using System.Threading;

namespace DinnerSimulator.DiningRoom.Controller
{
    public class DiningRoomController : IObservable<Order>
    {

        private HotelMasterController hotelMasterController;
        public DiningRoomModel diningRoomModel;

        private List<CustomerController> customerControllers;
        private List<LineChiefController> lineChiefControllers;
        
        static int costomerCount = 0;
        
        private static Queue<CustomerGroup> CustomerQueue = new Queue<CustomerGroup>();
        private static ManualResetEvent customerQueueMre = new ManualResetEvent(false);
        private static Mutex customerQueueMtx = new Mutex();

        private static Queue<Order> OrderList = new Queue<Order>();
        public Queue<Order> OrderListing { get => OrderList; set => OrderList = value; }

        private static List<Thread> orderThreads = new List<Thread>();

        private List<IObserver<Order>> observers;


        public DiningRoomController(DiningRoomModel diningRoomModel)
        {
            this.diningRoomModel = diningRoomModel;
            this.hotelMasterController = new HotelMasterController(diningRoomModel);
            this.lineChiefControllers = new List<LineChiefController>();
            this.observers = new List<IObserver<Order>>();
        }
        public void Run()
        {

            CustomersFactory factory = new CustomersFactory();
            factory.Subscribe(hotelMasterController);

            CustomersArrival(factory, 3);

            Thread takeOrderThread = new Thread(() =>
            {
                while (true)
                {
                    customerQueueMtx.WaitOne();
                    if (CustomerQueue.Count != 0)
                    {
                        int id = costomerCount++;
                        Console.WriteLine("=========Client n_" + id + " commande=======");

                        CustomerGroup clt = CustomerQueue.Dequeue();
                        clt.CustomerState = CustomerState.Ordering;
                        CustomerController customerController = new CustomerController(clt, new RandomOrderStrategy());

                        orderThreads.Add(new Thread(() => CustomerOrder(customerController)));
                        orderThreads[orderThreads.Count - 1].Name = "Commande n_" + (orderThreads.Count - 1) + " du client n_" + id;
                        orderThreads[orderThreads.Count - 1].Start();
                    }
                    customerQueueMtx.ReleaseMutex();
                }
            });
            takeOrderThread.Name = "Take_order";
            takeOrderThread.Start();

            Thread.Sleep(1000);
            Console.WriteLine("========== " + OrderListing.Count + " Commande(s) ont ete prise. ==========");

            foreach(Order comm in OrderListing)
            {
                foreach(KeyValuePair<Recipe, int> dic in comm.orderLine)
                {
                    Console.WriteLine("======= " + dic.Key.RecipeTitle + " : " + dic.Value + " =======");
                }
            }

        }

        public void TakeOrder()
        {
            while (true)
            {
                customerQueueMtx.WaitOne();
                if (CustomerQueue.Count != 0)
                {
                    int id = costomerCount++;
                    Console.WriteLine("=========Client n_" + id + " commande=======");

                    CustomerGroup clt = CustomerQueue.Dequeue();
                    clt.CustomerState = CustomerState.Ordering;
                    CustomerController customerController = new CustomerController(clt, new RandomOrderStrategy());

                    orderThreads.Add(new Thread(() => CustomerOrder(customerController)));
                    orderThreads[orderThreads.Count - 1].Name = "Commande n_" + (orderThreads.Count - 1) + " du client n_" + id;
                    orderThreads[orderThreads.Count - 1].Start();
                }
                customerQueueMtx.ReleaseMutex();
            }
        }

        public void CustomersArrival(CustomersFactory factory,int nbCustomersGroup)
        {
            for (int i = 0; i < nbCustomersGroup; i++)
            {
                CustomerQueue.Enqueue(factory.CreateCustomers(4));
                customerQueueMre.Set();
            }
        }

        public void CustomerOrder(CustomerController customerController)
        {
            customerQueueMtx.WaitOne();
            int[] table = FindCustomerTable(customerController.Customer);

            if(table != null)
            {                
                OrderList.Enqueue(customerController.Order(DiningRoomModel.Squares[table[0]].Lines[table[1]].Tables[table[2]].MenuCard));
                Order order = OrderList.Peek();
                foreach(IObserver<Order> observer in this.observers)
                {
                    observer.OnNext(order);
                }
                customerController.Customer.CustomerState = CustomerState.Ordered;
                Thread thread = Thread.CurrentThread;
                Console.WriteLine("========="+thread.Name + " prise========");
            }

            customerQueueMtx.ReleaseMutex();
        }

        public int[] FindCustomerTable(CustomerGroup customer)
        {
            int nbSquare = DiningRoomModel.Squares.Count;
            for (int i = 0; i < nbSquare; i++)
            {
                int nbLine = DiningRoomModel.Squares[i].Lines.Count;
                for (int j = 0; j < nbLine; j++)
                {
                    int nbTable = DiningRoomModel.Squares[i].Lines[j].Tables.Count;
                    for (int k = 0; k < nbTable; k++)
                    {
                        if (customer == DiningRoomModel.Squares[i].Lines[j].Tables[k].Group )
                        {
                            //return diningRoomModel.Squares[i].Lines[j].Tables[k];
                            return new int[3] { i, j, k };
                        }
                    }
                }
            }
            return null;
        }

        public IDisposable Subscribe(IObserver<Order> observer)
        {
            if (!observers.Contains(observer)){
                observers.Add(observer);
            }
            return new DRUnsubscriber<Order>(observers, observer);
        }
    }
}
