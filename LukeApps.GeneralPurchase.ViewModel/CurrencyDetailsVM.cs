using LukeApps.CurrencyRates.Models;
using LukeApps.CurrencyRates.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class CurrencyDetailsVM
    {
        /// <exception cref="ArgumentNullException">Currency Delta should not be null</exception>
        /// <exception cref="ArgumentException">Currency Delta should be of one currency</exception>
        public CurrencyDetailsVM(IEnumerable<Currency> currencyDelta)
        {
            if (currencyDelta.Select(c => c.CurrencyCode).Distinct().Count() == 1)
            {
                var a = currencyDelta.OrderByDescending(c => c.AuditDetail.CreatedDate).FirstOrDefault();
                EntryDate = a.AuditDetail.CreatedDate;
                CurrencyCode = a.CurrencyCode;
                CurrencyRateEuro = a.CurrencyRateDefault;
                CurrencyDelta = currencyDelta.ToList();
            }
            else if (currencyDelta.Select(c => c.CurrencyCode).Distinct().Count() == 0)
            {
                throw new ArgumentNullException();
            }
            else
            {
                throw new ArgumentException("Currency Delta having multiple Currencies");
            }
        }

        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

        public CurrencyCode CurrencyCode { get; set; }

        [Display(Name = "Rate - EUR")]
        public double CurrencyRateEuro { get; set; }

        public List<Currency> CurrencyDelta { get; set; }
    }
}