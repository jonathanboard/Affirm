using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalanceTheBooks.Service;
using BalanceTheBooks.Service.Model;
using System.IO;
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

            balanceService.SaveYeilds();
          
        }

        

//interest_rate,amount,id,default_likelihood,state
//0.15,10552,1,0.02,MO
//0.15,51157,2,0.01,VT
//0.35,74965,3,0.06,AL

    }
}
