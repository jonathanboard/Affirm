using BalanceTheBooks.Service.Model;
using System.Net;

namespace BalanceTheBooks.Service
{
    public interface IBalanceService
    {
        HttpStatusCode BalanceLoan(Loan loanData);
        HttpStatusCode SaveYeilds();
    }
}
