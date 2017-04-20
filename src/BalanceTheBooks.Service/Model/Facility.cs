namespace BalanceTheBooks.Service.Model
{
    public class Facility
    {
        public long BankId { get; set; }
        public long Id { get; set; }
        public float InterestRate { get; set; }
        public int Amount { get; set; }
        public int CommitedAmount { get; set; }
        public float Yeild { get; set; }
    }
}
