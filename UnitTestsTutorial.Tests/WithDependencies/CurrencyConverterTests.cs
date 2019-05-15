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

            var sut = new Fixture()
                .Configure();

            // act
            sut.ConvertPrice(price, currencyToConvert);

            // assert
        }

        private class Fixture
        {
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

                    return mock.Create<CurrencyConverter>();
                }
            }
        }
    }
}
