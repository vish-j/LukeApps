using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Enums
{
    public enum VendorResponse
    {
        Responded,

        [Display(Name = "No Response")]
        NoResponse,

        Declined
    }
}