using LukeApps.AccountingDocumentor;
using LukeApps.AccountingDocumentor.Interfaces;
using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.EmployeeData;
using LukeApps.FileHandling;
using LukeApps.GeneralPurchase.Enums;
using LukeApps.TrackingExtended;
using PhilApprovalFlow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
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

        [Display(Name = "ExpenseClaim Number")]
        public string ExpenseClaimNumber => ExpenseClaimID.ToString("000000");
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Display(Name = "Bank Account Number")]
        public string BankAccountNumber { get; set; }

        public string OriginatorID { get; set; }

        public Employee Originator => EmployeeProvider.GetEmployeeProvider().GetUserData(OriginatorID);

        public string ReviewerID { get; set; }

        public Employee Reviewer => EmployeeProvider.GetEmployeeProvider().GetUserData(ReviewerID);

        public string ApproverID { get; set; }

        public Employee Approver => EmployeeProvider.GetEmployeeProvider().GetUserData(ApproverID);

        public PaymentMethod PaymentMethod { get; set; }

        public MultipleFilesHandle SupportingDocuments { get; set; }

        public AuditDetail AuditDetail { get; set; } = new AuditDetail();
        public bool IsDeleted { get; set; }
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