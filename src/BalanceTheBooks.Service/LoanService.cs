
using BalanceTheBooks.Service.Repository;
using System.Configuration;
using BalanceTheBooks.Service.Model;

namespace BalanceTheBooks.Service
{
    public class LoanService : ILoanService
    {
        private ILoanRepository _repository;

        public LoanService()
            :this(new LoanRepository(ConfigurationManager.AppSettings["loanFile"], ConfigurationManager.AppSettings["assignmentFile"]) )
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

        public void Save(Loan loanData)
        {
            _repository.SaveLoan(loanData);
        }
    }
}
