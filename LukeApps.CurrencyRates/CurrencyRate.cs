using LukeApps.CurrencyRates.Enums;
using System;

namespace LukeApps.CurrencyRates
{
    public class CurrencyRate
    {
        public CurrencyCode CurrencyCode { get; set; }
        public double Rate { get; set; }
        public DateTime DateOfRecord { get; set; }
    }
}