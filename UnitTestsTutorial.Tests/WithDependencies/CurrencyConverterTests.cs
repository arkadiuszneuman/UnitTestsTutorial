using Autofac.Extras.Moq;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestsTutorial.WithDependencies;
using Xunit;

namespace UnitTestsTutorial.Tests.WithDependencies
{
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
                CurrencyCode = "GBP"
            };

            var currencyToConvert = "EUR";

            var sut = new Fixture()
                .WithExchangeRate(10)
                .Configure();

            // act
            var result = sut.ConvertPrice(price, currencyToConvert);

            // assert
            result.Should().BeEquivalentTo(new SimplePrice
            {
                Price = 50,
                CurrencyCode = "EUR"
            });
        }

        [Fact]
        public void Should_Add_Exchange_Rate_To_Cache()
        {
            // arrange
            var price = new SimplePrice
            {
                Price = 5,
                CurrencyCode = "GBP"
            };

            var currencyToConvert = "EUR";

            var fixture = new Fixture();
            var sut = fixture
                .WithExchangeRate(15)
                .Configure();

            // act
            sut.ConvertPrice(price, currencyToConvert);

            // assert
            fixture.ExchangeRatesInCache.Should().BeEquivalentTo(new Fixture.ExchangeRateInCache
            {
                CurrencyFrom = "GBP",
                CurrencyTo = "EUR",
                ExchangeRate = 15
            });
        }

        private class Fixture
        {
            public class ExchangeRateInCache
            {
                public string CurrencyFrom { get; set; }
                public string CurrencyTo { get; set; }
                public decimal ExchangeRate { get; set; }
            }

            public List<ExchangeRateInCache> ExchangeRatesInCache { get; set; } = new List<ExchangeRateInCache>();
            private int _exchangeRate;

            public Fixture WithExchangeRate(int exchangeRate)
            {
                _exchangeRate = exchangeRate;
                return this;
            }

            public CurrencyConverter Configure()
            {
                using (var mock = AutoMock.GetLoose())
                {
                    mock.Mock<ICurrencyAccess>()
                        .Setup(x => x.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(_exchangeRate);

                    mock.Mock<ICurrencyAccess>()
                        .Setup(x => x.AddToCache(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
                        .Callback<string, string, decimal>((currencyFrom, currencyTo, exchangeRate) =>
                            ExchangeRatesInCache.Add(new ExchangeRateInCache
                            {
                                CurrencyFrom = currencyFrom,
                                CurrencyTo = currencyTo,
                                ExchangeRate = exchangeRate
                            }));

                    return mock.Create<CurrencyConverter>();
                }
            }
        }
    }
}
