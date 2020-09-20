using LukeApps.AccountingDocumentor;
using LukeApps.AccountingDocumentor.Interfaces;
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

namespace LukeApps.GeneralPurchase.Models
{
    [DocumentPAFPath("ExpenseClaims")]
    [PAFMetadata(Key = "app", Value = "ExpenseClaim")]
    public class ExpenseClaim : AccountingDocument, IApprovalFlow<ExpenseClaimTransition>, IEntity, IAuditDetail, IPAFMatrix
    {
        public ExpenseClaim()
        {
            AuditDetail = new AuditDetail();
            SupportingDocuments = new MultipleFilesHandle();
            ExpenseClaimItems = new HashSet<ExpenseClaimItem>();
            Transitions = new HashSet<ExpenseClaimTransition>();
        }

        [Key]
        public long ExpenseClaimID { get; set; }

        [Display(Name = "Expense-Claim Number")]
        public string ExpenseClaimNumber => ExpenseClaimID.ToString("000000");

        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Display(Name = "Bank Account Number")]
        public string BankAccountNumber { get; set; }

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

        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }

        [Display(Name = "Supporting Documents")]
        public MultipleFilesHandle SupportingDocuments { get; set; }

        public AuditDetail AuditDetail { get; set; } = new AuditDetail();
        public bool IsDeleted { get; set; }

        [Display(Name = "Line Items")]
        public virtual ICollection<ExpenseClaimItem> ExpenseClaimItems { get; set; }

        public virtual ICollection<ExpenseClaimTransition> Transitions { get; set; }

        public object GetID() => ExpenseClaimID;

        public override IEnumerable<IScopeItem> GetScopeItems() => ExpenseClaimItems;

        public string GetShortDescription()
        {
            return "Expense Claim";
        }

        public string GetLongDescription()
        {
            return "Expense Claim";
        }
    }
}