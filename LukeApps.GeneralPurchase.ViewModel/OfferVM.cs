using LukeApps.CurrencyRates;
using LukeApps.FileHandling;
using LukeApps.GeneralPurchase.Classes;
using LukeApps.GeneralPurchase.Enums;
using LukeApps.GeneralPurchase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class OfferVM
    {
        public OfferVM()
        {
        }

        public OfferVM(long enquiryID, SelectList companies)
        {
            EnquiryID = enquiryID;
            Revision = 0;
            ScopeItems = new List<ScopeItem>();
            VendorResponse = VendorResponse.NoResponse;

            Companies = companies;
        }

        public OfferVM(Offer offer)
        {
            OfferID = offer.OfferID;
            EnquiryID = offer.EnquiryID;
            CompanyID = offer.CompanyID;
            CompanyName = offer.Company.CompanyName;
            EnquiryNumber = offer.Enquiry.EnquiryNumber;

            Revision = offer.Revision;
            ReferenceNumber = offer.ReferenceNumber;
            VendorResponse = offer.VendorResponse;
            BidReceivedDate = offer.BidReceivedDate;
            ExpiryDate = offer.ExpiryDate;
            AgreedPaymentTerms = offer.AgreedPaymentTerms;
            DeliveryDate = offer.DeliveryDate;
            DeliveryTerms = offer.DeliveryTerms;
            Quotation = offer.Quotation;
            IsNew = offer.IsNew;
            ScopeItems = offer.ScopeItems.ToList();
            TotalOfferValue = offer.Total;
        }

        [Key]
        public long OfferID { get; set; }

        public long EnquiryID { get; set; }

        public long CompanyID { get; set; }

        [Display(Name = "Vendor Response")]
        public VendorResponse VendorResponse { get; set; }

        [Display(Name = "Quote Ref. No.")]
        public string ReferenceNumber { get; set; }

        public int Revision { get; set; }

        [Display(Name = "Date of Received Bid")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BidReceivedDate { get; set; }

        [Display(Name = "Total Price quoted")]
        public Price TotalPriceQuoted { get; set; }

        [Display(Name = "Expiry Date Of Offer")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; } = DateTime.Now.AddMonths(3);

        [Display(Name = "Terms and Conditions agreed?")]
        public bool? IsTermsConditionsAgreed { get; set; }

        [Display(Name = "Deviations Proposed in the Terms And Conditions")]
        [DataType(DataType.MultilineText)]
        public string DeviationsInTermsAndConditions { get; set; }

        [Display(Name = "Delivery Terms")]
        [DataType(DataType.MultilineText)]
        public string DeliveryTerms { get; set; }

        [Display(Name = "Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Is Payment terms agreed?")]
        public bool IsPaymentTermsAgreed { get; set; }

        [Display(Name = "Payment Terms")]
        public PaymentTerms InitialPaymentTerms { get; set; }

        [Display(Name = "Payment Terms")]
        public PaymentTerms AgreedPaymentTerms { get; set; }

        [Display(Name = "Technically Acceptable?")]
        public bool? IsTechnicallyAcceptable { get; set; }

        [Display(Name = "Commercially Acceptable?")]
        public bool? IsCommerciallyAcceptable { get; set; }

        public bool IsNew { get; set; }

        [Display(Name = "Offer Number")]
        public string OfferNumber => OfferID.ToString("000000");

        public MultipleFilesHandle Quotation { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Brief Description of Good/Service")]
        public string GoodsBriefDescription { get; set; }

        [Display(Name = "Total Offer Value")]
        public Price TotalOfferValue { get; set; }

        public List<ScopeItem> ScopeItems { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public string EnquiryNumber { get; set; }

        public SelectList Companies { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Technical Comments")]
        public string TechnicalComments { get; private set; }
    }
}