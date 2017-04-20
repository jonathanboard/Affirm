﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalanceTheBooks.Service.Model;
using BalanceTheBooks.Service.Repository;
using System.Configuration;
using System.Net;

namespace BalanceTheBooks.Service
{
    public class BalanceService
    {
        private Dictionary<long, Bank> _banks;
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

                //_repository.Save(loanData);
            }
            catch(Exception)
            {
                return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.OK;
        }

        public HttpStatusCode SaveYeilds()
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
            var bestMatchYeild = 0f;
            Facility bestMatch = null;

            for (var i = 0; i < creditFacilities.Count(); i++)
            {
                var loanYeild = getLoanYeild(creditFacilities[i], loanData);
                if(bestMatchYeild < loanYeild)
                {
                    bestMatchYeild = loanYeild;
                    bestMatch = creditFacilities[i];
                }
            }

            bestMatch.CommitedAmount += loanData.Amount;
            bestMatch.Yeild += bestMatchYeild;
            
            loanData.CreditFacility = bestMatch;

            return bestMatchYeild != 0f;
        }

        private float getLoanYeild(Facility facility, Loan loanData)
        {
            return (1.0f - loanData.DefaultLikelihoodOfDefault) * loanData.InterestRate * loanData.Amount
                - loanData.DefaultLikelihoodOfDefault * loanData.Amount
                - facility.InterestRate * loanData.Amount;
        }

        private List<Facility> getFilteredCreditFacilities(List<Facility> matchCreditFacilities, Loan loanData)
        {
            return matchCreditFacilities.Where(cf => (cf.Amount - cf.CommitedAmount) >= loanData.Amount).ToList();
        }

        private List<Covenant> getMatchingLoanCovenents(Loan targetLoan)
        {
            return _covenants.Where(c => c.BanndedState.ToLower() != targetLoan.OriginationState.Trim().ToLower()
                && c.MaxDefaultLikelihood >= targetLoan.DefaultLikelihoodOfDefault).ToList();
        }

        private List<Facility> getMatchingCreditFacilities(List<Covenant> covenents)
        {
            return covenents.Select(x => _facilities[x.FacilityId]).ToList();
        }

    }
}