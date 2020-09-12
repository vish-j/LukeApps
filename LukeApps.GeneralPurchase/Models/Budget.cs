using LukeApps.CurrencyRates;
using LukeApps.TrackingExtended;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
    public class Budget : IEntity, IAuditDetail
    {
        public Budget()
        {
            Enquiries = new HashSet<Enquiry>();
            PurchaseOrders = new HashSet<PurchaseOrder>();
            Invoices = new HashSet<Invoice>();
            AuditDetail = new AuditDetail();
        }

        [Key]
        public long BudgetID { get; set; }


        [Display(Name = "Budget Number")]
        public string BudgetNumber => BudgetID.ToString("000000");

        public string BudgetName { get; set; }

        public Price BudgetAmount { get; set; }

        public AuditDetail AuditDetail { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Enquiry> Enquiries { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }

        public object GetID() => BudgetID;
    }
}