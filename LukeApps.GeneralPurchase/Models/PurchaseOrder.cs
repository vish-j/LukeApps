using LukeApps.AccountingDocumentor;
using LukeApps.AccountingDocumentor.Interfaces;
using LukeApps.ApprovalProcess;
using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.EmployeeData;
using LukeApps.FileHandling;
using LukeApps.GeneralPurchase.Classes;
using LukeApps.TrackingExtended;
using PhilApprovalFlow;
using PhilApprovalFlow.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
    [DocumentPAFPath("PurchaseOrders")]
    [PAFMetadata(Key = "app", Value = "PurchaseOrder")]
    public class PurchaseOrder : AccountingDocument, IApprovalFlow<PurchaseOrderTransition>, IEntity, IAuditDetail, IPAFMatrix
    {
        public PurchaseOrder()
        {
            ApprovedDocuments = new MultipleFilesHandle();
            AuditDetail = new AuditDetail();
            PurchaseOrderItems = new HashSet<PurchaseOrderItem>();
            Invoices = new HashSet<Invoice>();
            Transitions = new HashSet<PurchaseOrderTransition>();
        }

        [Key]
        public long PurchaseOrderID { get; set; }

        [Display(Name = "Purchase Order Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseOrderDate { get; set; } = DateTime.Now;

        [Display(Name = "PurchaseOrder Number")]
        public string PurchaseOrderNumber => PurchaseOrderID.ToString("000000");

        public string Summary => $"PO:{PurchaseOrderNumber} - { Company.CompanyName}";

        public long BudgetID { get; set; }
        public virtual Budget Budget { get; set; }

        [Required]
        public string OriginatorID { get; set; }

        public Employee Originator => EmployeeProvider.GetEmployeeProvider().GetUserData(OriginatorID);

        public string ReviewerID { get; set; }

        public Employee Reviewer => EmployeeProvider.GetEmployeeProvider().GetUserData(ReviewerID);

        [Required]
        public string ApproverID { get; set; }

        public Employee Approver => EmployeeProvider.GetEmployeeProvider().GetUserData(ApproverID);

        [Display(Name = "Approved Documents")]
        public MultipleFilesHandle ApprovedDocuments { get; set; }

        [Display(Name = "Purchase Order Expiry Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseOrderExpiryDate { get; set; }

        public AuditDetail AuditDetail { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Offer Offer { get; set; }

        [Display(Name = "Line Items")]
        public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        [Display(Name = "Workflow")]
        public virtual ICollection<PurchaseOrderTransition> Transitions { get; set; }

        public int Revision { get; set; }

        [Display(Name = "Sequence Number")]
        public int SequenceNumber { get; set; }

        [Display(Name = "Company Section")]
        public string CompanySection { get; set; }

        public long CompanyID { get; set; }
        public virtual Company Company { get; set; }

        [Display(Name = "Company Contact Name")]
        public string CompanyContactName { get; set; }

        [Display(Name = "Offer Reference")]
        public string OfferReference { get; set; }

        [Display(Name = "Shipping Section")]
        public string ShippingSection { get; set; }

        [Display(Name = "Originator Name")]
        public string OriginatorName { get; set; }

        [Display(Name = "Originator Phone")]
        public string OriginatorPhone { get; set; }

        public string TechnicalEvaluatorID { get; set; }

        [Display(Name = "Technical Evaluator")]
        public Employee TechnicalEvaluator => EmployeeProvider.GetEmployeeProvider().GetUserData(TechnicalEvaluatorID);

        [Display(Name = "Technical Evaluator Name")]
        public string TechnicalEvaluatorName { get; set; }

        [Display(Name = "Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Delivery Terms")]
        public string DeliveryTerms { get; set; }

        [Display(Name = "Payment Terms")]
        public PaymentTerms PaymentTerms { get; set; }

        [Display(Name = "Approver Name")]
        public string ApproverName { get; set; }

        [Display(Name = "Approver Position")]
        public string ApproverPosition { get; set; }

        [Display(Name = "Reviewer Name")]
        public string ReviewerName { get; set; }

        [Display(Name = "Reviewer Position")]
        public string ReviewerPosition { get; set; }

        [Display(Name = "Close Date")]
        public DateTime? CloseDate { get; set; }

        [Display(Name = "Is Purchase Order Closed?")]
        public bool IsPurchaseOrderClosed => CloseDate != null;

        [Display(Name = "Cancel Date")]
        public DateTime? CancelDate { get; set; }

        [Display(Name = "Is Purchase Order Cancelled?")]
        public bool IsPurchaseOrderCancelled => CancelDate != null;

        public object GetID() => PurchaseOrderID;

        public override IEnumerable<IScopeItem> GetScopeItems() => PurchaseOrderItems;

        public string GetShortDescription()
        {
            return "Purchase Order";
        }

        public string GetLongDescription()
        {
            return "Purchase Order";
        }
    }
}