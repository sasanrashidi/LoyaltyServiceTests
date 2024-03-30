
using Moq;
using LoyaltyServiceProject;

namespace LoyaltyServiceTests

{
    [TestClass]
    public class MockTest
    {
        [TestMethod]
        public void Test()
        {
            var mock = new Mock<IOrderRepository>();

            mock.Setup(repo => repo.GetTotalAmountSpent("user123")).Returns(10);
            mock.Setup(repo => repo.GetTotalAmountSpent("Hobbit")).Returns(100);

            LoyaltyService Loyal = new(mock.Object);

            Assert.AreEqual(1, Loyal.CalculateLoyaltyPoints("user123"));
            Assert.AreEqual(10, Loyal.CalculateLoyaltyPoints("Hobbit"));

        }

        [TestMethod]
        public void ProvideTravelAdvice_Sends_Notification_With_Country_Info()
        {
            var userId = "user123";
            var countryCode = "Iran";
            var countryInfo = new CountryInfo { CountryCode = countryCode, TravelRestrictions = "Something", VaccinationReg = "Alot" };

            var mockCountryService = new Mock<ICountryInfoService>();
            mockCountryService.Setup(repo => repo.GetCountryInfo(countryCode)).Returns(countryInfo);

            var mockNotificationService = new Mock<INotificationService>();

            var travelAdvisorService = new TravelAdvisorService(mockCountryService.Object, mockNotificationService.Object);

            travelAdvisorService.ProvideTravelAdvice(userId, countryCode);

            mockNotificationService.Verify(
                ns => ns.SendNotification(userId, $"Travel Restrictions: {countryInfo.TravelRestrictions}, Vaccination Requirement: {countryInfo.VaccinationReg}"),
                Times.Once);
        }
    }
}