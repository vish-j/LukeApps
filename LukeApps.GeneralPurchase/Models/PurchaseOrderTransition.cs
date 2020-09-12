using LukeApps.ApprovalProcess.Classes;
using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.TrackingExtended;

namespace LukeApps.GeneralPurchase.Models
{
    public class PurchaseOrderTransition : Transition, ITransition, IEntity, IAuditDetail
    {
        public PurchaseOrderTransition()
        {
            AuditDetail = new AuditDetail();
        }

        public PurchaseOrderTransition(Transition t, long agreementID) : base(t)
        {
            PurchaseOrderID = agreementID;
            AuditDetail = new AuditDetail();
        }

        public long PurchaseOrderID { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public object GetID()
        {
            return PurchaseOrderID;
        }

        public AuditDetail AuditDetail { get; set; }

        public bool IsDeleted { get; set; }
    }
}