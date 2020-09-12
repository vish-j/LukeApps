using LukeApps.AccountingDocumentor;
using LukeApps.TrackingExtended;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
    public class PurchaseOrderItem : DocumentItem, IEntity, IAuditDetail
    {
        [Key]
        public long PurchaseOrderItemID { get; set; }

        public long PurchaseOrderID { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public override object GetParentID() => PurchaseOrderID;

        public AuditDetail AuditDetail { get; set; } = new AuditDetail();
        public bool IsDeleted { get; set; }

        public object GetID() => PurchaseOrderItemID;
    }
}