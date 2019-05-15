using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsTutorial.WithDependencies
{
    public class SimplePrice
    {
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
    }

    public class CurrencyConverter
    {
        private readonly ICurrencyAccess _currencyAccess;

        public CurrencyConverter(ICurrencyAccess currencyAccess)
        {
            _currencyAccess = currencyAccess;
        }

        public SimplePrice ConvertPrice(SimplePrice priceToConvert, string currencyToConvert)
        {
            var exchangeRate = _currencyAccess.GetExchangeRate(priceToConvert.CurrencyCode, currencyToConvert);
            var value = exchangeRate * priceToConvert.Price;

            return new SimplePrice
            {
                Price = value,
                CurrencyCode = currencyToConvert
            };
        }
    }
}
