using LukeApps.AlertHandling;
using LukeApps.Authorization.Attributes;
using LukeApps.Authorization.RoleMap;
using LukeApps.EmployeeData;
using LukeApps.FileHandling;
using LukeApps.GeneralPurchase.DAL;
using LukeApps.GeneralPurchase.Models;
using LukeApps.GeneralPurchase.ViewModel;
using LukeApps.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace LukePurchaseSystem.Controllers
{
    [AuthorizeRoles(Role.Dev)]
    public class DashboardController : Controller
    {
        private GenericRepository<PurchaseEntities, PurchaseOrder> repo;

        public DashboardController() =>
            repo = new GenericRepository<PurchaseEntities, PurchaseOrder>(System.Web.HttpContext.Current.User.Identity.Name);

        // GET: ManagementReference/Dashboard
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<AlertNotification> alertLogs = GetAlertLogs.ForApp("ExpenseClaim").ForUser(User.GetUserData().Username).Serialize();

            var expiringAgreements = repo.GetAll().Include(a => a.Company).Where(a => a.PurchaseOrderExpiryDate != null).AsEnumerable().Where(a => DateTime.Now.AddDays(56).Date >= a.PurchaseOrderExpiryDate && !a.IsPurchaseOrderClosed && !a.IsPurchaseOrderCancelled).ToList();

            return View(new DashboardVM()
            {
                MRAlerts = alertLogs,
                TotalExpiringPurchaseOrders = expiringAgreements.Count,
                TotalExpiringInvoices = 0,
                ExpiringPurchaseOrders = expiringAgreements.ToList(),
            });
        }

        [HttpPost]
        [AuthorizeRoles(Role.Dev, Role.MRProcurementAdmin, Role.MRFinance)]
        public int DismissAlert(string key) => AlertOperations.ForKey(key).ByUser(User.Identity.Name.Substring(6)).Acknowledge();

        public JsonResult UploadFile()
        {
            return Json(new
            {
                Files = Filer.SetUsername(User.Identity.Name)
                .SetCurrentFolder("WipeOut")
                .AddFiles(Request.Files)
                .Upload()
                .GetFilesInformation(),
            }, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadFile(string key)
        {
            var file = Filer.SetFileKey(key).Download();
            return File(file.FileContent, file.ContentType, file.FileName);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}