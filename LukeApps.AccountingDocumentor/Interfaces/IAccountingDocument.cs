using LukeApps.CurrencyRates;
using System.Collections.Generic;

namespace LukeApps.AccountingDocumentor.Interfaces
{
    public interface IAccountingDocument
    {
        Price Total { get; }
        IEnumerable<IScopeItem> GetScopeItems();
    }
}