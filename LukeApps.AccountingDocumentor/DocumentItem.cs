using LukeApps.AccountingDocumentor.Enums;
using LukeApps.AccountingDocumentor.Interfaces;
using LukeApps.CurrencyRates;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.AccountingDocumentor
{
    public abstract class DocumentItem : IScopeItem
    {
        private bool islumpsum;

        private string unit;

        private double quantity = 0;

        [DataType(DataType.MultilineText)]
        [Display(Name = "Item Description")]
        public string Description { get; set; }

        [Display(Name = "Lump Sum?")]
        public bool IsLumpSum
        {
            get => islumpsum;
            set
            {
                if (value)
                {
                    Unit = "LumpSum";
                    Quantity = 1;
                }
                islumpsum = value;
            }
        }

        public string Unit
        {
            get => unit;
            set => unit = IsLumpSum ? "LumpSum" : value;
        }

        public double Quantity
        {
            get => quantity;
            set => quantity = IsLumpSum ? 1 : value;
        }

        [Display(Name = "Unit Price")]
        public Price UnitPrice { get; set; } = new Price();

        [Display(Name = "Total Price")]
        public Price TotalPrice => new Price(UnitPrice.CurrencyCode, UnitPrice.Value * Quantity);

        [Display(Name = "Scope Item Type")]
        public ScopeItemType ScopeItemType { get; set; } = ScopeItemType.Main;

        public int Order { get; set; } = 0;

        public abstract object GetParentID();

        public override string ToString() => IsLumpSum
                ? $"{Description},\nTotal Price: {TotalPrice}"
                : $"{Description},\n{Quantity:n} {Unit},\nUnit Price: {UnitPrice},\nTotal Price: {TotalPrice}";
    }
}