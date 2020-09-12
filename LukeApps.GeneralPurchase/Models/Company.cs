using LukeApps.GeneralPurchase.Classes;
using LukeApps.GeneralPurchase.Enums;
using LukeApps.TrackingExtended;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LukeApps.GeneralPurchase.Models
{
    public class Company : IEntity, IAuditDetail
    {
        public Company()
        {
            CompanyFocalPoints = new HashSet<CompanyFocalPoint>();
            PurchaseOrders = new HashSet<PurchaseOrder>();
            BankAccounts = new HashSet<BankAccount>();
            Offers = new HashSet<Offer>();
            AuditDetail = new AuditDetail();
        }

        [Key]
        public long CompanyID { get; set; }

        [Display(Name = "Company Number")]
        public string CompanyNumber => CompanyID.ToString("000000");

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Registration Number (CR)")]
        public string CompanyRegistration { get; set; }

        [Display(Name = "Commodity Description")]
        public string CommodityDescription { get; set; }

        public string WebSite { get; set; }
        public Address Address { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        [Display(Name = "Block Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BlockStartDate { get; set; }

        [Display(Name = "Block End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BlockEndDate { get; set; }

        [Display(Name = "Reason")]
        [DataType(DataType.MultilineText)]
        public string BlockBlacklistReason { get; set; }

        [Display(Name = "Department Number")]
        public string DepartmentNumber { get; set; }

        [Display(Name = "Company Status")]
        public CompanyStatus CompanyStatus
        {
            get
            {
                DateTime now = DateTime.Now;
                if ((BlockEndDate ?? now) == DateTime.MaxValue)
                {
                    return CompanyStatus.BlackListed;
                }

                if ((BlockStartDate ?? now) < now && (BlockEndDate ?? now) > now)
                {
                    return CompanyStatus.Blocked;
                }

                return CompanyStatus.Registered;
            }
        }

        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }

        public CompanyFocalPoint DefaultFocalPoint => CompanyFocalPoints.First(c => c.IsDefault);
        public virtual ICollection<CompanyFocalPoint> CompanyFocalPoints { get; set; }
        public AuditDetail AuditDetail { get; set; } = new AuditDetail();
        public bool IsDeleted { get; set; }

        public object GetID() => CompanyID;
    }
}