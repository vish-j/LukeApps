using LukeApps.ApprovalProcess.Classes;
using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.TrackingExtended;

namespace LukeApps.GeneralPurchase.Models
{
    public class InvoiceTransition : Transition, ITransition, IEntity, IAuditDetail
    {
        public InvoiceTransition()
        {
            AuditDetail = new AuditDetail();
        }

        public InvoiceTransition(Transition t, long id) : base(t)
        {
            InvoiceID = id;
            AuditDetail = new AuditDetail();
        }

        public long InvoiceID { get; set; }

        public virtual Invoice Invoice { get; set; }

        public object GetID()
        {
            return InvoiceID;
        }

        public AuditDetail AuditDetail { get; set; }

        public bool IsDeleted { get; set; }
    }
}