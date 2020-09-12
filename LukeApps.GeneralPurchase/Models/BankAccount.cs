using LukeApps.GeneralPurchase.Classes;
using LukeApps.TrackingExtended;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
    public class BankAccount : IEntity, IAuditDetail
    {
        public BankAccount()
        {
            AuditDetail = new AuditDetail();
        }

        [Key]
        public long BankAccountID { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Bank Account Name")]
        public string BackAccountName { get; set; }

        [Display(Name = "Back Account Number")]
        public string BackAccountNumber { get; set; }

        public string IBAN { get; set; }

        public Address Address { get; set; }

        [Display(Name = "Sort Code")]
        public string SortCode { get; set; }

        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }

        [Display(Name = "Routing Code")]
        public string RoutingCode { get; set; }

        public long CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public AuditDetail AuditDetail { get; set; }
        public bool IsDeleted { get; set; }

        public object GetID() => BankAccountID;
    }
}