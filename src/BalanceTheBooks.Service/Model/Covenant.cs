namespace BalanceTheBooks.Service.Model
{
    public class Covenant
    {
        public long BankId { get; set; }
        public long FacilityId { get; set; }
        public float MaxDefaultLikelihood { get; set; }
        public string BannedState { get; set; }
    }
}
