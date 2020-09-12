using LukeApps.AccountingDocumentor.Interfaces;
using LukeApps.CurrencyRates;
using System.Collections.Generic;
using System.Linq;

namespace LukeApps.AccountingDocumentor
{
    public abstract class AccountingDocument : IAccountingDocument
    {
        public Price Total => GetTotalPrice(GetScopeItems());

        public abstract IEnumerable<IScopeItem> GetScopeItems();

        public Price GetTotalPrice<T>(T ScopeItems) where T : IEnumerable<IScopeItem>
        {
            return ScopeItems == null || !ScopeItems.Any() ?
                new Price()
              : new Price(ScopeItems.First().TotalPrice.CurrencyCode, ScopeItems.Sum(i => i.TotalPrice.Value));
        }
    }
}