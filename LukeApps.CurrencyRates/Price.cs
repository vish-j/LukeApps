using LukeApps.CurrencyRates.Enums;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LukeApps.CurrencyRates
{
    public class Price
    {
        public Price()
        {
            PriceDate = DateTime.Now;
        }

        public Price(CurrencyCode currencyCode, double value)
        {
            CurrencyCode = currencyCode;
            Value = value;
            PriceDate = DateTime.Now;
        }

        [SkipTracking]
        [NotMapped]
        [JsonIgnore]
        public DateTime PriceDate { get; set; }

        [Display(Name = "Currency Code")]
        public CurrencyCode CurrencyCode { get; set; } = CurrencyCode.OMR;

        public double Value { get; set; } = 0;

        [NotMapped]
        [JsonIgnore]
        [SkipTracking]
        public double ValueEuro => Math.Round(CurrencyProvider.GetCurrencyProvider().GetDefaultCurrencyRate(CurrencyCode, PriceDate) * Value, 2);

        [NotMapped]
        [JsonIgnore]
        [SkipTracking]
        public double DefaultCurrencyRate => CurrencyProvider.GetCurrencyProvider().GetDefaultCurrencyRate(CurrencyCode, PriceDate);

        public override string ToString() =>
           CurrencyCode == CurrencyCode.OMR ? $"{CurrencyCode} {Value:#,##0.000}" : $"{CurrencyCode} {Value:n}";
    }
}