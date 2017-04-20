using BalanceTheBooks.Service;
using System.Net;

namespace BalanceTheBooks.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            BalanceService balanceService = new BalanceService();
            LoanService loanService = new LoanService();

            var loan = loanService.GetNextLoan();

            while(loan != null)
            {
                if(HttpStatusCode.OK == balanceService.BalanceLoan(loan))
                {
                    loanService.Save(loan);
                }

                loan = loanService.GetNextLoan();
            }

            balanceService.SaveYields();
          
        }
    }
}
