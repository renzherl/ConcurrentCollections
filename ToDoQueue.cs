using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ConcurrentCollections
{
    public class ToDoQueue
    {
        private readonly ConcurrentQueue<Trade> _queue = new ConcurrentQueue<Trade>();
        private Boolean _workingDayComplete = false;
        private readonly BonusCalculator _bonusCalculator;

        public ToDoQueue(BonusCalculator bonusCalculator)
        {
            _bonusCalculator = bonusCalculator;
        }

        public void AddTrade(Trade trade)
        {
            _queue.Enqueue(trade);
        }

        public void CompleteAdding()
        {
            _workingDayComplete = true;
        }

        public void MonitorAndLogTrades()
        {
            while (true)
            {
                Trade nextTrade;
                bool hasNextTrade = _queue.TryDequeue(out nextTrade);
                if (hasNextTrade)
                {
                    _bonusCalculator.ProcessTrade(nextTrade);
                    Console.WriteLine("Processing transaction from " + nextTrade.Person.Name);
                }
                else if (_workingDayComplete)
                {
                    Console.WriteLine("No more sales to log - exiting");
                    return;
                }
                else
                {
                    Console.WriteLine("No more transaction");
                    Thread.Sleep(500);
                }
            }
        }


    }
}
