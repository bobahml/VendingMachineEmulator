namespace VendingMachineBL
{
    public class Coin
    {
        public Coin(int denomination, int count)
        {
            Denomination = denomination;
            Count = count;
        }

        public int Denomination { get; private set; }
        public int Count { get; set; }
    }
}