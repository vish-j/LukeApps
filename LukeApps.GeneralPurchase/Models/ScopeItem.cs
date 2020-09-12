using LukeApps.AccountingDocumentor;
using LukeApps.GeneralPurchase.Enums;
using LukeApps.TrackingExtended;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
    public class ScopeItem : DocumentItem, IEntity, IAuditDetail
    {
        [Key]
        public long ScopeItemID { get; set; }

        public long OfferID { get; set; }

        public virtual Offer Offer { get; set; }

        public override object GetParentID() => OfferID;

        public AuditDetail AuditDetail { get; set; } = new AuditDetail();
        public bool IsDeleted { get; set; }

        public object GetID() => OfferID;
    }
}