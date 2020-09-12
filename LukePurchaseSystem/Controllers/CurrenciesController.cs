using LukeApps.Authorization.Attributes;
using LukeApps.Authorization.RoleMap;
using LukeApps.Common.Helpers;
using LukeApps.CurrencyRates.DAL;
using LukeApps.CurrencyRates.Enums;
using LukeApps.CurrencyRates.Models;
using LukeApps.GeneralPurchase.ViewModel;
using LukeApps.GenericRepository;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LukePurchaseSystem.Controllers
{
    public class CurrenciesController : ExtendedController
    {
        private GenericRepository<CurrencyEntities, Currency> repo;

        public CurrenciesController() =>
            repo = new GenericRepository<CurrencyEntities, Currency>(System.Web.HttpContext.Current.User.Identity.Name);

        public CurrenciesController(string Username) =>
            repo = new GenericRepository<CurrencyEntities, Currency>(Username);

        // GET: ManagementReference/Currencies
        [AuthorizeRoles(Role.Dev, Role.MRProcurement)]
        public ActionResult Index(string ErrorMessage = null, long? id = null)
        {
            buildViewBag(ErrorMessage, id);

            return View();
        }

        private void buildViewBag(string ErrorMessage = null, long? id = null)
        {
            if (id != null)
                ViewBag.ModalID = id;
            if (ErrorMessage != null)
                ViewBag.ErrorMessage = ErrorMessage;
        }

        //fast json export to client
        public async Task<JsonResult> GetJSON()
        {
            var latestCurrencyValues = from t in repo.Context.Currencies
                                       group t by t.CurrencyCode
                        into g
                                       select new
                                       {
                                           CurrencyID = g.Select(x => x.CurrencyID),
                                           DateOfRecord = (from t2 in g select t2.AuditDetail.CreatedDate).Max()
                                       };

            long[] latestCurrencyIDS = latestCurrencyValues.SelectMany(s => s.CurrencyID).ToArray();

            var currencies = await repo.Context.Currencies.Where(c => latestCurrencyIDS.Contains(c.CurrencyID)).ToListAsync();
            var detailCollection = currencies.Select(c => new
            {
                CurrencyID = c.CurrencyID,
                CurrencyCodeID = c.CurrencyCode.ToString(),
                CurrencyCode = c.CurrencyCode.GetDisplay(),
                CurrencyRateEuro = c.CurrencyRateDefault,
                AuditDetail_CreatedDate = c.AuditDetail.CreatedDate.ToShortDateISO(),
                AuditDetail_CreatedEntryUser = c.AuditDetail.CreatedEntryUserDisplayName,
                AuditDetail_LastModifiedDate = c.AuditDetail.LastModifiedDate?.ToShortDateISO(),
                AuditDetail_LastModifiedEntryUser = c.AuditDetail.LastModifiedEntryUserDisplayName
            });
            var TotalRecords = detailCollection.Count();
            return Json(new
            {
                iTotalRecords = TotalRecords,
                iTotalDisplayRecords = TotalRecords,
                aaData = detailCollection
            },
            JsonRequestBehavior.AllowGet);
        }

        // GET: ManagementReference/Currencies/Details/5
        [AuthorizeRoles(Role.Dev, Role.MRProcurement)]
        public async Task<ActionResult> Details(CurrencyCode? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Currency> currencies = await repo.Context.Currencies.Where(c => c.CurrencyCode == id).ToListAsync();

            if (currencies.Count == 0)
            {
                return HttpNotFound();
            }
            return PartialView(new CurrencyDetailsVM(currencies));
        }

        // GET: ManagementReference/Currencies/Update
        [AuthorizeRoles(Role.Dev, Role.MRProcurement)]
        public ActionResult Update(CurrencyCode curr) =>
            View(new Currency() { CurrencyCode = curr });

        // POST: ManagementReference/Currencies/Update
        [HttpPost]
        [AuthorizeRoles(Role.Dev, Role.MRProcurement)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(Currency currency)
        {
            if (ModelState.IsValid)
            {

                repo.Context.Currencies.Add(currency);
            

                await repo.Context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = currency.CurrencyID });
            }

            return View(currency);
        }

        // GET: ManagementReference/Currencies/Update
        [AuthorizeRoles(Role.Dev, Role.MRProcurement)]
        public async Task<ActionResult> Edit(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }

            Currency currency = await repo.Context.Currencies.FindAsync(id);
            if (currency == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            return View(currency);
        }

        // POST: ManagementReference/Currencies/Update
        [HttpPost]
        [AuthorizeRoles(Role.Dev, Role.MRProcurement)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Currency currency)
        {
            if (ModelState.IsValid)
            {
                var preCurrency = await repo.Context.Currencies.FindAsync(currency.CurrencyID);
                preCurrency.CurrencyRateDefault = currency.CurrencyRateDefault;

                repo.Context.Entry(preCurrency).State = EntityState.Modified;
                await repo.Context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = currency.CurrencyID });
            }

            return View(currency);
        }

        // GET: ManagementReference/Currencies/Delete/5
        [AuthorizeRoles(Role.Dev, Role.MRProcurementAdmin)]
        public async Task<ActionResult> Delete(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            Currency currency = await repo.Context.Currencies.FindAsync(id);
            if (currency == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(currency);
        }

        // POST: ManagementReference/Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(Role.Dev, Role.MRProcurementAdmin)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Currency currency = await repo.Context.Currencies.FindAsync(id);

            repo.Context.Currencies.Remove(currency);
            await repo.Context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}