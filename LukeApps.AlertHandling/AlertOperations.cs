using LukeApps.AlertHandling.DAL;
using LukeApps.AlertHandling.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace LukeApps.AlertHandling
{
    public class AlertOperations : IAlertOperations, ICanGetUser, IDisposable
    {
        private readonly AlertEntities db = new AlertEntities();
        private Guid[] keys;
        private string user;

        private AlertOperations(string key)
        {
            keys = new Guid[] { new Guid(key) };
        }

        private AlertOperations(string[] keys)
        {
            this.keys = Array.ConvertAll(keys, delegate (string stringID) { Guid gid; return gid = new Guid(stringID); });
        }

        public static ICanGetUser ForKey(string key)
        {
            return new AlertOperations(key);
        }

        public static ICanGetUser ForKeys(string[] keys)
        {
            return new AlertOperations(keys);
        }

        public IAlertOperations ByUser(string User)
        {
            user = User;
            return this;
        }

        public int Acknowledge()
        {
            int status = 0;
            var alerts = db.Alerts.Where(a => keys.Contains(a.AlertID)).ToList();

            if (alerts.Any())
            {
                foreach (var alert in alerts)
                {
                    alert.IsAcknowledged = true;
                    alert.AcknowledgedBy = user;
                    db.Entry(alert).State = EntityState.Modified;
                }
                status = db.SaveChanges();
            }
            return status;
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                }

                keys = null;
                user = null;
                disposedValue = true;
            }
        }

        ~AlertOperations()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}