using LukeApps.CurrencyRates.Enums;
using LukeApps.CurrencyRates.Models;
using System.Collections.Generic;

namespace LukeApps.CurrencyRates.DAL
{
    public class CurrencyInitializer : System.Data.Entity.CreateDatabaseIfNotExists<CurrencyEntities>
    {
        protected override void Seed(CurrencyEntities db)
        {
            var CurrencyList = new List<Currency>
            {
                new Currency
                {
                    CurrencyCode = CurrencyCode.OMR,
                    CurrencyRateDefault = 1
                },
                new Currency
                {
                    CurrencyCode = CurrencyCode.EUR,
                    CurrencyRateDefault = 0.452993
                },
                new Currency
                {
                    CurrencyCode = CurrencyCode.AED,
                    CurrencyRateDefault = 0.104697
                },
                new Currency
                {
                    CurrencyCode = CurrencyCode.GBP,
                    CurrencyRateDefault = 0.499532
                },
                new Currency
                {
                    CurrencyCode = CurrencyCode.INR,
                    CurrencyRateDefault = 0.00521111
                },
                new Currency
                {
                    CurrencyCode = CurrencyCode.USD,
                    CurrencyRateDefault = 0.384500
                }
            };

            CurrencyList.ForEach(c => db.Currencies.Add(c));
            db.SaveChanges();
        }
    }
}