using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace FxConvertTask.Tests
{
    public class RatesRepositoryTests
    {
        [Fact]
        public void UnitTests()
        {
            Mock<IOptions> optionsMock = new Mock<IOptions>();
            optionsMock.Setup(m => m.BaseCurrency).Returns("EUR");
            optionsMock.Setup(m => m.DateFormat).Returns("yyyy-MM-dd");
            optionsMock.Setup(m => m.DecimalCulture).Returns(new System.Globalization.CultureInfo("en-US"));

            Mock<IFixerClient> fixerMock = new Mock<IFixerClient>();

            Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
            rates.Add("DKK", 7.4409m);
            rates.Add("RON", 4.55m);

            fixerMock.Setup(m => m.Get(It.IsAny<DateTime>())).Returns(new FixerIo { BaseCurrency = "EUR", Date = DateTime.Today, Rates = rates });

            RatesRepository ratesRepo = new RatesRepository(fixerMock.Object, optionsMock.Object);


            Assert.Throws<CurrencyNotFoundException>(() => ratesRepo.GetRate(DateTime.Today, "a", "b"));

            //Check DKK -> RON through EUR
            decimal rate = 0m;
            rate = ratesRepo.GetRate(DateTime.Today, "DKK", "RON");
            Assert.Equal(4.55m /7.4409m, rate);

            //Uses cache
            fixerMock.Verify(m => m.Get(It.IsAny<DateTime>()), Times.Once);
        }
    }
}
