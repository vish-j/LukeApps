using LukeApps.CurrencyRates.Enums;
using LukeApps.TrackingExtended;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.CurrencyRates.Models
{
    public class Currency : IEntity, IAuditDetail
    {
        public Currency()
        {
            AuditDetail = new AuditDetail();
        }

        [Key]
        [Display(Name = "Currency ID")]
        public long CurrencyID { get; set; }

        public CurrencyCode CurrencyCode { get; set; }

        [Required]
        [Display(Name = "Rate - OMR")]
        public double CurrencyRateDefault { get; set; }

        public AuditDetail AuditDetail { get; set; }

        public bool IsDeleted { get; set; }
        public object GetID() => CurrencyID;
    }
}