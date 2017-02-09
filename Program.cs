using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentCollections
{
    class Program
    {
        public static readonly List<string> AllShirtNames =
                new List<string> { "Facebook", "Google", "Amazon", "Microsoft", "Schlumberger" };

        static void Main(string[] args)
        {
            BonusCalculator bonusCalculator = new BonusCalculator();
            ToDoQueue toDoQueue = new ToDoQueue(bonusCalculator);

            StockController controller = new StockController(toDoQueue);

            SalesPerson[] sales =
            {
                new SalesPerson("Mark"),
                new SalesPerson("Larry"),
                new SalesPerson("Bill"),
                new SalesPerson("Paal")
            };

            TimeSpan workDay = new TimeSpan(0, 0, 1);

            Task t1 = Task.Run(() => sales[0].Work(controller, workDay));
            Task t2 = Task.Run(() => sales[1].Work(controller, workDay));
            Task t3 = Task.Run(() => sales[2].Work(controller, workDay));
            Task t4 = Task.Run(() => sales[3].Work(controller, workDay));

            Task bonusLogger = Task.Run(() => toDoQueue.MonitorAndLogTrades());
            Task bonusLogger2 = Task.Run(() => toDoQueue.MonitorAndLogTrades());


            Task.WaitAll(t1, t2, t3, t4);
            toDoQueue.CompleteAdding();
            Task.WaitAll(bonusLogger, bonusLogger2);

            controller.DisplayStatus();
            bonusCalculator.DisplayReport(sales);

            //var stock = new ConcurrentDictionary<string, int>();
            //stock.TryAdd("A",4);
            //stock.TryAdd("B",5);
            //Console.WriteLine(string.Format("No. of T-shirts in stock {0}",stock.Count));

            //stock.TryAdd("C",6);
            //stock["D"] = 7;
            //Console.WriteLine(string.Format("\r\nstock[D] =  {0}", stock["D"]));

            //int x;
            //stock.TryRemove("A",out x);
            //Console.WriteLine("\r\nEnumerating:");
            //foreach (var s in stock)
            //{
            //    Console.WriteLine("{0} : {1}", s.Key, s.Value);
            //}



            //var c_orders = new ConcurrentQueue<string>();
            //var orders = new Queue<string>();

            //PlaceOrders(orders, "Jia Guo");
            //PlaceOrders(orders, "Li RAO");
            ////Task task1 = Task.Run(() => PlaceConcurrentOrders(c_orders, "Sam"));
            ////Task task2 = Task.Run(() => PlaceConcurrentOrders(c_orders, "Li"));
            ////Task.WaitAll(task1, task2);

            //foreach (var o in orders)
            //{
            //    Console.WriteLine("ORDER: " + o);
            //}

        }

        private static object _lock = new object();

        private static void PlaceOrders(Queue<string> orders, string name)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(2);
                string orderName = string.Format("{0} wants t-shirt {1}", name, i + 1);
                lock (_lock)
                {
                    orders.Enqueue(orderName);
                }
            }
        }

        private static void PlaceConcurrentOrders(ConcurrentQueue<string> orders, string name)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(2);
                string orderName = string.Format("{0} wants t-shirt {1}", name, i + 1);
                orders.Enqueue(orderName);
            }
        }
    }
}
