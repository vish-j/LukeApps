using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Enums
{
    public enum PurchaseCategory
    {
        Goods,
        Services,

        [Display(Name = "Gifts / Lunch")]
        Gifts,

        Hardware,
        Software,
        Other
    }
}