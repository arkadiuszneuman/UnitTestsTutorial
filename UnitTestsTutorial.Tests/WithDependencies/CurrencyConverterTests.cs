using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestsTutorial.WithDependencies;
using Xunit;

namespace UnitTestsTutorial.Tests.WithDependencies
{

    public class CurrencyAccessFake : ICurrencyAccess
    {
        public decimal GetExchangeRate(string currencyFrom, string currencyTo)
        {
            return 10;
        }
    }

    [Trait("CurrencyConverter", "Convert price")]
    public class CurrencyConverterTests
    {
        [Fact]
        public void Should_Valid_Convert_Price()
        {
            // arrange
            var price = new SimplePrice
            {
                Price = 5,
                CurrencyCode = "PLN"
            };

            var currencyToConvert = "EUR";

            var sut = new CurrencyConverter(new CurrencyAccessFake());

            // act
            var result = sut.ConvertPrice(price, currencyToConvert);

            // assert
            Assert.Equal(50, result.Price);
            Assert.Equal("EUR", result.CurrencyCode);
        }
    }
}
