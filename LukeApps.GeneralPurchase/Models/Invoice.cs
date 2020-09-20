using LukeApps.AccountingDocumentor;
using LukeApps.AccountingDocumentor.Interfaces;
using LukeApps.ApprovalProcess;
using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.EmployeeData;
using LukeApps.FileHandling;
using LukeApps.TrackingExtended;
using PhilApprovalFlow;
using PhilApprovalFlow.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
    [DocumentPAFPath("Invoices")]
    [PAFMetadata(Key = "app", Value = "Invoice")]
    public class Invoice : AccountingDocument, IApprovalFlow<InvoiceTransition>, IEntity, IAuditDetail, IPAFMatrix
    {
        public Invoice()
        {
            AuditDetail = new AuditDetail();
            Documents = new MultipleFilesHandle();
            PurchaseOrders = new HashSet<PurchaseOrder>();
            Transitions = new HashSet<InvoiceTransition>();
        }

        [Key]
        public long InvoiceID { get; set; }

        [Display(Name = "Invoice Number")]
        public string InvoiceNumber => InvoiceID.ToString("000000");

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
        public MultipleFilesHandle Documents { get; set; }

        public AuditDetail AuditDetail { get; set; }
        public bool IsDeleted { get; set; }

        [Display(Name = "Workflow")]
        public virtual ICollection<InvoiceTransition> Transitions { get; set; }

        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        [Display(Name = "Line Items")]
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }

        public object GetID() => InvoiceID;

        public override IEnumerable<IScopeItem> GetScopeItems() => InvoiceItems;

        public string GetShortDescription()
        {
            return "Invoice";
        }

        public string GetLongDescription()
        {
            return "Invoice";
        }
    }
}