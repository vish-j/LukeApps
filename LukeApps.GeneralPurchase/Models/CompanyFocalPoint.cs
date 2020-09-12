using LukeApps.TrackingExtended;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
    public class CompanyFocalPoint : IEntity, IAuditDetail
    {
        [Key]
        public long CompanyFocalPointID { get; set; }

        public long CompanyID { get; set; }
        public virtual Company Company { get; set; }

        [Display(Name = "Name of the Company representative")]
        public string ContactName { get; set; }

        [Display(Name = "Mobile")]
        public string MobileNumber { get; set; }

        public string Phone { get; set; }

        public int? Ext { get; set; }

        public string Fax { get; set; }

        [Display(Name = "Email Address")]
        public string Email1 { get; set; }

        [Display(Name = "Email Address (Additional)")]
        public string Email2 { get; set; }

        public AuditDetail AuditDetail { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDefault { get; set; }

        public object GetID() => CompanyFocalPointID;
    }
}