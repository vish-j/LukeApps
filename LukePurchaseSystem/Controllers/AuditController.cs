using LukeApps.Authorization.Attributes;
using LukeApps.Authorization.RoleMap;
using LukeApps.GeneralPurchase.DAL;
using LukeApps.GeneralPurchase.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace LukePurchaseSystem.Controllers
{
    [AuthorizeRoles(Role.Dev)]
    public class AuditController : AuditBaseController
    {
        public AuditController() => Db = new PurchaseEntities(System.Web.HttpContext.Current.User.Identity.Name);

        internal override void SetLog(string type, long? id)
        {
            switch (type)
            {
                case "Enquiries":

                    var enquiry = ((PurchaseEntities)Db).Enquiries.Include(e => e.Transitions).Include(e => e.Offers).FirstOrDefault(e => e.EnquiryID == id);

                    if (enquiry != null)
                    {
                        Log = GetTrail<Enquiry>((long)id);

                        var etids = enquiry.Transitions.Select(t => t.TransitionID).ToArray();
                        foreach (var etid in etids)
                            Log.AddRange(GetTrail<EnquiryTransition>(etid));

                        var oids = enquiry.Offers.Select(o => o.OfferID).ToArray();
                        foreach (var oid in oids)
                            Log.AddRange(GetTrail<Offer>(oid));

                        var sids = enquiry.Offers.SelectMany(o => o.ScopeItems).Select(o => o.OfferID).ToArray();
                        foreach (var sid in sids)
                            Log.AddRange(GetTrail<ScopeItem>(sid));
                    }

                    break;

                case "ExpenseClaims":

                    var expenseClaim = ((PurchaseEntities)Db).ExpenseClaims.Include(e => e.Transitions).Include(e => e.ExpenseClaimItems).FirstOrDefault(e => e.ExpenseClaimID == id);
                    if (expenseClaim != null)
                    {
                        Log = GetTrail<ExpenseClaim>((long)id);

                        var atids = expenseClaim.Transitions.Select(o => o.TransitionID).ToArray();
                        foreach (var atid in atids)
                            Log.AddRange(GetTrail<ExpenseClaimTransition>(atid));

                        var itids = expenseClaim.ExpenseClaimItems.Select(o => o.ExpenseClaimItemID).ToArray();
                        foreach (var itid in itids)
                            Log.AddRange(GetTrail<ExpenseClaimItem>(itid));
                    }

                    break;

                default:
                    Log = null;
                    break;
            }
        }
    }
}