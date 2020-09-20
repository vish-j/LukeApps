using LukeApps.ApprovalProcess;
using LukeApps.CurrencyRates;
using LukeApps.TrackingExtended;
using PhilApprovalFlow;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        [Required]
        [Display(Name = "Budget Name")]
        public string BudgetName { get; set; }

        [Display(Name = "Budget Amount")]
        public Price BudgetAmount { get; set; }

        public Price Balance => getBalance();

        private Price getBalance()
        {
           return new Price(CurrencyRates.Enums.CurrencyCode.OMR,  PurchaseOrders.Where(p => p.IsPurchaseOrderClosed && !p.IsPurchaseOrderCancelled).Sum(p => p.Total.DefaultCurrencyValue) + PurchaseOrders.Where(p => p.Transitions.IsApproved()).Sum(p => p.Total.DefaultCurrencyValue));
        }

        public AuditDetail AuditDetail { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Enquiry> Enquiries { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<ExpenseClaim> ExpenseClaims { get; set; }
        public object GetID() => BudgetID;
    }
}