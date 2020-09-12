using LukeApps.AccountingDocumentor;
using LukeApps.TrackingExtended;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
    public class InvoiceItem : DocumentItem, IEntity, IAuditDetail
    {
        [Key]
        public long InvoiceItemID { get; set; }

        public long InvoiceID { get; set; }

        public virtual Invoice Invoice { get; set; }

        public override object GetParentID() => InvoiceID;

        public AuditDetail AuditDetail { get; set; } = new AuditDetail();
        public bool IsDeleted { get; set; }

        public object GetID() => InvoiceItemID;
    }
}