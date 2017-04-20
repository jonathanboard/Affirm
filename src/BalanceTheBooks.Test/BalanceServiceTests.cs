using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BalanceTheBooks.Service;
using BalanceTheBooks.Service.Repository;
using BalanceTheBooks.Service.Model;
using System.Net;


namespace BalanceTheBooks.Test
{
    [TestClass()]
    public class BalanceServiceTests
    {
        [TestMethod]
        public void Example_DI_For_Service_Should_Not_Fail_With_A_Failure()
        {
            Mock<IBalanceRepository> repositoryMock = new Mock<IBalanceRepository>();
            repositoryMock.Setup(r => r.LoadBanks(It.IsAny<string>())).Returns(new Dictionary<long, Bank>());
            repositoryMock.Setup(r => r.LoadCovenents(It.IsAny<string>())).Returns(new List<Covenant>());
            repositoryMock.Setup(r => r.LoadFacility(It.IsAny<string>())).Returns(new Dictionary<long, Facility>());
            repositoryMock.Setup(r => r.Save(It.IsAny<List<Facility>>(), It.IsAny<string>()));
                

            IBalanceService balanceService = new BalanceService(repositoryMock.Object);
            Loan testLoan = new Loan() { LoanId = 0, Amount = 10, DefaultLikelihoodOfDefault = 08.0f, OriginationState = "CA" };
            var statusCode = balanceService.BalanceLoan(testLoan);

            Assert.AreEqual(statusCode, HttpStatusCode.InternalServerError, "We expected this to blow up");

        }
    }
}
