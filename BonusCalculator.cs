using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ConcurrentCollections
{
    public class BonusCalculator
    {
        private ConcurrentDictionary<SalesPerson, int> _salesByPerson = new ConcurrentDictionary<SalesPerson, int>();
        private ConcurrentDictionary<SalesPerson, int> _purchasesByPerson = new ConcurrentDictionary<SalesPerson, int>();
 
        public void ProcessTrade(Trade trade)
        {
            Thread.Sleep(300);
            if (trade.QuantitySold > 0)
            {
                _salesByPerson.AddOrUpdate(trade.Person, trade.QuantitySold,
                (key, oldValue) => oldValue + trade.QuantitySold);
            }
            else
            {
                _purchasesByPerson.AddOrUpdate(trade.Person, -trade.QuantitySold,
                (key, oldValue) => oldValue - trade.QuantitySold);
            }
            
        }

        public void DisplayReport(SalesPerson[] people)
        {
            Console.WriteLine();
            Console.WriteLine("Transactions by salesperson:");
            foreach (var p in people)
            {
                int sales = _salesByPerson.GetOrAdd(p, 0);
                int purchase = _purchasesByPerson.GetOrAdd(p, 0);
                Console.WriteLine("{0,15} sold {1,3}, bought {2,3} items, total {3}", p.Name, sales, purchase, sales + purchase);
            }
        }
    }
}
