using LukeApps.GeneralPurchase.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class BidSummaryCompany
    {
        public BidSummaryCompany(Company company)
        {
            CompanyName = company.CompanyName;
            Offers = company.Offers.OrderBy(o => o.Revision).Select(o => new BidSummaryOffer(o)).ToList();
            CurrentRev = company.Offers.Max(o => o.Revision);
            MainTableHeight = Offers.Max(o => o.MainTableHeight);
            AdditionalTableHeight = Offers.Max(o => o.AdditionalTableHeight);
        }

        [Display(Name = "Company Name")]
        private string CompanyName { get; set; }

        public List<BidSummaryOffer> Offers { get; private set; }

        public int CurrentRev { get; private set; }
        public int MainTableHeight { get; private set; }

        public int AdditionalTableHeight { get; private set; }
    }
}