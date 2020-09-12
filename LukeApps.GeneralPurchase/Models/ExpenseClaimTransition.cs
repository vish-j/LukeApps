using LukeApps.ApprovalProcess.Classes;
using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.TrackingExtended;

namespace LukeApps.GeneralPurchase.Models
{
    public class ExpenseClaimTransition : Transition, ITransition, IEntity, IAuditDetail
    {
        public ExpenseClaimTransition()
        {
            AuditDetail = new AuditDetail();
        }

        public ExpenseClaimTransition(Transition t, long id) : base(t)
        {
            ExpenseClaimID = id;
            AuditDetail = new AuditDetail();
        }

        public long ExpenseClaimID { get; set; }

        public virtual ExpenseClaim ExpenseClaim { get; set; }

        public object GetID()
        {
            return ExpenseClaimID;
        }

        public AuditDetail AuditDetail { get; set; }

        public bool IsDeleted { get; set; }
    }
}