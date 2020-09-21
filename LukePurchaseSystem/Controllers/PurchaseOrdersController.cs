using LukeApps.ApprovalProcess;
using LukeApps.Authorization.Attributes;
using LukeApps.Authorization.RoleMap;
using LukeApps.Common.Helpers;
using LukeApps.EmployeeData;
using LukeApps.GeneralPurchase.DAL;
using LukeApps.GeneralPurchase.Models;
using LukeApps.GeneralPurchase.ViewModel;
using LukeApps.GenericRepository;
using PhilApprovalFlow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LukePurchaseSystem.Controllers
{
    public class PurchaseOrdersController : ExtendedController
    {
        private GenericRepository<PurchaseEntities, PurchaseOrder> repo;

        public PurchaseOrdersController() =>
            repo = new GenericRepository<PurchaseEntities, PurchaseOrder>(System.Web.HttpContext.Current.User.Identity.Name);

        public PurchaseOrdersController(string Username) =>
            repo = new GenericRepository<PurchaseEntities, PurchaseOrder>(Username);

        // GET: PurchaseOrders
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
            var purchaseOrders = await repo.GetAll().Include(p => p.Budget).ToListAsync();

            var detailCollection = purchaseOrders.Select(p => new
            {
                PurchaseOrderID = p.PurchaseOrderID,
                BudgetID = p.BudgetID,
                OriginatorID = p.OriginatorID,
                ReviewerID = p.ReviewerID,
                ApproverID = p.ApproverID,
                ApprovedDocuments = p.ApprovedDocuments,
                IsPurchaseOrderClosed = (p.IsPurchaseOrderCancelled ? "Yes" : "No"),
                PurchaseOrderExpiryDate = p.PurchaseOrderExpiryDate?.ToShortDateISO(),
                AuditDetail_CreatedDate = p.AuditDetail.CreatedDate.ToShortDateISO(),
                AuditDetail_CreatedEntryUser = p.AuditDetail.CreatedEntryUserDisplayName,
                AuditDetail_LastModifiedDate = p.AuditDetail.LastModifiedDate?.ToShortDateISO(),
                AuditDetail_LastModifiedEntryUser = p.AuditDetail.LastModifiedEntryUserDisplayName
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

        [AuthorizeRoles(Role.Dev)]
        public async Task<JsonResult> GetPurchaseOrder(long? id)
        {
            PurchaseOrder purchaseOrder = await repo.FindByAsync(p => p.PurchaseOrderID == id);
            return Json(purchaseOrder,
            JsonRequestBehavior.AllowGet);
        }

        // GET: PurchaseOrders/Details/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = await repo.FindByAsync(p => p.PurchaseOrderID == id);

            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }

            var currentUser = User.GetUserData();

            ViewBag.IsApprovedEnabled = purchaseOrder.Transitions.IsApprovedEnabled(currentUser.Username);

            ViewBag.IsRejectEnabled = purchaseOrder.Transitions.IsRejectEnabled(currentUser.Username);

            ViewBag.CanInvalidate = User.IsInRole(Role.Dev) || User.IsInRole(Role.SOFocalPoint);

            ViewBag.IsInTransitions = purchaseOrder.Transitions.Any(t => t.ApproverID == currentUser.Username);

            return PartialView(purchaseOrder);
        }

        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult AddNewPurchaseOrderItem(long id)
        {
            return AddNewChild(new PurchaseOrderItem(), r => r.PurchaseOrderID, id);
        }
        private void viewBagCompanyID() => ViewBag.CompanyID = new SelectList(repo.Context.Companies, "CompanyID", "CompanyName");
        private void viewBagBudgetID() => ViewBag.BudgetID = new SelectList(repo.Context.Budgets, "BudgetID", "BudgetName");

        public void viewBagApproveList() =>
            ViewBag.Employees = new SelectList(EmployeeProvider.GetEmployeeProvider().Users, nameof(Employee.Username), "displayName");

        // GET: PurchaseOrders/Create
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult Create()
        {
            viewBagBudgetID();
            viewBagApproveList();
            viewBagCompanyID();
            return View(new PurchaseOrder()
            {
                PurchaseOrderItems = new List<PurchaseOrderItem> {
                new PurchaseOrderItem()
                }
            });
        }

        // POST: PurchaseOrders/Create

        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PurchaseOrder po)
        {
            if (ModelState.IsValid)
            {
                repo.Add(po);
                await repo.SaveChangesAsync();
                return RedirectToAction("Index", new { id = po.PurchaseOrderID });
            }

            viewBagBudgetID();
            viewBagApproveList();
            viewBagCompanyID();
            return View(po);
        }

        // GET: PurchaseOrders/Edit/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Edit(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            PurchaseOrder purchaseOrder = await repo.FindByAsync(p => p.PurchaseOrderID == id);
            if (purchaseOrder == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            viewBagBudgetID();
            viewBagApproveList();
            viewBagCompanyID();
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Edit/5

        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PurchaseOrder po)
        {
            if (ModelState.IsValid)
            {
                var prePurchaseOrder = repo.FindBy(p => p.PurchaseOrderID == po.PurchaseOrderID);

                prePurchaseOrder.OriginatorID = po.OriginatorID;
                prePurchaseOrder.ReviewerID = po.ReviewerID;
                prePurchaseOrder.ApproverID = po.ApproverID;
                prePurchaseOrder.ApprovedDocuments = po.ApprovedDocuments;
                if (prePurchaseOrder.IsPurchaseOrderCancelled)
                {
                    prePurchaseOrder.CancelDate = DateTime.Now;
                }
                prePurchaseOrder.PurchaseOrderExpiryDate = po.PurchaseOrderExpiryDate;

                repo.Edit(prePurchaseOrder);
                repo.UpdateChildCollection(r => r.PurchaseOrderItems, prePurchaseOrder, po);
                await repo.SaveChangesAsync();
                return RedirectToAction("Index", new { id = prePurchaseOrder.PurchaseOrderID });
            }
            viewBagBudgetID();
            viewBagApproveList();
            viewBagCompanyID();
            return View(po);
        }

        // GET: PurchaseOrders/Delete/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Delete(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            PurchaseOrder purchaseOrder = await repo.FindByAsync(p => p.PurchaseOrderID == id);
            if (purchaseOrder == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            PurchaseOrder purchaseOrder = await repo.FindByAsync(p => p.PurchaseOrderID == id);

            repo.Delete(purchaseOrder);
            await repo.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: SmallOrders/Agreements/GetPurchaseOrder/5
        public JsonResult GetPurchaseOrder(long id)
        {
            PurchaseOrder agreement = repo.Context.PurchaseOrders.Include(p => p.Transitions).FirstOrDefault(f => f.PurchaseOrderID == id);
            return Json(new PurchaseOrderVM(agreement),
            JsonRequestBehavior.AllowGet);
        }

        #region #ApprovalProccess

        [HttpGet]
        public async Task<ActionResult> Forward(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            PurchaseOrder purchaseOrder = await repo.FindByAsync(e => e.PurchaseOrderID == id, nameof(PurchaseOrder.Transitions));
            if (purchaseOrder == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            ViewBag.ApproveList = new MultiSelectList(EmployeeProvider.GetEmployeeProvider().Users, nameof(Employee.Username), "displayName");
            return View(new RequestVM()
            {
                ID = purchaseOrder.PurchaseOrderID,
                Display = purchaseOrder.PurchaseOrderNumber,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Forward(RequestVM request)
        {
            if (ModelState.IsValid)
            {
                PurchaseOrder purchaseOrder = await repo.FindByAsync(e => e.PurchaseOrderID == request.ID, nameof(PurchaseOrder.Transitions));
                var employeeProvider = EmployeeProvider.GetEmployeeProvider();
                var approval = purchaseOrder.GetApprovalFlow()
                 .SetUserName(User.GetUserData());

                foreach (var approver in request.Approvers)
                {
                    approval.RequestApproval(employeeProvider.GetUserData(approver), request.Comments)
                        .LoadNotification(employeeProvider.GetUserData(approver), new List<Employee> { purchaseOrder.Originator });
                }

                repo.Edit(purchaseOrder);
                await repo.SaveChangesAsync();

                approval.FireNotifications();

                return RedirectToAction("Index", new { id = purchaseOrder.PurchaseOrderID });
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
            PurchaseOrder purchaseOrder = await repo.FindByAsync(e => e.PurchaseOrderID == id, nameof(PurchaseOrder.Transitions));
            if (purchaseOrder == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(new DecisionVM()
            {
                ID = purchaseOrder.PurchaseOrderID,
                Display = purchaseOrder.GetLongDescription(),
                RequesterComments = purchaseOrder.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve(DecisionVM decision)
        {
            PurchaseOrder purchaseOrder = await repo.FindByAsync(e => e.PurchaseOrderID == decision.ID, nameof(PurchaseOrder.Transitions));
            Employee currentemployee = User.GetUserData();

            var approval = purchaseOrder.GetApprovalFlow()
                .SetUserName(currentemployee);

            ProcessApprovals(purchaseOrder, currentemployee.Username, approval);

            repo.Edit(purchaseOrder);
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

            PurchaseOrder purchaseOrder = await repo.FindByAsync(e => e.PurchaseOrderID == id, nameof(PurchaseOrder.Transitions));

            if (purchaseOrder == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            return View(new DecisionVM()
            {
                ID = purchaseOrder.PurchaseOrderID,
                Display = purchaseOrder.GetLongDescription(),
                RequesterComments = purchaseOrder.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reject(DecisionVM decision)
        {
            PurchaseOrder purchaseOrder = await repo.FindByAsync(e => e.PurchaseOrderID == decision.ID, nameof(PurchaseOrder.Transitions));

            var approval = purchaseOrder.GetApprovalFlow()
                .SetUserName(User.GetUserData())
                .Reject(decision.Comments)
                .LoadNotification(User.GetUserData(), new List<Employee> { purchaseOrder.Originator });

            repo.Edit(purchaseOrder);
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

            PurchaseOrder purchaseOrder = await repo.FindByAsync(e => e.PurchaseOrderID == id, nameof(PurchaseOrder.Transitions));

            if (purchaseOrder == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            return View(new DecisionVM()
            {
                ID = purchaseOrder.PurchaseOrderID,
                Approver = username,
                Display = purchaseOrder.GetLongDescription(),
                RequesterComments = purchaseOrder.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        // POST: SU/Requisitions/Invalidate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invalidate(DecisionVM decision)
        {
            PurchaseOrder purchaseOrder = await repo.FindByAsync(e => e.PurchaseOrderID == decision.ID, nameof(PurchaseOrder.Transitions));

            var approval = purchaseOrder.GetApprovalFlow()
                .SetUserName(User.GetUserData())
                .Invalidate(decision.ApproverEmployee, decision.Comments)
                .LoadNotification(decision.ApproverEmployee, new List<Employee> { purchaseOrder.Originator });

            repo.Edit(purchaseOrder);
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