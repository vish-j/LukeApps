using LukeApps.Common.Helpers;
using LukeApps.Common.Models;
using LukeApps.EmployeeData;
using LukeApps.FileHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TrackerEnabledDbContext.Common.Interfaces;

namespace LukePurchaseSystem.Controllers
{
    public abstract class AuditBaseController : Controller
    {
        protected List<AuditEvent> Log = new List<AuditEvent>();
        protected ITrackerContext Db;
        private EmployeeProvider empProvider = EmployeeProvider.GetEmployeeProvider();

        // GET: xx/AuditTrail
        public ActionResult Trail(string returnUrl)
        {
            var url = new Uri(returnUrl);
            var q = HttpUtility.ParseQueryString(url.Query);
            string type = url.AbsolutePath.Split('/').Where(x => !string.IsNullOrEmpty(x)).LastOrDefault();

            long id = long.TryParse(q["id"], out id) ? id : -1;

            if (modelID.check(id) || type == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.ReturnUrl = returnUrl;

            SetLog(type, id);

            if (Log == null)
                return HttpNotFound();

            return View("~/Views/Audit/Trail.cshtml", Log.Where(o => o.Details.Count != 0).OrderBy(o => o.DateUTC).ToList());
        }

        internal abstract void SetLog(string type, long? id);

        public FileResult Download(string key)
        {
            var file = Filer.SetFileKey(key).Download();
            return File(file.FileContent, file.ContentType, file.FileName);
        }

        protected List<AuditEvent> GetTrail<TEntity>(object id) => Db.GetLogs<TEntity>(id)
                 .AsEnumerable()
                 .Select(e => new AuditEvent(e.EventDateUTC)
                 {
                     RecordID = id.ToString(),
                     User = e.UserName,
                     Type = cleanPropertyname(typeof(TEntity).Name),
                     Details = e.LogDetails.Where(d => !d.PropertyName.Contains("AuditDetail"))
                 .Select(d => new KeyValuePair<string, string>(cleanPropertyname(d.PropertyName), checkAndResloveName(d.NewValue)))
                 .ToList()
                 }).ToList();

        private string checkAndResloveName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return (Regex.IsMatch(value, "([5][1|2][0-9]{6})")) ? empProvider.GetDisplayName(value) : value;
        }

        private string cleanPropertyname(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.Replace('_', ' ').AddSpacesToSentence(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
                if (Log != null)
                    Log.Clear();
            }
            base.Dispose(disposing);
        }
    }
}