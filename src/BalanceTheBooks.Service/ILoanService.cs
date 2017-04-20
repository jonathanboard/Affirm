using BalanceTheBooks.Service.Model;

namespace BalanceTheBooks.Service
{
    public interface ILoanService
    {
        Loan GetNextLoan();
    }
}