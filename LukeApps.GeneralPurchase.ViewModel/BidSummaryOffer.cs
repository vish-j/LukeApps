using LukeApps.AccountingDocumentor.Enums;
using LukeApps.Common.Helpers;
using LukeApps.CurrencyRates;
using LukeApps.GeneralPurchase.Classes;
using LukeApps.GeneralPurchase.Enums;
using LukeApps.GeneralPurchase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class BidSummaryOffer
    {
        private int _mainRows;

        private int _additionalRows;

        public BidSummaryOffer(Offer offer)
        {
            OfferID = offer.OfferID;

            CompanyName = offer.Company.CompanyName;
            Revision = offer.Revision;
            ReferenceNumber = offer.ReferenceNumber;
            VendorResponse = offer.VendorResponse;
            BidReceivedDate = offer.BidReceivedDate;
            ExpiryDate = VendorResponse == VendorResponse.Responded ? offer.ExpiryDate : null;
            AgreedPaymentTerms = offer.AgreedPaymentTerms;
            TotalPriceQuoted = offer.Total;
            ScopeItems = offer.ScopeItems.Where(s => s.ScopeItemType == ScopeItemType.Main || s.ScopeItemType == ScopeItemType.Additional).OrderBy(s => s.Order).ToList();
            IsNew = offer.IsNew;
            AdditionalTable = new List<List<object>>();
            initializeMainTable();
        }

        [Display(Name = "Offer ID")]
        public long OfferID { get; private set; }

        public string CompanyName { get; set; }
        public int Revision { get; private set; }

        public string OfferType => (Revision > 0) ? string.Format("Revised Offer Revision {0:00}", Revision) : "Original Offer";
        public string ReferenceNumber { get; set; }
        public VendorResponse VendorResponse { get; set; }

        [Display(Name = "Date of Received Bid")]
        public DateTime? BidReceivedDate { get; private set; }

        [Display(Name = "Expiry Date Of Offer")]
        public DateTime? ExpiryDate { get; private set; }

        [Display(Name = "Is Payment terms agreed?")]
        public bool IsPaymentTermsAgreed { get; private set; }

        [Display(Name = "Agreed Payment Terms")]
        public PaymentTerms AgreedPaymentTerms { get; private set; }

        [Display(Name = "Terms and Conditions agreed?")]
        public bool? IsTermsConditionsAgreed { get; private set; }

        [Display(Name = "Deviations Proposed in the Terms And Conditions")]
        public string TermsAndConditions { get; private set; }

        [Display(Name = "Technical Acceptability of the Offer")]
        public bool IsTechnicallyAcceptable { get; private set; }

        [Display(Name = "Commercial Acceptabilibty of the Offer")]
        public bool IsCommerciallyAcceptable { get; private set; }

        public Price TotalPriceQuoted { get; private set; }
        public List<ScopeItem> ScopeItems { get; private set; }

        public List<List<object>> MainTable { get; private set; }

        public int MainTableHeight => _mainRows;

        public int AdditionalTableHeight => _additionalRows;

        public List<List<object>> AdditionalTable { get; private set; }

        public string BriefDescription { get; private set; }
        public bool IsNew { get; private set; }

        private void initializeMainTable()
        {
            MainTable = new List<List<object>>();
            MainTable.Add(new List<object> { new { text = OfferType, colSpan = 4, bold = true, fontSize = 14, fillColor = "#EEE" }, new { }, new { }, new { } });
            MainTable.Add(new List<object> { new { text = CompanyName, colSpan = 3, bold = true, fillColor = "#EEE" }, new { }, new { }, new { text = (BidReceivedDate == null) ? "-" : BidReceivedDate?.ToString("dd-MM-yy"), fillColor = "#EEE" } });
            MainTable.Add(new List<object> { new { text = "", colSpan = 2, bold = true, fillColor = "#EEE" }, new { }, "Expiry: ", (ExpiryDate == null) ? "-" : ExpiryDate?.ToString("dd-MM-yy") });
            MainTable.Add(new List<object> { new { text = "", colSpan = 2, bold = true, fillColor = "#EEE" }, new { }, "Reference Number: ", ReferenceNumber ?? "-" });

            if (VendorResponse == VendorResponse.Responded)
            {
                MainTable.Add(new List<object> { new { text = "Description", bold = true }, new { text = "Quantity", bold = true }, new { text = "Unit Price", bold = true }, new { text = "Total", bold = true } });

                var i = 0;

                foreach (var item in ScopeItems)
                {
                    List<object> row = new List<object>();
                    row.Add((++i).ToString() + ". " + item.Description);

                    row.Add(item.Quantity);
                    row.Add(item.UnitPrice.ToString());
                    row.Add(item.TotalPrice.ToString());
                    MainTable.Add(row);
                }
                i = 0;
            }
            else
            {
                MainTable.Add(new List<object> { new { text = "Description", bold = true }, new { text = "Quantity", bold = true }, new { text = "Unit Price", bold = true }, new { text = "Total", bold = true } });
                MainTable.Add(new List<object> { new { colSpan = 4, rowSpan = 4, alignment = "center", text = BriefDescription }, new { }, new { }, new { } });
            }

            _mainRows = MainTable.Count;
        }

     
    }
}