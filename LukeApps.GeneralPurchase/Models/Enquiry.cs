using LukeApps.ApprovalProcess;
using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.EmployeeData;
using LukeApps.FileHandling;
using LukeApps.GeneralPurchase.Enums;
using LukeApps.TrackingExtended;
using PhilApprovalFlow;
using PhilApprovalFlow.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LukeApps.GeneralPurchase.Models
{
    [DocumentPAFPath("Enquiries")]
    [PAFMetadata(Key = "app", Value = "Enquiry")]
    public class Enquiry : IApprovalFlow<EnquiryTransition>, IEntity, IAuditDetail, IPAFMatrix
    {
        public Enquiry()
        {
            AuditDetail = new AuditDetail();
            SupportingDocuments = new MultipleFilesHandle();
            Offers = new HashSet<Offer>();
            Transitions = new HashSet<EnquiryTransition>();
        }

        [Key]
        public long EnquiryID { get; set; }

        [Display(Name = "Enquiry Number")]
        public string EnquiryNumber => EnquiryID.ToString("000000");

        [Display(Name = "Enquiry Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnquiryDate { get; set; } = DateTime.Now;

        [Display(Name = "Closing Enquiry Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ClosingEnquiryDate { get; set; } = DateTime.Now;

        [Display(Name = "Subject of Enquiry")]
        public string SubjectOfEnquiry { get; set; }

        [Display(Name = "Scope of Work")]
        public string ScopeOfWork { get; set; }

        public long BudgetID { get; set; }
        public virtual Budget Budget { get; set; }

        [Required]
        public string OriginatorID { get; set; }

        public Employee Originator => EmployeeProvider.GetEmployeeProvider().GetUserData(OriginatorID);

        public string TechnicalEvaluatorID { get; set; }

        [Display(Name = "Technical Evaluator")]
        public Employee TechnicalEvaluator => EmployeeProvider.GetEmployeeProvider().GetUserData(TechnicalEvaluatorID);

        public string ReviewerID { get; set; }

        public Employee Reviewer => EmployeeProvider.GetEmployeeProvider().GetUserData(ReviewerID);

        [Required]
        public string ApproverID { get; set; }

        public Employee Approver => EmployeeProvider.GetEmployeeProvider().GetUserData(ApproverID);

        public long? RecommendedOfferID { get; set; }

        [Display(Name = "Recommended Offer")]

        public Offer RecommendedOffer => RecommendedOfferID == null || Offers == null ? null : Offers.FirstOrDefault(o => o.OfferID == RecommendedOfferID);

        [Display(Name = "Recommendation Reason")]
        public string RecommendationReason { get; set; }

        [Display(Name = "Supporting Documents")]
        public MultipleFilesHandle SupportingDocuments { get; set; }

        public AuditDetail AuditDetail { get; set; } = new AuditDetail();
        public bool IsDeleted { get; set; }

        [Display(Name = "Workflow")]
        public virtual ICollection<EnquiryTransition> Transitions { get; set; }

        [NotMapped]
        [Display(Name = "Vendors To Enquire")]
        public string VendorEnquiries { get; set; }

        [NotMapped]
        public string[] VendorEnquiriesList
        {
            get => (VendorEnquiries != null) ? VendorEnquiries.Split(',') : new string[] { };
            set => VendorEnquiries = string.Join(",", value);
        }

        [NotMapped]
        [Display(Name = "Vendors Enquired")]
        public List<KeyValuePair<long, string>> VendorsEnquired => Offers.Where(o => o.IsNew == true).Select(c => new KeyValuePair<long, string>(c.OfferID, c.Company.CompanyName)).ToList();

        public virtual ICollection<Offer> Offers { get; set; }

        public object GetID() => EnquiryID;

        public string GetShortDescription()
        {
            return "Enquiry";
        }

        public string GetLongDescription()
        {
            return "Enquiry";
        }

        public bool IsSingleOffer => Offers.Where(o => o.IsNew).Count() == 1;

        public bool IsAllOffersAccepted => Offers.Any(o => o.PurchaseOrder != null && !o.PurchaseOrder.IsPurchaseOrderCancelled);

        public bool IsOfferAwarded => RecommendedOffer != null && (RecommendedOffer.PurchaseOrder != null && RecommendedOffer.PurchaseOrder.IsPurchaseOrderCancelled);

        public bool IsOfferAddable => Transitions.IsApproved();

        public StatusBidSummary StatusBidSummary { get; set; }
    }
}