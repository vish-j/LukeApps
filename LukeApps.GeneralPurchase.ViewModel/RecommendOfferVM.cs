using LukeApps.GeneralPurchase.Models;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.GeneralPurchase.ViewModel
{
    public class RecommendOfferVM
    {
        public RecommendOfferVM()
        {
        }

        public RecommendOfferVM(Offer offer, long? recommendedOfferID = null)
        {
            EnquiryID = offer.EnquiryID;
            RecommendationReason = offer.Enquiry.RecommendationReason;
            RecommendedOfferID = recommendedOfferID ?? offer.Enquiry.RecommendedOfferID ?? 0;
        }

        public long EnquiryID { get; set; }

        [Display(Name = "Reason for Recommendation")]
        [DataType(DataType.MultilineText)]
        public string RecommendationReason { get; set; }

        public long RecommendedOfferID { get; set; }
    }
}