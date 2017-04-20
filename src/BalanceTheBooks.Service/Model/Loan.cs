namespace BalanceTheBooks.Service.Model
{
    public class Loan
    {
        public long LoanId { get; set; }
        public int Amount { get; set; }
        public float InterestRate { get; set; }
        public float DefaultLikelihoodOfDefault { get; set; }
        public string OriginationState { get; set; }
        public float LoanYield { get; set; }
        public Facility CreditFacility { get; set; }
    }
}
