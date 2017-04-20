using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalanceTheBooks.Service.Model;

namespace BalanceTheBooks.Service.Repository
{
    public interface IBalanceRepository
    {
        Dictionary<long, Bank> LoadBanks(string fileName);
        Dictionary<long, Facility> LoadFacility(string fileName);
        List<Covenant> LoadCovenents(string fileName);
        bool Save(List<Facility> facilities, string fileName);
    }
}
