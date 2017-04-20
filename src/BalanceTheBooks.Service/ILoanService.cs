using BalanceTheBooks.Service.Model;
using System.Net;

namespace BalanceTheBooks.Service
{
    public interface ILoanService
    {
        Loan GetNextLoan();
        HttpStatusCode Save(Loan loanData);
    }
}