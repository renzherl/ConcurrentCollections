using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

            StockController controller = new StockController();
            TimeSpan workDay = new TimeSpan(0, 0, 2);

            Task t1 = Task.Run(() => new SalesPerson("Mark").Work(controller, workDay));
            Task t2 = Task.Run(() => new SalesPerson("Larry").Work(controller, workDay));
            Task t3 = Task.Run(() => new SalesPerson("Bill").Work(controller, workDay));
            Task t4 = Task.Run(() => new SalesPerson("Paal").Work(controller, workDay));

            Task.WaitAll(t1, t2, t3, t4);
            controller.DisplayStatus();

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
