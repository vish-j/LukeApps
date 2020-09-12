using LukeApps.AccountingDocumentor.Enums;
using LukeApps.Common.Helpers;
using LukeApps.FileHandling;
using LukeApps.GeneralPurchase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PhilApprovalFlow;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class OfferDisplayVM
    {
        public OfferDisplayVM(Offer offer)
        {
            OfferID = offer.OfferID;
            OfferNumber = offer.OfferNumber;
            Revision = offer.Revision.ToString("00");
            ReferenceNumber = offer.ReferenceNumber;
            RegistrationNumber = offer.Company.CompanyRegistration ?? offer.Company.CompanyNumber;
            CompanyName = offer.Company.CompanyName;
            EnquiryNumber = offer.Enquiry.EnquiryNumber;
            AgreementNumber = (offer.PurchaseOrder == null || offer.PurchaseOrder.IsPurchaseOrderCancelled) ? "Not Accepted" : offer.PurchaseOrder.PurchaseOrderNumber;
            VendorResponse = offer.VendorResponse.GetDisplay();
            Quotation = offer.Quotation;

            if (offer.VendorResponse != Enums.VendorResponse.Responded)
            {
                BidReceivedDate = "-";
                TotalPriceQuoted = "-";
                ExpiryDate = "-";
                InitialPaymentTerms = "-";
                IsTermsConditionsAgreed = "-";
                DeviationsInTermsAndConditions = "-";
                IsPaymentTermsAgreed = "-";
                AgreedPaymentTerms = "-";
                Milestones = "-";
                ScheduleExecution = "-";
                DeliveryDate = "-";
                DeliveryTerms = "-";
                IsTechnicallyAcceptable = "-";
                TechnicalComments = "-";
                IsCommerciallyAcceptable = "-";
                GoodsBriefDescription = "-";
                ScopeItems = new List<ScopeItem>();
            }
            else
            {
                BidReceivedDate = offer.BidReceivedDate == null ? "Pending" : (((DateTime)offer.BidReceivedDate).ToString("dd/MM/yyyy"));
                TotalPriceQuoted = offer.Total.ToString();
                ExpiryDate = offer.ExpiryDate == null ? "Pending" : ((DateTime)offer.ExpiryDate).ToString("dd/MM/yyyy");

                AgreedPaymentTerms = offer.AgreedPaymentTerms.ToString();

                DeliveryDate = (offer.DeliveryDate == null) ? "Pending" : ((DateTime)offer.DeliveryDate).ToString("dd/MM/yyyy");
                DeliveryTerms = (offer.DeliveryTerms == null) ? "Pending" : offer.DeliveryTerms;

                ScopeItems = offer.ScopeItems.Where(s => s.ScopeItemType == ScopeItemType.Main).OrderBy(s => s.Order).ToList();
            }

            CreatedDate = offer.AuditDetail.CreatedDate.ToString("dd/MM/yyyy");
            CreatedEntryUser = offer.AuditDetail.CreatedEntryUserDisplayName;
            LastModifiedDate = offer.AuditDetail.CreatedDate.ToString("dd/MM/yyyy");
            LastModifiedEntryUser = offer.AuditDetail.LastModifiedEntryUserDisplayName;
            IsOfferAccepted = offer.PurchaseOrder != null && !offer.PurchaseOrder.IsPurchaseOrderCancelled;
            IsBidSummaryApproved = offer.Enquiry.Transitions.IsAnyApproved();
            IsOfferExpired = offer.IsOfferExpired;
        }

        public long OfferID { get; set; }

        [Display(Name = "Delivery Terms")]
        public string OfferNumber { get; private set; }

        public string Revision { get; private set; }

        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; private set; }

        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; private set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; private set; }

        [Display(Name = "Enquiry Number")]
        public string EnquiryNumber { get; private set; }

        [Display(Name = "Project Number")]
        public string ProjectNumber { get; private set; }

        [Display(Name = "Agreement Number")]
        public string AgreementNumber { get; private set; }

        [Display(Name = "Vendor Response")]
        public string VendorResponse { get; private set; }

        public MultipleFilesHandle Quotation { get; }

        [Display(Name = "Date of Received Bid")]
        public string BidReceivedDate { get; private set; }

        [Display(Name = "Total Price quoted")]
        public string TotalPriceQuoted { get; private set; }

        [Display(Name = "Expiry Date")]
        public string ExpiryDate { get; private set; }

        [Display(Name = "Initial Payment Terms")]
        public string InitialPaymentTerms { get; private set; }

        [Display(Name = "Terms and Conditions agreed?")]
        public string IsTermsConditionsAgreed { get; private set; }

        [Display(Name = "Deviations Proposed in the Terms And Conditions")]
        public string DeviationsInTermsAndConditions { get; private set; }

        [Display(Name = "Delivery Terms")]
        public string DeliveryTerms { get; private set; }

        [Display(Name = "Delivery Date")]
        public string DeliveryDate { get; private set; }

        [Display(Name = "Is Payment terms agreed?")]
        public string IsPaymentTermsAgreed { get; private set; }

        [Display(Name = "Agreed Payment Terms")]
        public string AgreedPaymentTerms { get; private set; }

        public string Milestones { get; private set; }

        [Display(Name = "Schedule for the Execution")]
        public string ScheduleExecution { get; private set; }

        [Display(Name = "Technical Acceptability of the Offer")]
        public string IsTechnicallyAcceptable { get; private set; }

        [Display(Name = "Commercial Acceptabilibty of the Offer")]
        public string IsCommerciallyAcceptable { get; private set; }

        public string QuotationKey { get; set; }

        [Display(Name = "Created Date")]
        public string CreatedDate { get; private set; }

        [Display(Name = "Created Entry User")]
        public string CreatedEntryUser { get; private set; }

        [Display(Name = "Last Modified Date")]
        public string LastModifiedDate { get; private set; }

        [Display(Name = "Last Modified Entry User")]
        public string LastModifiedEntryUser { get; private set; }

        [Display(Name = "Main Scope Items")]
        public List<ScopeItem> ScopeItems { get; private set; }

        [Display(Name = "Additional Scope Items")]
        public List<ScopeItem> AdditionalScopeItems { get; private set; }

        [Display(Name = "Optional Scope Items")]
        public List<ScopeItem> OptionalScopeItems { get; private set; }

        public bool IsOfferAccepted { get; private set; }
        public string TechnicalComments { get; private set; }
        public string GoodsBriefDescription { get; private set; }
        public bool IsBidSummaryApproved { get; private set; }
        public bool IsOfferExpired { get; private set; }
    }
}