using LukeApps.CurrencyRates.Enums;
using LukeApps.CurrencyRates.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LukeApps.CurrencyRates
{
    public class CurrencyHandler
    {
        public CurrencyHandler(List<Currency> CurrencyList)
        {
            SetCurrency(CurrencyList);
        }

        protected void SetCurrency(List<Currency> CurrencyList)
        {
            _currencyList = CurrencyList.Select(c => new CurrencyRate
            {
                CurrencyCode = c.CurrencyCode,
                Rate = c.CurrencyRateDefault,
                DateOfRecord = c.AuditDetail.CreatedDate
            }).ToList();
        }

        protected CurrencyHandler()
        {
        }

        private List<CurrencyRate> _currencyList;

        private List<CurrencyRate> _currencyListTransactionTime;

        public void SetTransactionDate(DateTime TransactionDate)
        {
            _currencyListTransactionTime = _currencyList.GroupBy(c => c.CurrencyCode).Select(c => new CurrencyRate
            {
                CurrencyCode = c.Key,
                Rate = (c.Where(r => r.DateOfRecord.Date <= TransactionDate.Date).OrderByDescending(r => r.DateOfRecord).FirstOrDefault() ?? c.Where(r => r.DateOfRecord > TransactionDate).OrderByDescending(r => r.DateOfRecord).FirstOrDefault()).Rate
            }).ToList();
        }

        /// <summary>
        /// Get Rate Needed Rate
        /// </summary>
        /// <param name="From">Currency Code of the Current Price</param>
        /// <param name="To">Currency Code of the Price to be Converted</param>
        /// <exception cref="InvalidOperationException">Transaction Time should not be null</exception>
        /// <returns>Needed Rate</returns>
        public double GetRate(CurrencyCode From, CurrencyCode To)
        {
            if (_currencyListTransactionTime == null)
                throw new InvalidOperationException("Transaction Time is not set");
            return getRate(From, To);
        }

        /// <exception cref="InvalidOperationException">Transaction Time should not be null</exception>
        private double getRate(CurrencyCode From, CurrencyCode To)
        {
            var fromRate = _currencyListTransactionTime.Where(c => c.CurrencyCode == From).FirstOrDefault().Rate;
            var toRate = _currencyListTransactionTime.Where(c => c.CurrencyCode == To).FirstOrDefault().Rate;

            if (From == CurrencyCode.EUR)
                return toRate;
            else
                return toRate / fromRate;
        }

        /// <exception cref="InvalidOperationException">Transaction Time should not be null</exception>
        public double GetRate(CurrencyCode From, CurrencyCode To, DateTime TransactionDate)
        {
            SetTransactionDate(TransactionDate);
            return getRate(From, To);
        }

        /// <exception cref="InvalidOperationException">Transaction Time should not be null</exception>
        public double GetDefaultCurrencyRate(CurrencyCode From, DateTime TransactionDate)
        {
            SetTransactionDate(TransactionDate);
            return getRate(From, CurrencyCode.OMR);
        }
    }
}