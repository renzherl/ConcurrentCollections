using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Threading;

namespace ConcurrentCollections
{
    public class ToDoQueue
    {
        private readonly BlockingCollection<Trade> _queue = new BlockingCollection<Trade>();
        private readonly BonusCalculator _bonusCalculator;

        public ToDoQueue(BonusCalculator bonusCalculator)
        {
            _bonusCalculator = bonusCalculator;
        }

        public void AddTrade(Trade trade)
        {
            _queue.Add(trade);
        }

        public void CompleteAdding()
        {
            _queue.CompleteAdding();
        }

        public void MonitorAndLogTrades()
        {
            while (true)
            {
                try
                {
                    Trade nextTrade =_queue.Take();
                    _bonusCalculator.ProcessTrade(nextTrade);
                    Console.WriteLine("Processing transaction from " + nextTrade.Person.Name);
                }
                catch(InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
        }


    }
}
