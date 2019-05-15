using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsTutorial.WithDependencies
{
    public interface ICurrencyAccess
    {
        decimal GetExchangeRate(string currencyFrom, string currencyTo);
        void AddToCache(string currencyCode, string currencyToConvert, decimal exchangeRate);
    }

    public class CurrencyAccess : ICurrencyAccess
    {
        public void AddToCache(string currencyCode, string currencyToConvert, decimal exchangeRate)
        {
            throw new NotImplementedException();
        }

        public decimal GetExchangeRate(string currencyFrom, string currencyTo)
        {
            using (SqlConnection sql = new SqlConnection())
            {
                sql.Open();
                var command = sql.CreateCommand();
                command.CommandText = $"select exchangerate from rates where currencyFrom = {currencyFrom} and currencyTo = {currencyTo}";
                var reader = command.ExecuteReader();
                return Decimal.Parse(reader[0].ToString());
            }
        }
    }
}
