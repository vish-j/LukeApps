using LukeApps.Authorization.Attributes;
using LukeApps.Authorization.RoleMap;
using LukeApps.Common.Helpers;
using LukeApps.GeneralPurchase.DAL;
using LukeApps.GeneralPurchase.Models;
using LukeApps.GenericRepository;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LukePurchaseSystem.Controllers
{
    public class BudgetsController : ExtendedController
    {
        private GenericRepository<PurchaseEntities, Budget> repo;

        public BudgetsController() =>
            repo = new GenericRepository<PurchaseEntities, Budget>(System.Web.HttpContext.Current.User.Identity.Name);

        public BudgetsController(string Username) =>
            repo = new GenericRepository<PurchaseEntities, Budget>(Username);

        // GET: Budgets
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
            var budgets = await repo.GetAll().ToListAsync();
            var detailCollection = budgets.Select(b => new
            {
                BudgetID = b.BudgetID,
                BudgetName = b.BudgetName,
                BudgetAmount = b.BudgetAmount,
                AuditDetail_CreatedDate = b.AuditDetail.CreatedDate.ToShortDateISO(),
                AuditDetail_CreatedEntryUser = b.AuditDetail.CreatedEntryUserDisplayName,
                AuditDetail_LastModifiedDate = b.AuditDetail.LastModifiedDate?.ToShortDateISO(),
                AuditDetail_LastModifiedEntryUser = b.AuditDetail.LastModifiedEntryUserDisplayName
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

        // GET: Budgets/Details/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = await repo.FindByAsync(b => b.BudgetID == id);

            if (budget == null)
            {
                return HttpNotFound();
            }
            return PartialView(budget);
        }

        // GET: Budgets/Create
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult Create()
        {
            return View(new Budget());
        }

        // POST: Budgets/Create

        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BudgetID,BudgetName,BudgetAmount,AuditDetail")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                repo.Add(budget);
                await repo.SaveChangesAsync();
                return RedirectToAction("Index", new { id = budget.BudgetID });
            }

            return View(budget);
        }

        // GET: Budgets/Edit/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Edit(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            Budget budget = await repo.FindByAsync(b => b.BudgetID == id);
            if (budget == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(budget);
        }

        // POST: Budgets/Edit/5

        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BudgetID,BudgetName,BudgetAmount,AuditDetail")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                var preBudget = repo.FindBy(b => b.BudgetID == budget.BudgetID);

                preBudget.BudgetName = budget.BudgetName;
                preBudget.BudgetAmount = budget.BudgetAmount;
                repo.Edit(preBudget);
                await repo.SaveChangesAsync();
                return RedirectToAction("Index", new { id = preBudget.BudgetID });
            }

            return View(budget);
        }

        // GET: Budgets/Delete/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Delete(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            Budget budget = await repo.FindByAsync(b => b.BudgetID == id);
            if (budget == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Budget budget = await repo.FindByAsync(b => b.BudgetID == id);

            repo.Delete(budget);
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