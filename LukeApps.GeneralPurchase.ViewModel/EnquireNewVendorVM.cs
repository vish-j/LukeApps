using LukeApps.GeneralPurchase.Enums;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class EnquireNewVendorVM
    {
        public EnquireNewVendorVM()
        {
        }

        [Display(Name = "Company")]
        public long CompanyID { get; set; }

        public long EnquiryID { get; set; }
        public VendorResponse VendorResponse { get; set; } = VendorResponse.NoResponse;
        public SelectList Companies { get; set; }
    }
}