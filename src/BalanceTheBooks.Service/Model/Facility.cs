namespace BalanceTheBooks.Service.Model
{
    public class Facility
    {
        public long BankId { get; set; }
        public long Id { get; set; }
        public float InterestRate { get; set; }
        public int Amount { get; set; }
        public int CommittedAmount { get; set; }
        public float Yield { get; set; }
    }
}
