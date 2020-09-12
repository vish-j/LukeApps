using LukeApps.AccountingDocumentor;
using LukeApps.AccountingDocumentor.Interfaces;
using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.EmployeeData;
using LukeApps.FileHandling;
using LukeApps.GeneralPurchase.Classes;
using LukeApps.TrackingExtended;
using PhilApprovalFlow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
    public class PurchaseOrder : AccountingDocument, IApprovalFlow<PurchaseOrderTransition>, IEntity, IAuditDetail, IPAFMatrix
    {
        public PurchaseOrder()
        {
            AuditDetail = new AuditDetail();
            PurchaseOrderItems = new HashSet<PurchaseOrderItem>();
            Invoices = new HashSet<Invoice>();
            Transitions = new HashSet<PurchaseOrderTransition>();
        }

        [Key]
        public long PurchaseOrderID { get; set; }


        [Display(Name = "PurchaseOrder Number")]
        public string PurchaseOrderNumber => PurchaseOrderID.ToString("000000");
        public long BudgetID { get; set; }
        public virtual Budget Budget { get; set; }

        public string OriginatorID { get; set; }

        public Employee Originator => EmployeeProvider.GetEmployeeProvider().GetUserData(OriginatorID);

        public string ReviewerID { get; set; }

        public Employee Reviewer => EmployeeProvider.GetEmployeeProvider().GetUserData(ReviewerID);

        public string ApproverID { get; set; }

        public Employee Approver => EmployeeProvider.GetEmployeeProvider().GetUserData(ApproverID);

        public MultipleFilesHandle ApprovedDocuments { get; set; }

        public bool IsPurchaseOrderCancelled { get; set; }

        [Display(Name = "Purchase Order Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseOrderExpiryDate { get; set; }

        public AuditDetail AuditDetail { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Offer Offer { get; set; }
        public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<PurchaseOrderTransition> Transitions { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public int Revision { get; set; }
        public int SequenceNumber { get; set; }
        public string CompanySection { get; set; }
        public long CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public string CompanyContactName { get; set; }
        public string OfferReference { get; set; }
        public string ShippingSection { get; set; }
        public string OriginatorName { get; set; }
        public string OriginatorPhone { get; set; }
        public string TechnicalEvaluatorID { get; set; }
        public Employee TechnicalEvaluator => EmployeeProvider.GetEmployeeProvider().GetUserData(TechnicalEvaluatorID);
        public string TechnicalEvaluatorName { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryTerms { get; set; }
        public PaymentTerms PaymentTerms { get; set; }
        public string ApproverName { get; set; }
        public string ApproverPosition { get; set; }
        public string ReviewerName { get; set; }
        public string ReviewerPosition { get; set; }
        public bool IsPurchaseOrderClosed { get; set; }

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