using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Enums
{
    public enum StatusBidSummary
    {
        [Display(Name = "Not Started")]
        NotStarted,

        [Display(Name = "In Process")]
        InProcess,

        Approved
    }
}