using LukeApps.Authorization.Attributes;
using LukeApps.Authorization.RoleMap;
using LukeApps.Common.Helpers;
using LukeApps.GeneralPurchase.DAL;
using LukeApps.GeneralPurchase.Models;
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
    public class CompaniesController : ExtendedController
    {
        private GenericRepository<PurchaseEntities, Company> repo;

        public CompaniesController() =>
            repo = new GenericRepository<PurchaseEntities, Company>(System.Web.HttpContext.Current.User.Identity.Name);

        public CompaniesController(string Username) =>
            repo = new GenericRepository<PurchaseEntities, Company>(Username);

        // GET: Companies
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult Index(string ErrorMessage = null, long? id = null)
        {
            BuildViewBag(ErrorMessage, id);
            return View();
        }

        //fast json export to client
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<JsonResult> GetJSON()
        {
            var companies = await repo.GetAll().ToListAsync();
            var detailCollection = companies.Select(c => new
            {
                CompanyID = c.CompanyID,
                CompanyName = c.CompanyName,
                CompanyRegistration = c.CompanyRegistration,
                CommodityDescription = c.CommodityDescription,
                WebSite = c.WebSite,
                Address = c.Address,
                Comments = c.Comments,
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

        // GET: Companies/Details/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await repo.FindByAsync(c => c.CompanyID == id);

            if (company == null)
            {
                return HttpNotFound();
            }
            return PartialView(company);
        }

        // GET: Companies/Create
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult Create()
        {
            return View(new Company()
            {
                CompanyFocalPoints = new List<CompanyFocalPoint> {
                new CompanyFocalPoint() },
                BankAccounts = new List<BankAccount> { new BankAccount() }
            });
        }

        // POST: Companies/Create

        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Company cpy)
        {
            if (ModelState.IsValid)
            {
                repo.Add(cpy);
                await repo.SaveChangesAsync();
                return RedirectToAction("Index", new { id = cpy.CompanyID });
            }

            return View(cpy);
        }

        // GET: Companies/Edit/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Edit(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            Company company = await repo.FindByAsync(c => c.CompanyID == id);
            if (company == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(company);
        }

        // POST: Companies/Edit/5

        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Company cpy)
        {
            if (ModelState.IsValid)
            {
                var preCompany = repo.FindBy(c => c.CompanyID == cpy.CompanyID);

                preCompany.CompanyName = cpy.CompanyName;
                preCompany.CompanyRegistration = cpy.CompanyRegistration;
                preCompany.CommodityDescription = cpy.CommodityDescription;
                preCompany.WebSite = cpy.WebSite;
                preCompany.Address = cpy.Address;
                preCompany.Comments = cpy.Comments;
                preCompany.BlockStartDate = cpy.BlockStartDate;
                preCompany.BlockEndDate = cpy.BlockEndDate;
                preCompany.BlockBlacklistReason = cpy.BlockBlacklistReason;

                repo.Edit(preCompany);
                await repo.SaveChangesAsync();
                return RedirectToAction("Index", new { id = preCompany.CompanyID });
            }

            return View(cpy);
        }

        // GET: Companies/Delete/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Delete(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            Company company = await repo.FindByAsync(c => c.CompanyID == id);
            if (company == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Company company = await repo.FindByAsync(c => c.CompanyID == id);

            repo.Delete(company);
            await repo.SaveChangesAsync();

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