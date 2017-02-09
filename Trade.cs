namespace ConcurrentCollections
{
    public class Trade
    {
        public SalesPerson Person { get; private set; }

        //negative QuantitySold means buy
        public int QuantitySold { get; private set; }

        public Trade(SalesPerson person, int quantity)
        {
            this.Person = person;
            this.QuantitySold = quantity;
        }

    }
}
