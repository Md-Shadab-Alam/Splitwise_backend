namespace Splitwise.Entities
{
    public class Transaction
    {
        public int Payer {  get; set; }
        public int Receiver { get; set; }
        public decimal Amount { get; set; } 
    }
}
