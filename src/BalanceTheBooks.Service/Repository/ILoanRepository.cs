using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalanceTheBooks.Service.Model;

namespace BalanceTheBooks.Service.Repository
{
    public interface ILoanRepository
    {
        Loan GetNextLoan();
        void SaveLoan(Loan loanData);
    }
}
