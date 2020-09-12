using LukeApps.AccountingDocumentor.Enums;
using LukeApps.CurrencyRates;

namespace LukeApps.AccountingDocumentor.Interfaces
{
    public interface IScopeItem
    {
        string Description { get; set; }

        bool IsLumpSum { get; set; }

        string Unit { get; set; }

        double Quantity { get; set; }

        Price UnitPrice { get; set; }

        Price TotalPrice { get; }

        ScopeItemType ScopeItemType { get; set; }
        int Order { get; set; }

        object GetParentID();
    }
}