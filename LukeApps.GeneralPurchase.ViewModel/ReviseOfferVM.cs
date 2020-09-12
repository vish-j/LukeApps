using LukeApps.GeneralPurchase.Classes;
using LukeApps.GeneralPurchase.Enums;
using LukeApps.GeneralPurchase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class ReviseOfferVM 
    {
        public ReviseOfferVM()
        {
            ScopeItems = new HashSet<ScopeItem>();
        }

        public long PreviousOfferID { get; set; }

        public long EnquiryID { get; set; }

        public long CompanyID { get; set; }
        public string CompanyName { get; set; }

        [Display(Name = "Vendor Response")]
        public VendorResponse VendorResponse { get; set; }

        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Date Bid Received")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BidReceivedDate { get; set; } = DateTime.Now;

        [Display(Name = "Expiry Date Of Offer")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; } = DateTime.Now.AddMonths(3);

        [Display(Name = "Initial Payment Terms")]
        [DataType(DataType.MultilineText)]
        public PaymentTerms InitialPaymentTerms { get; set; }

        [Display(Name = "Terms and Conditions agreed?")]
        public bool? IsTermsConditionsAgreed { get; set; }

        [Display(Name = "Deviations Proposed in the Terms And Conditions")]
        [DataType(DataType.MultilineText)]
        public string DeviationsInTermsAndConditions { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Goods Brief Description")]
        public string GoodsBriefDescription { get; set; }

        [Display(Name = "Total Offer Value")]
        public double TotalOfferValue { get; set; }

        [Display(Name = "Is Payment terms agreed?")]
        public bool IsPaymentTermsAgreed { get; set; }

        [Display(Name = "Agreed Payment Terms")]
        [DataType(DataType.MultilineText)]
        public PaymentTerms AgreedPaymentTerms { get; set; }

        public ICollection<ScopeItem> ScopeItems { get; set; }

        [Display(Name = "Delivery Terms")]
        [DataType(DataType.MultilineText)]
        public string DeliveryTerms { get; set; }

        [Display(Name = "Delivery Date")]
        public DateTime? DeliveryDate { get; set; }
    }
}