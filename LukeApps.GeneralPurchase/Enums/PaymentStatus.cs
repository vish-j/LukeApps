using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Enums
{
    public enum PaymentStatus
    {
        None,

        [Display(Name = "Invoice Received")]
        Received,

        [Display(Name = "Invoice Approved")]
        Approved,

        [Display(Name = "Invoice Paid")]
        Paid
    }
}