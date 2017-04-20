using System;
using System.Collections.Generic;
using BalanceTheBooks.Service.Model;
using System.IO;
using CsvHelper;

namespace BalanceTheBooks.Service.Repository
{
    public class BalanceRepository : IBalanceRepository
    {
        private const string YEILD_HEADER = "facility_id,expected_yield";
        private const string YEILD_TEMPLATE = "{0},{1}";

        public Dictionary<long, Bank> LoadBanks(string fileName)
        {
            var returnVal = new Dictionary<long, Bank>();

            using (var reader = new StreamReader(fileName))
            {
                var csvReader = new CsvReader(reader);
                while(csvReader.Read())
                {
                    ///todo: this is very brittle
                    var record = csvReader.CurrentRecord;
                    long id = 0;
                    if(long.TryParse(record[0], out id))
                    {
                        returnVal.Add(id, new Bank() { Id = id, Name = record[1].Trim() });
                    }                    
                }
             }

            return returnVal;
        }

        public List<Covenant> LoadCovenents(string fileName)
        {
            var returnVal = new List<Covenant>();

            using (var reader = new StreamReader(fileName))
            {
                var csvReader = new CsvHelper.CsvReader(reader);
                while (csvReader.Read())
                {
                    ///todo: this is very brittle
                    var record = csvReader.CurrentRecord;
      
                    long bankId = 0;
                    float maxDefualt = 1.0f;
                    long facilityId = 0;

                    if (long.TryParse(record[0], out facilityId))
                    {
                        float.TryParse(record[1], out maxDefualt);
                        long.TryParse(record[2], out bankId);

                        returnVal.Add(new Covenant() { BankId = bankId, FacilityId = facilityId, MaxDefaultLikelihood = maxDefualt, BanndedState = record[3].Trim() });
                    }
                }
            }

            return returnVal;
        }

        public Dictionary<long, Facility> LoadFacility(string fileName)
        {
            var returnVal = new Dictionary<long, Facility>();

            using (var reader = new StreamReader(fileName))
            {
                var csvReader = new CsvHelper.CsvReader(reader);
                while (csvReader.Read())
                {
                    ///todo: this is very brittle
                    ///amount,interest_rate,id,bank_id
                    var record = csvReader.CurrentRecord;
                    int amount = int.Parse(record[0], System.Globalization.NumberStyles.AllowDecimalPoint);
                    float interest = float.Parse(record[1]);
                    long id = int.Parse(record[2]);
                    long bankId = long.Parse(record[2]);

                    returnVal.Add(id, new Facility() { Id = id, Amount = amount, InterestRate = interest, BankId = bankId });
                }
            }

            return returnVal;
        }

        public bool Save(List<Facility> facilities, string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine(YEILD_HEADER);

                foreach(var facility in facilities)
                {
                    writer.WriteLine(string.Format(YEILD_TEMPLATE, facility.Id, Math.Round(facility.Yield, MidpointRounding.AwayFromZero)));
                }
                writer.Flush();
                writer.Close();
            }

            return true;
        }
    }
}
