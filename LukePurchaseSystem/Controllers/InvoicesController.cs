using LukeApps.ApprovalProcess;
using LukeApps.Authorization.Attributes;
using LukeApps.Authorization.RoleMap;
using LukeApps.Common.Helpers;
using LukeApps.EmployeeData;
using LukeApps.GeneralPurchase.DAL;
using LukeApps.GeneralPurchase.Models;
using LukeApps.GenericRepository;
using PhilApprovalFlow;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LukePurchaseSystem.Controllers
{
    public class InvoicesController : ExtendedController
    {
        private GenericRepository<PurchaseEntities, Invoice> repo;

        public InvoicesController() =>
            repo = new GenericRepository<PurchaseEntities, Invoice>(System.Web.HttpContext.Current.User.Identity.Name);

        public InvoicesController(string Username) =>
            repo = new GenericRepository<PurchaseEntities, Invoice>(Username);

        // GET: Invoices
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
            var invoices = await repo.GetAll().Include(i => i.Budget).ToListAsync();

            var detailCollection = invoices.Select(i => new
            {
                InvoiceID = i.InvoiceID,
                BudgetID = i.BudgetID,
                OriginatorID = i.OriginatorID,
                ReviewerID = i.ReviewerID,
                ApproverID = i.ApproverID,
                Documents = i.Documents,
                AuditDetail_CreatedDate = i.AuditDetail.CreatedDate.ToShortDateISO(),
                AuditDetail_CreatedEntryUser = i.AuditDetail.CreatedEntryUserDisplayName,
                AuditDetail_LastModifiedDate = i.AuditDetail.LastModifiedDate?.ToShortDateISO(),
                AuditDetail_LastModifiedEntryUser = i.AuditDetail.LastModifiedEntryUserDisplayName
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

        // GET: Invoices/Details/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == id);

            if (invoice == null)
            {
                return HttpNotFound();
            }
            var currentUser = User.GetUserData();

            ViewBag.IsApprovedEnabled = invoice.Transitions.IsApprovedEnabled(currentUser.Username);

            ViewBag.IsRejectEnabled = invoice.Transitions.IsRejectEnabled(currentUser.Username);

            ViewBag.CanInvalidate = User.IsInRole(Role.Dev) || User.IsInRole(Role.SOFocalPoint);

            ViewBag.IsInTransitions = invoice.Transitions.Any(t => t.ApproverID == currentUser.Username);

            return PartialView(invoice);
        }

        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult AddNewInvoiceItem(long id)
        {
            return AddNewChild(new InvoiceItem(), r => r.InvoiceID, id);
        }

        private void viewBagBudgetID() => ViewBag.BudgetID = new SelectList(repo.Context.Budgets, "BudgetID", "BudgetName");
        public void viewBagApproveList() =>
            ViewBag.Employees = new SelectList(EmployeeProvider.GetEmployeeProvider().Users, nameof(Employee.Username), "displayName");


        // GET: Invoices/Create
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult Create()
        {
            viewBagApproveList();
            viewBagBudgetID();
            return View(new Invoice()
            {
                InvoiceItems = new List<InvoiceItem> {
                new InvoiceItem()
                }
            });
        }

        // POST: Invoices/Create

        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Invoice inv)
        {
            if (ModelState.IsValid)
            {
                repo.Add(inv);
                await repo.SaveChangesAsync();
                return RedirectToAction("Index", new { id = inv.InvoiceID });
            }

            viewBagBudgetID();
            viewBagApproveList();
            return View(inv);
        }

        // GET: Invoices/Edit/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Edit(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == id);
            if (invoice == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            viewBagBudgetID();
            viewBagApproveList();
            return View(invoice);
        }

        // POST: Invoices/Edit/5

        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Invoice inv)
        {
            if (ModelState.IsValid)
            {
                var preInvoice = repo.FindBy(i => i.InvoiceID == inv.InvoiceID);

                preInvoice.OriginatorID = inv.OriginatorID;
                preInvoice.ReviewerID = inv.ReviewerID;
                preInvoice.ApproverID = inv.ApproverID;
                preInvoice.Documents = inv.Documents;

                repo.Edit(preInvoice);
                repo.UpdateChildCollection(r => r.InvoiceItems, preInvoice, inv);
                await repo.SaveChangesAsync();

                return RedirectToAction("Index", new { id = preInvoice.InvoiceID });
            }
            viewBagBudgetID();
            viewBagApproveList();
            return View(inv);
        }

        // GET: Invoices/Delete/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Delete(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == id);
            if (invoice == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == id);

            repo.Delete(invoice);
            await repo.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        #region #ApprovalProccess

        [HttpGet]
        public async Task<ActionResult> Forward(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == id, nameof(Invoice.Transitions));
            if (invoice == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            ViewBag.ApproveList = new MultiSelectList(EmployeeProvider.GetEmployeeProvider().Users, nameof(Employee.Username), "displayName");
            return View(new RequestVM()
            {
                ID = invoice.InvoiceID,
                Display = invoice.InvoiceNumber,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Forward(RequestVM request)
        {
            if (ModelState.IsValid)
            {
                Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == request.ID, nameof(Invoice.Transitions));
                var employeeProvider = EmployeeProvider.GetEmployeeProvider();
                var approval = invoice.GetApprovalFlow()
                 .SetUserName(User.GetUserData());

                foreach (var approver in request.Approvers)
                {
                    approval.RequestApproval(employeeProvider.GetUserData(approver), request.Comments)
                        .LoadNotification(employeeProvider.GetUserData(approver), new List<Employee> { invoice.Originator });
                }

                repo.Edit(invoice);
                await repo.SaveChangesAsync();

                approval.FireNotifications();

                return RedirectToAction("Index", new { id = invoice.InvoiceID });
            }

            ViewBag.ApproveList = new MultiSelectList(EmployeeProvider.GetEmployeeProvider().Users, nameof(Employee.Username), "displayName", request.Approvers);
            return View(request);
        }


        [HttpGet]
        public async Task<ActionResult> Approve(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }

            Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == id, nameof(Invoice.Transitions));

            if (invoice == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(new DecisionVM()
            {
                ID = invoice.InvoiceID,
                Display = invoice.GetLongDescription(),
                RequesterComments = invoice.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve(DecisionVM decision)
        {
            Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == decision.ID, nameof(Invoice.Transitions));
            Employee currentemployee = User.GetUserData();

            var approval = invoice.GetApprovalFlow()
                .SetUserName(currentemployee);

            ProcessApprovals(invoice, currentemployee.Username, approval, decision.Comments);

            repo.Edit(invoice);
            await repo.SaveChangesAsync();

            approval.FireNotifications();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Reject(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }

            Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == id, nameof(Invoice.Transitions));

            if (invoice == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            return View(new DecisionVM()
            {
                ID = invoice.InvoiceID,
                Display = invoice.GetLongDescription(),
                RequesterComments = invoice.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reject(DecisionVM decision)
        {
            Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == decision.ID, nameof(Invoice.Transitions));

            var approval = invoice.GetApprovalFlow()
                .SetUserName(User.GetUserData())
                .Reject(decision.Comments)
                .LoadNotification(User.GetUserData(), new List<Employee> { invoice.Originator });

            repo.Edit(invoice);
            await repo.SaveChangesAsync();

            approval.FireNotifications();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Invalidate(long? id, string username)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }

            Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == id, nameof(Invoice.Transitions));

            if (invoice == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            return View(new DecisionVM()
            {
                ID = invoice.InvoiceID,
                Approver = username,
                Display = invoice.GetLongDescription(),
                RequesterComments = invoice.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        // POST: SU/Requisitions/Invalidate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invalidate(DecisionVM decision)
        {
            Invoice invoice = await repo.FindByAsync(i => i.InvoiceID == decision.ID, nameof(Invoice.Transitions));

            var approval = invoice.GetApprovalFlow()
                .SetUserName(User.GetUserData())
                .Invalidate(decision.ApproverEmployee, decision.Comments)
                .LoadNotification(decision.ApproverEmployee, new List<Employee> { invoice.Originator });

            repo.Edit(invoice);
            await repo.SaveChangesAsync();

            approval.FireNotifications();

            return RedirectToAction("Index", new { id = decision.ID });
        }

        #endregion #ApprovalProccess


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