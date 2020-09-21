using LukeApps.GeneralPurchase.Enums;
using LukeApps.GeneralPurchase.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class BidSummary
    {
        private const int numberOfDescriptiveLines = 6;

        private long[,] _bidsummaryGrid;

        private DateTime lastChangeDate;

        public BidSummary(Enquiry enquiry)
        {
            EnquiryNumber = enquiry.EnquiryID.ToString("00000");
            DedicatedProcurementRepresentative = enquiry.OriginatorID;
            Companies = enquiry.Offers.Where(o => o.IsNew).Select(o => new BidSummaryCompany(new Company()
            {
                CompanyName = $"{o.Company.CompanyName}",
                Offers = enquiry.Offers.Where(f => f.CompanyID == o.CompanyID).ToList()
            })).ToList();
            var offersLastChangeDate = enquiry.Offers.Max(c => c.AuditDetail.LastModifiedDate ?? c.AuditDetail.CreatedDate);
            var enquiryLastChangeDate = enquiry.AuditDetail.LastModifiedDate ?? enquiry.AuditDetail.CreatedDate;
            lastChangeDate = offersLastChangeDate > enquiryLastChangeDate ? offersLastChangeDate : enquiryLastChangeDate;
            Title = enquiry.SubjectOfEnquiry;
            RecommendationReason = enquiry.RecommendationReason;
            RecommendedOfferID = enquiry.RecommendedOfferID;
            MainTableHeight = Companies.Max(o => o.MainTableHeight);
            AdditionalTableHeight = Companies.Max(o => o.AdditionalTableHeight) + 1;
            setBidSummaryGrid();
            initializeTable();

            var list = AllOffers.Select(o => new { o.OfferID, o.CompanyName, o.ScopeItems.Count });
            ColumnWidths = AllOffers
                .Where(o => o.IsNew)
                .SelectMany(o => list.Where(l => l.CompanyName == o.CompanyName).All(l => l.Count == 0) ? new List<object> { 65, 50, 55, 40 } : new List<object> { '*', 50, 55, 50 })
                .ToList();

            ColumnWidths.Insert(0, '*');
        }

        public List<List<object>> Table { get; private set; }

        public string Title { get; private set; }

        public string RecommendationReason { get; private set; }

        public List<object> ColumnWidths { get; private set; }

        public long? RecommendedOfferID { get; private set; }

        private int MainTableHeight { get; set; }

        private int AdditionalTableHeight { get; set; }

        private string EnquiryNumber { get; set; }

        private string DedicatedProcurementRepresentative { get; set; }

        private List<BidSummaryCompany> Companies { get; set; }

        private List<BidSummaryOffer> AllOffers { get; set; }

        private void setBidSummaryGrid()
        {
            long[,] grid = new long[Companies.Max(c => c.CurrentRev) + 1, Companies.Count];
            int row = 0;
            int col = 0;
            AllOffers = new List<BidSummaryOffer>();
            foreach (var company in Companies)
            {
                row = 0;
                foreach (var offer in company.Offers)
                {
                    grid[row, col] = offer.OfferID;
                    AllOffers.Add(offer);
                    row++;
                }
                col++;
            }
            _bidsummaryGrid = grid;
        }

        private void initializeTable()
        {
            List<List<object>> table = new List<List<object>>
            {
                new List<object>() { "", "Oman Meat" },
                new List<object>() { "", "Address 1" },
                new List<object>() { "", "Address 2" },
                new List<object>() { "", "Sultanate of Oman" },
                new List<object>() { "" },
                new List<object>() { "" },
                new List<object>() { "", "Comparison Table" },
                new List<object>() { "", Title },
                new List<object>() { "Date : ", lastChangeDate.ToString("dd-MM-yy") }
            };

            var sectionRowStartIndex = table.Count;

            var c = 0;
            List<object> recommendedOffer = new List<object>() { "Award Recommendation" };
            List<object> recommendation = new List<object>() { "Reasons for recommendation" };

            for (int i = 0; i < _bidsummaryGrid.GetLength(0); i++)
            {
                table.Add(new List<object>() { new { rowSpan = MainTableHeight + AdditionalTableHeight, text = "", fillColor = "#EEE" } });
                for (int s = 1; s < MainTableHeight; s++)
                {
                    table.Add(new List<object>() { new { } });
                }

                for (int s = 0; s < AdditionalTableHeight; s++)
                {
                    table.Add(new List<object>() { new { } });
                }

                table.Add(new List<object>() { "" });
                table.Add(new List<object>() { "Total Amount" });
                //table.Add(new List<object>() { "PROPOSED PAYMENTS" });

                table.Add(new List<object>() { "Accept T&P Std. Terms & Conditions" });
                //table.Add(new List<object>() { "90 Days Payment Terms" });
                table.Add(new List<object>() { "Technical Acceptable" });
                table.Add(new List<object>() { "Commercial Acceptable" });
                table.Add(new List<object>() { "" });

                for (int j = 0; j < _bidsummaryGrid.GetLength(1); j++)
                {
                    List<List<object>> temptable = new List<List<object>>();
                    if (_bidsummaryGrid[i, j] != 0)
                    {
                        var offer = AllOffers.Find(f => f.OfferID == _bidsummaryGrid[i, j]);
                        temptable = offer.MainTable;
                        int extraRows = (MainTableHeight - offer.MainTable.Count) + 1;

                        for (int s = 0; s < extraRows; ++s)
                        {
                            temptable.Add(new List<object> { new { colSpan = 4, text = "" }, new { }, new { }, new { } });
                        }

                        if (offer.AdditionalTable.Count != 0)
                        {
                            temptable.Add(new List<object> { "", "Additional (if required by Oman Meat)", "", "" });
                            temptable.AddRange(offer.AdditionalTable);
                        }

                        int extraAddrows = AdditionalTableHeight - offer.AdditionalTable.Count;
                        if (extraAddrows > 0)
                        {
                            for (int s = 0; s < extraAddrows; ++s)
                            {
                                temptable.Add(new List<object>() { new { colSpan = 4, text = "" }, new { }, new { }, new { } });
                            }
                        }
                        if (offer.VendorResponse == VendorResponse.Responded)
                        {
                            temptable.Add(new List<object> { "", "", "", offer.TotalPriceQuoted.ToString() });
                            //temptable.Add(new List<object> { "", offer.AgreedPaymentTerms.Entitlement, "", "" });

                            temptable.Add(new List<object> { "", (offer.IsTermsConditionsAgreed == null) ? "Not yet agreed" : ((bool)offer.IsTermsConditionsAgreed) ? "Yes" : "No, " + offer.TermsAndConditions, "", "" });
                            //temptable.Add(new List<object> { "", (offer.AgreedPaymentTerms.Term == null) ? "" : ((offer.AgreedPaymentTerms.Term ?? 0) == 90)? "Yes" : "No, " + offer.AgreedPaymentTerms.Term?.ToString("00 Days"), "", "" });
                            temptable.Add(new List<object> { "", (offer.IsTechnicallyAcceptable) ? "Yes" : "No", "", "" });
                            temptable.Add(new List<object> { "", (offer.IsCommerciallyAcceptable) ? "Yes" : "No", "", "" });
                            temptable.Add(new List<object> { new { colSpan = 4, text = "" }, new { }, new { }, new { } });
                        }
                        else
                        {
                            temptable.Add(new List<object> { new { colSpan = 4, rowSpan = numberOfDescriptiveLines - 1, text = "" }, new { }, new { }, new { } });
                            for (int a = 1; a < numberOfDescriptiveLines - 1; a++)
                            {
                                temptable.Add(new List<object> { new { }, new { }, new { }, new { } });
                            }
                        }
                    }
                    else
                    {
                        temptable = getemptyTable();
                    }
                    for (int s = sectionRowStartIndex; (s - sectionRowStartIndex) < (MainTableHeight + AdditionalTableHeight + numberOfDescriptiveLines); s++)
                    {
                        table.ElementAt(s).AddRange(temptable.ElementAt(s - sectionRowStartIndex));
                    }

                    if (_bidsummaryGrid[i, j] == RecommendedOfferID)
                    {
                        c = (j + 1) * 4;
                        for (int m = 1; m <= c; m++)
                        {
                            if ((m + 3) % 4 == 0)
                            {
                                recommendedOffer.Add(new { text = (c - 3 == m) ? "Yes" : "No", colSpan = 4 });
                                recommendation.Add(new { text = (c - 3 == m) ? RecommendationReason : "", colSpan = 4 });
                            }
                            else
                            {
                                recommendedOffer.Add(new { });
                                recommendation.Add(new { });
                            }
                        }

                        c = (_bidsummaryGrid.GetLength(1) - (j + 1)) * 4;

                        for (int n = 1; n <= c; n++)
                        {
                            if ((n + 3) % 4 == 0)
                            {
                                recommendedOffer.Add(new { text = "No", colSpan = 4 });
                                recommendation.Add(new { text = "", colSpan = 4 });
                            }
                            else
                            {
                                recommendedOffer.Add(new { });
                                recommendation.Add(new { });
                            }
                        }
                    }
                }
                sectionRowStartIndex = table.Count;
            }

            table.Add(recommendedOffer);
            table.Add(recommendation);
            Table = table;
        }

        private List<List<object>> getemptyTable()
        {
            List<List<object>> emptyTable = new List<List<object>>();
            int totalheight = numberOfDescriptiveLines + MainTableHeight + AdditionalTableHeight;
            emptyTable.Add(new List<object>() { new { colSpan = 4, rowSpan = totalheight, text = "" }, new { }, new { }, new { } });
            for (int e = 1; e < totalheight; e++)
            {
                emptyTable.Add(new List<object>() { new { }, new { }, new { }, new { } });
            }
            return emptyTable;
        }
    }
}