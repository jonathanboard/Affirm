using System;
using System.Collections.Generic;
using System.Linq;
using BalanceTheBooks.Service.Model;
using BalanceTheBooks.Service.Repository;
using System.Configuration;
using System.Net;

namespace BalanceTheBooks.Service
{
    public class BalanceService : IBalanceService
    {
        Dictionary<long, Bank> _banks;
        Dictionary<long, Facility> _facilities;
        List<Covenant> _covenants;
        IBalanceRepository _repository;

        public BalanceService()
            :this(new BalanceRepository())
        {
            _banks = _repository.LoadBanks(ConfigurationManager.AppSettings["bankFile"]);
            _facilities = _repository.LoadFacility(ConfigurationManager.AppSettings["facilityFile"]);
            _covenants = _repository.LoadCovenents(ConfigurationManager.AppSettings["covenentFile"]);
        }

        public BalanceService(IBalanceRepository repository)
        {
            _repository = repository;           
        }

        public HttpStatusCode BalanceLoan(Loan loanData)
        {
            try
            {
                var matchCovenents = getMatchingLoanCovenents(loanData);
                var matchCreditFacilities = getMatchingCreditFacilities(matchCovenents);

                var filteredCrediFacilities = getFilteredCreditFacilities(matchCreditFacilities, loanData);
                var bestmatch = matchFacility(filteredCrediFacilities, loanData);          
            }
            catch(Exception)
            {
                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }

        public HttpStatusCode SaveYields()
        {
            try
            {
                _repository.Save(_facilities.Values.ToList(), ConfigurationManager.AppSettings["yeildFile"]);
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }

        private bool matchFacility(List<Facility> creditFacilities, Loan loanData)
        {
            var bestMatchYield = 0f;
            Facility bestMatch = null;

            for (var i = 0; i < creditFacilities.Count(); i++)
            {
                var loanYield = getLoanYield(creditFacilities[i], loanData);
                
                if(bestMatchYield < loanYield)
                {
                    bestMatchYield = loanYield;
                    bestMatch = creditFacilities[i];
                }
            }

            if (bestMatchYield > 0f)
            {
                bestMatch.CommittedAmount += loanData.Amount;
                bestMatch.Yield += bestMatchYield;

                loanData.CreditFacility = bestMatch;
            }
                      
            return bestMatchYield != 0f;
        }

        private float getLoanYield(Facility facility, Loan loanData)
        {
            return (1.0f - loanData.DefaultLikelihoodOfDefault) * loanData.InterestRate * loanData.Amount
                - loanData.DefaultLikelihoodOfDefault * loanData.Amount
                - facility.InterestRate * loanData.Amount;
        }

        private List<Facility> getFilteredCreditFacilities(List<Facility> matchCreditFacilities, Loan loanData)
        {
            return matchCreditFacilities.Where(cf => (cf.Amount - cf.CommittedAmount) >= loanData.Amount).ToList();
        }

        private List<Covenant> getMatchingLoanCovenents(Loan targetLoan)
        {
            return _covenants.Where(c => c.BannedState.ToLower() != targetLoan.OriginationState.Trim().ToLower()
                && c.MaxDefaultLikelihood >= targetLoan.DefaultLikelihoodOfDefault).ToList();
        }

        private List<Facility> getMatchingCreditFacilities(List<Covenant> covenents)
        {
            return covenents.Select(x => _facilities[x.FacilityId]).ToList();
        }

    }
}
