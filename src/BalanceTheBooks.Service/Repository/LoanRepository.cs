using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalanceTheBooks.Service.Model;
using System.IO;
using CsvHelper;

namespace BalanceTheBooks.Service.Repository
{
    class LoanRepository : ILoanRepository, IDisposable
    {
        StreamReader reader = null;
        StreamWriter writer = null;
        CsvReader csvReader = null;

        private const string ASSIGNED_LOAN_HEADER = "loan_id,facility_id";
        private const string ASSIGNED_LOAN_TEMPLATE = "{0},{1}";


        public LoanRepository(string inboundLoanFileName, string assignedLoansFileName)
        {
            reader = new StreamReader(inboundLoanFileName);
            writer = new StreamWriter(assignedLoansFileName);
            csvReader = new CsvReader(reader);

            writer.WriteLine(ASSIGNED_LOAN_HEADER);                                                    
        }       

        public void SaveLoan(Loan loanData)
        {
            writer.WriteLine(string.Format(ASSIGNED_LOAN_TEMPLATE, loanData.LoanId, loanData.CreditFacility.Id));
            writer.Flush();
        }

        public Loan GetNextLoan()
        {
            if(csvReader.Read())
            {
                //interest_rate,amount,id,default_likelihood,state
                var record = csvReader.CurrentRecord;
                float interestRate=  0.0f;
                int amount = 0;
                long id =0  ;
                float defaultLikelyhood;

                if (long.TryParse(record[2], out id))
                {
                    float.TryParse(record[0], out interestRate);
                    int.TryParse(record[1], out amount);
                    float.TryParse(record[3], out defaultLikelyhood);

                    return new Loan() { LoanId = id,
                        Amount = amount,
                        InterestRate = interestRate,
                        DefaultLikelihoodOfDefault = defaultLikelyhood,
                        OriginationState = record[4].Trim() };
                }   
            }

           return null; 
        }

        public void Dispose()
        {
            csvReader.Dispose();
            reader.Close();
            writer.Close();
            reader.Dispose();
            writer.Dispose();
        }

    }
}
