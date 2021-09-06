using CurrencyQuotation.Daos.Interfaces;
using CurrencyQuotation.Models;
using CurrencyQuotation.Models.Dtos;
using CurrencyQuotation.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyQuotation.Services
{
    public class CurrencyQuotationService : ICurrencyQuotationService
    {
        private const string REAL_CURRENCY = "BRL";

        private readonly ILogger<CurrencyQuotationService> _logger;

        private readonly ICurrencyQuotationDao _currencyQuotationDao;

        public CurrencyQuotationService(ICurrencyQuotationDao currencyQuotationDao, ILogger<CurrencyQuotationService> logger)
        {
            this._currencyQuotationDao = currencyQuotationDao;
            this._logger = logger;
        }

        public decimal GetQuotation(string from, string to, decimal amount)
        {
            IList<string> currenciesName = new List<string>() { from, to };
            IList<Currency> currencies = this._currencyQuotationDao.GetQuotationByCurrencies(currenciesName);

            Currency fromCurrency = currencies.First(c => from.Equals(c.Name));
            Currency toCurrency = currencies.First(c => to.Equals(c.Name));

            decimal result = (toCurrency.DolarAmount / fromCurrency.DolarAmount) * amount;
            return result;
        }

        public bool InsertNewCurrency(CurrencyDto currencyDto)
        {
            try
            {
                decimal dolarAmountForRealCurrency = this._currencyQuotationDao.GetDolarAmountByName(REAL_CURRENCY);
                decimal dolarAmountNewCurrency = currencyDto.RealAmount * dolarAmountForRealCurrency;

                Currency currency = new(currencyDto.Name, dolarAmountNewCurrency);
                this._currencyQuotationDao.InsertNewCurrency(currency);

                _logger.LogInformation("Nova moeda salva com sucesso");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public void DeleteCurrencyByName(string name)
        {
            Currency currencyToRemove = this._currencyQuotationDao.GetCurrencyByName(name);

            this._currencyQuotationDao.DeleteByName(currencyToRemove);
        }

        public void SaveAll(IEnumerable<Currency> currencies)
        {
            foreach (Currency currency in currencies)
            {
                this._currencyQuotationDao.InsertNewCurrency(currency);
            }
        }

        public IList<Currency> GetAllCurrencies()
        {
            return this._currencyQuotationDao.GetAllCurrencies();
        }

        public void UpdateAll(IList<Currency> currenciesInDb)
        {
            foreach (Currency currency in currenciesInDb)
            {
                this._currencyQuotationDao.Update(currency);
            }
        }

        public void UpdateCurrencyByName(string name, decimal dolarAmount)
        {
            Currency currencyToUpdate = this._currencyQuotationDao.GetCurrencyByName(name);
            currencyToUpdate.DolarAmount = dolarAmount;

            this._currencyQuotationDao.Update(currencyToUpdate);
        }
    }
}
