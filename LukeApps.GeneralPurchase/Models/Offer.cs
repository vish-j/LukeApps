using LukeApps.AccountingDocumentor;
using LukeApps.AccountingDocumentor.Interfaces;
using LukeApps.FileHandling;
using LukeApps.GeneralPurchase.Classes;
using LukeApps.GeneralPurchase.Enums;
using LukeApps.TrackingExtended;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.Models
{
    public class Offer : AccountingDocument, IEntity, IAuditDetail
    {
        public Offer()
        {
            ScopeItems = new HashSet<ScopeItem>();
        }

        [Key]
        public long OfferID { get; set; }

        public string OfferNumber => OfferID.ToString("000000");

        [Display(Name = "Bid Status")]
        public VendorResponse VendorResponse { get; set; }

        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }

        public long CompanyID { get; set; }

        public virtual Company Company { get; set; }

        public long EnquiryID { get; set; }

        public virtual Enquiry Enquiry { get; set; }

        public int Revision { get; set; }

        [Display(Name = "Date of Received Bid")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BidReceivedDate { get; set; } = DateTime.Now;

        [Display(Name = "Expiry Date Of Offer")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; } = DateTime.Now.AddMonths(3);

        public MultipleFilesHandle Quotation { get; set; } = new MultipleFilesHandle();

        [Display(Name = "Delivery Terms")]
        [DataType(DataType.MultilineText)]
        public string DeliveryTerms { get; set; }

        [Display(Name = "Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "Agreed Payment Terms")]
        public PaymentTerms AgreedPaymentTerms { get; set; } = new PaymentTerms();


        [Display(Name = "Scope Items")]
        public virtual ICollection<ScopeItem> ScopeItems { get; set; }

        public virtual Offer PreviousOffer { get; set; }

        public bool IsOfferExpired => (ExpiryDate ?? DateTime.MaxValue).Date < DateTime.Now.Date;
        public bool IsNew { get; set; }


        public List<long> OfferJourney => _offerJourney;

        private List<long> _offerJourney;

        private void setofferJourney(Offer offer)
        {
            if (_offerJourney == null)
                _offerJourney = new List<long>();

            _offerJourney.Add(offer.OfferID);
            if (offer.PreviousOffer != null)
            {
                setofferJourney(offer.PreviousOffer);
            }
        }

        public bool IsOfferAccepted => PurchaseOrder != null;
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public AuditDetail AuditDetail { get; set; } = new AuditDetail();
        public bool IsDeleted { get; set; }

        public object GetID() => OfferID;

        public override IEnumerable<IScopeItem> GetScopeItems() => ScopeItems;
    }
}