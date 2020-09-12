using LukeApps.AccountingDocumentor;
using LukeApps.TrackingExtended;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
    public class ExpenseClaimItem : DocumentItem, IEntity, IAuditDetail
    {
        [Key]
        public long ExpenseClaimItemID { get; set; }

        public long ExpenseClaimID { get; set; }

        public virtual ExpenseClaim ExpenseClaim { get; set; }

        public override object GetParentID() => ExpenseClaimItemID;

        public AuditDetail AuditDetail { get; set; } = new AuditDetail();
        public bool IsDeleted { get; set; }

        public object GetID() => ExpenseClaimItemID;
    }
}