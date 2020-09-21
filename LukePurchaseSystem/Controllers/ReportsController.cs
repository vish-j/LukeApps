using LukeApps.Authorization.Attributes;
using LukeApps.Authorization.RoleMap;
using LukeApps.CurrencyRates.DAL;
using LukeApps.CurrencyRates.Models;
using LukeApps.GeneralPurchase.DAL;
using LukeApps.GeneralPurchase.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace LukePurchaseSystem.Controllers
{
    [AuthorizeRoles(Role.Dev)]
    public class ReportsController : ExtendedController
    {
        private PurchaseEntities db = new PurchaseEntities(System.Web.HttpContext.Current.User.Identity.Name);
        private CurrencyEntities cr = new CurrencyEntities(System.Web.HttpContext.Current.User.Identity.Name);
        private List<Currency> _currencyList;

        private void setCurrencyList()
        {
            if (_currencyList == null) _currencyList = cr.Currencies.ToList();
        }

        // GET: ManagementReference/Reports
        public ActionResult Commitments()
        {
            return View();
        }

        public JsonResult GetCommitmentReport(DateTime? startDate, DateTime? endDate)
        {
            if (startDate != null && endDate != null)
            {
                var detailCollection = new List<CommitmentLineVM>() { };
                return Json(new
                {
                    iTotalRecords = detailCollection.Count,
                    iTotalDisplayRecords = detailCollection.Count,
                    aaData = detailCollection
                },
            JsonRequestBehavior.AllowGet);
            }
            else
            {
                var detailCollection = new List<CommitmentLineVM>() { };
                return Json(new
                {
                    iTotalRecords = detailCollection.Count,
                    iTotalDisplayRecords = detailCollection.Count,
                    aaData = detailCollection
                },
            JsonRequestBehavior.AllowGet);
            }
        }

        public string cleanText(string txt)
        {
            const int MaxLength = 100;

            if (txt.Length > MaxLength)
                return txt.Substring(0, MaxLength);
            return txt;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _currencyList = null;
                db.Dispose();
                cr.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}