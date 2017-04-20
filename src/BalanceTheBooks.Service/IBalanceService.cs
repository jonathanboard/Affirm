using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
