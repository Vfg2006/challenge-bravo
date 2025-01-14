using CurrencyQuotation.Models;
using CurrencyQuotation.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyQuotation.Services.Interfaces
{
    public interface ICurrencyQuotationService
    {
        Task<decimal> GetQuotation(string from, string to, decimal amount);
        Task<bool> InsertNewCurrency(CurrencyDto currencyDto);
        Task DeleteCurrencyByName(string name);
        void SaveAll(IEnumerable<Currency> currencies);
        Task<IList<Currency>> GetAllCurrencies();
        void UpdateAll(IList<Currency> currenciesInDb);
        Task UpdateCurrencyByName(string name, decimal dolarAmount);
        Task<Currency> GetCurrencyByName(string name);
    }
}
