using LukeApps.ApprovalProcess;
using LukeApps.Authorization.Attributes;
using LukeApps.Authorization.RoleMap;
using LukeApps.Common.Helpers;
using LukeApps.EmployeeData;
using LukeApps.FileHandling;
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
    public class ExpenseClaimsController : ExtendedController
    {
        private GenericRepository<PurchaseEntities, ExpenseClaim> repo;

        public ExpenseClaimsController() =>
            repo = new GenericRepository<PurchaseEntities, ExpenseClaim>(System.Web.HttpContext.Current.User.Identity.Name);

        public ExpenseClaimsController(string Username) =>
            repo = new GenericRepository<PurchaseEntities, ExpenseClaim>(Username);

        // GET: ExpenseClaims
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
            var expenseClaims = await repo.GetAll(nameof(ExpenseClaim.Transitions), nameof(ExpenseClaim.ExpenseClaimItems)).ToListAsync();
            var detailCollection = expenseClaims.Select(e => new
            {
                ExpenseClaimID = e.ExpenseClaimID,
                ExpenseClaimNumber = e.ExpenseClaimNumber,
                RequestDate = e.RequestDate.ToShortDateISO(),
                Budget = e.Budget.BudgetName,
                OriginatorID = e.Originator.DisplayName,
                ReviewerID = e.Reviewer.DisplayName,
                ApproverID = e.Approver.DisplayName,
                PaymentMethod = e.PaymentMethod.GetDisplay(),
                Status = e.Transitions.Any() ? "Started" : "Not Started",
                Total = e.Total.ToString(),
                AuditDetail_CreatedDate = e.AuditDetail.CreatedDate.ToShortDateISO(),
                AuditDetail_CreatedEntryUser = e.AuditDetail.CreatedEntryUserDisplayName,
                AuditDetail_LastModifiedDate = e.AuditDetail.LastModifiedDate?.ToShortDateISO(),
                AuditDetail_LastModifiedEntryUser = e.AuditDetail.LastModifiedEntryUserDisplayName
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
        public async Task<JsonResult> GetExpenseClaim(long? id)
        {
            ExpenseClaim expenseClaim = await repo.FindByAsync(p => p.ExpenseClaimID == id, nameof(ExpenseClaim.ExpenseClaimItems), nameof(ExpenseClaim.Transitions));
            return Json(new {
                expenseClaim.ExpenseClaimNumber,
                RequestDate = expenseClaim.RequestDate.ToShortDateLocal(),
                expenseClaim.BankAccountNumber,
                PaymentMethod = expenseClaim.PaymentMethod.GetDisplay(),
                Originator = expenseClaim.Originator.Summary,
                OriginatorJobTitle = expenseClaim.Originator.JobTitle,
                Approver = expenseClaim.Approver.Summary,
                ApproverJobTitle = expenseClaim.Approver.JobTitle,
                OriginatorES = expenseClaim.Transitions.Any(t => t.ApproverID == expenseClaim.OriginatorID && t.ApproverDecision == PhilApprovalFlow.Enum.DecisionType.Approved) ? expenseClaim.Originator.ElectronicSignature : null,
                ApproverES = expenseClaim.Transitions.Any(t => t.ApproverID == expenseClaim.ApproverID && t.ApproverDecision == PhilApprovalFlow.Enum.DecisionType.Approved) ? expenseClaim.Originator.ElectronicSignature : null,
                LineItems = expenseClaim.ExpenseClaimItems.Select(e => new { 
                 e.Quantity,
                 e.Description,
                    UnitPrice = e.UnitPrice.ToString(),
                    TotalPrice = e.TotalPrice.ToString()
                })
            },
            JsonRequestBehavior.AllowGet);
        }

        // GET: ExpenseClaims/Details/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == id, nameof(ExpenseClaim.Transitions));

            if (expenseClaim == null)
            {
                return HttpNotFound();
            }

            var currentUser = User.GetUserData();

            ViewBag.IsApprovedEnabled = expenseClaim.Transitions.IsApprovedEnabled(currentUser.Username);

            ViewBag.IsRejectEnabled = expenseClaim.Transitions.IsRejectEnabled(currentUser.Username);

            ViewBag.CanInvalidate = User.IsInRole(Role.Dev) || User.IsInRole(Role.SOFocalPoint);

            ViewBag.IsInTransitions = expenseClaim.Transitions.Any(t => t.ApproverID == currentUser.Username);

            return PartialView(expenseClaim);
        }

        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult AddNewExpenseClaimItem(long id)
        {
            return AddNewChild(new ExpenseClaimItem(), r => r.ExpenseClaimID, id);
        }

        private void viewBagBudgetID() => ViewBag.BudgetID = new SelectList(repo.Context.Budgets, "BudgetID", "BudgetName");

        public void viewBagApproveList() =>
            ViewBag.Employees = new SelectList(EmployeeProvider.GetEmployeeProvider().Users, nameof(Employee.Username), "displayName");

        // GET: ExpenseClaims/Create
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public ActionResult Create()
        {
            viewBagApproveList();
            viewBagBudgetID();
            return View(new ExpenseClaim()
            {
                OriginatorID = User.Identity.Name,
                ExpenseClaimItems = new List<ExpenseClaimItem> {
                    new ExpenseClaimItem()
                }
            });
        }

        // POST: ExpenseClaims/Create

        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExpenseClaim expense)
        {
            if (ModelState.IsValid)
            {
                Employee currentemployee = User.GetUserData();

                var approval = expense.GetApprovalFlow().SetUserName(currentemployee.Username);

                ProcessApprovals(expense, currentemployee.Username, approval);

                repo.Add(expense);
                await repo.SaveChangesAsync();

                approval.FireNotifications();
                return RedirectToAction("Index", new { id = expense.ExpenseClaimID });
            }
            viewBagApproveList();
            viewBagBudgetID();
            return View(expense);
        }

        // GET: ExpenseClaims/Edit/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Edit(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == id);
            if (expenseClaim == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            viewBagApproveList();
            viewBagBudgetID();
            expenseClaim.SupportingDocuments.SetFileNames();
            return View(expenseClaim);
        }

        // POST: ExpenseClaims/Edit/5
        [HttpPost]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ExpenseClaim expense)
        {
            if (ModelState.IsValid)
            {
                var preExpenseClaim = repo.FindBy(e => e.ExpenseClaimID == expense.ExpenseClaimID);

                preExpenseClaim.RequestDate = expense.RequestDate;
                preExpenseClaim.BankAccountNumber = expense.BankAccountNumber;
                preExpenseClaim.OriginatorID = expense.OriginatorID;
                preExpenseClaim.ReviewerID = expense.ReviewerID;
                preExpenseClaim.ApproverID = expense.ApproverID;
                preExpenseClaim.PaymentMethod = expense.PaymentMethod;
                preExpenseClaim.SupportingDocuments = expense.SupportingDocuments;

                repo.Edit(preExpenseClaim);
                repo.UpdateChildCollection(r => r.ExpenseClaimItems, preExpenseClaim, expense);

                await repo.SaveChangesAsync();
                return RedirectToAction("Index", new { id = preExpenseClaim.ExpenseClaimID });
            }
            viewBagApproveList();
            viewBagBudgetID();
            return View(expense);
        }

        // GET: ExpenseClaims/Delete/5
        [HttpGet]
        [AuthorizeRoles(Role.Dev)]
        public async Task<ActionResult> Delete(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == id);
            if (expenseClaim == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(expenseClaim);
        }

        // POST: ExpenseClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeRoles(Role.Dev)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == id);

            repo.Delete(expenseClaim);
            await repo.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<FileResult> DownloadZipped(long? id)
        {
            if (modelID.check(id))
            {
                return File(Filer.InvalidDefaultFile.FileContent, System.Net.Mime.MediaTypeNames.Application.Octet, Filer.InvalidDefaultFile.FileName);
            }
            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == id);
            if (expenseClaim == null)
            {
                return File(Filer.InvalidDefaultFile.FileContent, System.Net.Mime.MediaTypeNames.Application.Octet, Filer.InvalidDefaultFile.FileName);
            }

            var file = expenseClaim.SupportingDocuments.GetZippedFiles(isUnZippedIfOneKey: true);
            return File(file.FileContent, System.Net.Mime.MediaTypeNames.Application.Octet, file.FileName);
        }

        #region #ApprovalProccess

        [HttpGet]
        public async Task<ActionResult> Forward(long? id)
        {
            if (modelID.check(id))
            {
                return RedirectToAction("Index", new { ErrorMessage = "ID Missing" });
            }
            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == id, nameof(ExpenseClaim.Transitions));
            if (expenseClaim == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            ViewBag.ApproveList = new MultiSelectList(EmployeeProvider.GetEmployeeProvider().Users, nameof(Employee.Username), "displayName");
            return View(new RequestVM()
            {
                ID = expenseClaim.ExpenseClaimID,
                Display = expenseClaim.ExpenseClaimNumber,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Forward(RequestVM request)
        {
            if (ModelState.IsValid)
            {
                ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == request.ID, nameof(ExpenseClaim.Transitions));
                var employeeProvider = EmployeeProvider.GetEmployeeProvider();
                var approval = expenseClaim.GetApprovalFlow()
                 .SetUserName(User.GetUserData());

                foreach (var approver in request.Approvers)
                {
                    approval.RequestApproval(employeeProvider.GetUserData(approver), request.Comments)
                        .LoadNotification(employeeProvider.GetUserData(approver), new List<Employee> { expenseClaim.Originator });
                }

                repo.Edit(expenseClaim);
                await repo.SaveChangesAsync();

                approval.FireNotifications();

                return RedirectToAction("Index", new { id = expenseClaim.ExpenseClaimID });
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
            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == id, nameof(ExpenseClaim.Transitions));
            if (expenseClaim == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }
            return View(new DecisionVM()
            {
                ID = expenseClaim.ExpenseClaimID,
                Display = expenseClaim.GetLongDescription(),
                RequesterComments = expenseClaim.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve(DecisionVM decision)
        {
            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == decision.ID, nameof(ExpenseClaim.Transitions));
            Employee currentemployee = User.GetUserData();

            var approval = expenseClaim.GetApprovalFlow()
                .SetUserName(currentemployee);

            ProcessApprovals(expenseClaim, currentemployee.Username, approval, decision.Comments);

            repo.Edit(expenseClaim);
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

            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == id, nameof(ExpenseClaim.Transitions));

            if (expenseClaim == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            return View(new DecisionVM()
            {
                ID = expenseClaim.ExpenseClaimID,
                Display = expenseClaim.GetLongDescription(),
                RequesterComments = expenseClaim.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reject(DecisionVM decision)
        {
            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == decision.ID, nameof(ExpenseClaim.Transitions));

            var approval = expenseClaim.GetApprovalFlow()
                .SetUserName(User.GetUserData())
                .Reject(decision.Comments)
                .LoadNotification(User.GetUserData(), new List<Employee> { expenseClaim.Originator });

            repo.Edit(expenseClaim);
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

            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == id, nameof(ExpenseClaim.Transitions));

            if (expenseClaim == null)
            {
                return RedirectToAction("Index", new { ErrorMessage = "Bad ID" });
            }

            return View(new DecisionVM()
            {
                ID = expenseClaim.ExpenseClaimID,
                Approver = username,
                Display = expenseClaim.GetLongDescription(),
                RequesterComments = expenseClaim.Transitions.Where(t => t.ApproverID == User.GetUserData().Username).FirstOrDefault()?.ApproverComments
            });
        }

        // POST: SU/Requisitions/Invalidate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invalidate(DecisionVM decision)
        {
            ExpenseClaim expenseClaim = await repo.FindByAsync(e => e.ExpenseClaimID == decision.ID, nameof(ExpenseClaim.Transitions));

            var approval = expenseClaim.GetApprovalFlow()
                .SetUserName(User.GetUserData())
                .Invalidate(decision.ApproverEmployee, decision.Comments)
                .LoadNotification(decision.ApproverEmployee, new List<Employee> { expenseClaim.Originator });

            repo.Edit(expenseClaim);
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