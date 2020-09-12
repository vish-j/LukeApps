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

        //public JsonResult GetCommitmentReport(string range)
        //{
        //    if (range != null)
        //    {
        //        string[] Dates = range.Split(':');

        //        var startDate = DateTime.ParseExact(Dates[0].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //        var endDate = DateTime.ParseExact(Dates[1].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //        var agreements = db.PurchaseOrders.Include(a => a.Offer)
        //                                      .Include(a => a.Invoices)
        //                                      .Include("Offer.Enquiry")
        //                                      .Where(t => t.PurchaseOrderDate > startDate && t.PurchaseOrderDate < endDate);

        //        var detailCollection = agreements.AsEnumerable().Select(a => new PurchaseOrderVM(a)).GroupBy(a =>
        //        a.VendorID).Select(m => new
        //        {
        //            CompanyName = m.Key,
        //            PurchaseOrders = m.Count(),
        //            InvoiceReceivedTotal = m.Sum(s => s.InvoiceReceivedTotal),
        //            InvoiceApprovedTotal = m.Sum(s => s.InvoiceApprovedTotal),
        //            InvoicePaidTotal = m.Sum(s => s.InvoicePaidTotal),
        //            TotalValue = (m.Sum(s => s.TotalValue.Value)).ToString("n"),
        //            TotalBalance = (m.Sum(s => s.TotalBalance.Value)).ToString("n"),
        //            InvoiceTotalAmount = (m.Sum(s => s.InvoiceTotalAmount)).ToString("n"),
        //            InvoiceTotalAmountPaid = (m.Sum(s => s.InvoicePaidTotalAmount)).ToString("n")
        //        }).ToList();
        //        var TotalRecords = detailCollection.Count();
        //        return Json(new
        //        {
        //            iTotalRecords = TotalRecords,
        //            iTotalDisplayRecords = TotalRecords,
        //            aaData = detailCollection
        //        },
        //        JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            data = new List<string[]>() { new string[] { "Error" } },
        //            columns = new List<object>() { new { title = "Status" } },
        //            totalColumns = 1
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}

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