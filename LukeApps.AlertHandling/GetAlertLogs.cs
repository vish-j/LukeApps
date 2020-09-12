using LukeApps.AlertHandling.DAL;
using LukeApps.AlertHandling.Interfaces;
using LukeApps.AlertHandling.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LukeApps.AlertHandling
{
    public class GetAlertLogs : ICanGetLogs, IDisposable
    {
        private IQueryable<Alert> query;

        private readonly AlertEntities db = new AlertEntities();

        private bool isOnlyActiveAlerts = true;

        private GetAlertLogs(string App)
        {
            query = db.Alerts.Where(a => a.App == App);
        }

        public static ICanGetLogs ForApp(string App)
        {
            return new GetAlertLogs(App);
        }

        public ICanGetLogs WithCategory(string Category)
        {
            query = query.Where(a => a.Category == Category);
            return this;
        }

        public ICanGetLogs ForUser(string user)
        {
            query = query.Where(a => a.Target.TargetName == user && a.Target.TargetType == Enum.TargetType.Employees);
            return this;
        }

        public ICanGetLogs ForInitiator(string user)
        {
            query = query.Where(a => a.Initiator == user);
            return this;
        }

        public ICanGetLogs WithArchivedLogs()
        {
            isOnlyActiveAlerts = false;
            return this;
        }

        public IEnumerable<AlertNotification> Serialize()
        {
            IEnumerable<Alert> alertList = getAlertQuery().AsEnumerable();

            foreach (var a in alertList)
            {
                yield return new AlertNotification
                {
                    Key = a.AlertID.ToString(),
                    TimeStamp = string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", a.AlertDate),
                    Category = a.Category,
                    Message = a.Message,
                    Severity = a.Severity.ToString(),
                    URL = a.URL,
                    IsArchived = a.IsArchived,
                };
            }
        }

        public async Task<IEnumerable<AlertNotification>> SerializeAsync()
        {
            IEnumerable<Alert> alertList = await getAlertQuery().ToListAsync();
            List<AlertNotification> notifications = new List<AlertNotification>();
            foreach (var a in alertList)
            {
                notifications.Add(new AlertNotification
                {
                    Key = a.AlertID.ToString(),
                    TimeStamp = string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", a.AlertDate),
                    Category = a.Category,
                    Message = a.Message,
                    Severity = a.Severity.ToString(),
                    URL = a.URL,
                    IsArchived = a.IsArchived,
                });
            }
            return notifications;
        }

        private IQueryable<Alert> getAlertQuery()
        {
            if (isOnlyActiveAlerts)
            {
                query = query.Where(a => (a.IsAcknowledged == false && a.TimeOut > DateTime.Now));
            }
            return query.OrderByDescending(q => q.AlertDate);
        }

        #region Dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                }

                disposedValue = true;
                query = null;
            }
        }

        ~GetAlertLogs()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose
    }
}