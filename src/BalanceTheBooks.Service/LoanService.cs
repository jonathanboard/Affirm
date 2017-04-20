using System;
using BalanceTheBooks.Service.Repository;
using System.Configuration;
using BalanceTheBooks.Service.Model;
using System.Net;

namespace BalanceTheBooks.Service
{
    public class LoanService : ILoanService
    {
        private ILoanRepository _repository;

        public LoanService()
            :this(new LoanRepository(ConfigurationManager.AppSettings["loanFile"], 
                ConfigurationManager.AppSettings["assignmentFile"]) )
        {

        }

        public LoanService(ILoanRepository repository)
        {
            _repository = repository;
        }

        public Loan GetNextLoan()
        {
            return _repository.GetNextLoan();
        }

        public HttpStatusCode Save(Loan loanData)
        {
            try
            {
                _repository.SaveLoan(loanData);
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }
    }
}
