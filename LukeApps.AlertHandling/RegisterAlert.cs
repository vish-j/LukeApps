using LukeApps.AlertHandling.DAL;
using LukeApps.AlertHandling.Interfaces;
using LukeApps.AlertHandling.Models;
using System;
using System.Threading.Tasks;

namespace LukeApps.AlertHandling
{
    public class RegisterAlert : ICanSetCategory, ICanSetInitiator, ICanSetTarget, ICanPushAlert
    {
        private Alert _alert = new Alert();
        private double _TTL = 0;

        private RegisterAlert(string App)
        {
            _alert.TimeOut = DateTime.MaxValue;
            _alert.App = App;
        }

        public static ICanSetCategory ForApp(string App)
        {
            return new RegisterAlert(App);
        }

        public ICanSetInitiator WithCategory(string Category)
        {
            _alert.Category = Category;
            return this;
        }

        public ICanSetTarget ByUser(string User)
        {
            _alert.Initiator = User;
            return this;
        }

        public ICanPushAlert ToTargetApp()
        {
            _alert.Target.TargetType = Enum.TargetType.App;
            _alert.Target.TargetName = _alert.App;
            return this;
        }

        public ICanPushAlert ToTargetApp(string App)
        {
            _alert.Target.TargetType = Enum.TargetType.App;
            _alert.Target.TargetName = App;
            return this;
        }

        public ICanPushAlert ToTargetEmployees(string[] EmployeeNumbers)
        {
            _alert.Target.TargetType = Enum.TargetType.Employees;
            _alert.Target.TargetName = string.Join(",", EmployeeNumbers);
            return this;
        }

        public ICanPushAlert ToTargetGroups(string[] Groups)
        {
            _alert.Target.TargetType = Enum.TargetType.Groups;
            _alert.Target.TargetName = string.Join(",", Groups);
            return this;
        }

        public ICanPushAlert WithURL(string URL)
        {
            _alert.URL = URL;
            return this;
        }

        public ICanPushAlert WithMessage(string Message)
        {
            _alert.Message = Message;
            return this;
        }        

        public ICanPushAlert WithTTL(double TTL)
        {
            _TTL = TTL;
            return this;
        }

        public ICanPushAlert IsCritical()
        {
            _alert.Severity = Enum.Severity.Critical;
            return this;
        }

        public ICanPushAlert IsInformation()
        {
            _alert.Severity = Enum.Severity.Information;
            return this;
        }

        public ICanPushAlert IsMajor()
        {
            _alert.Severity = Enum.Severity.Major;
            return this;
        }

        public ICanPushAlert IsMinor()
        {
            _alert.Severity = Enum.Severity.Minor;
            return this;
        }

        public ICanPushAlert IsWarning()
        {
            _alert.Severity = Enum.Severity.Warning;
            return this;
        }

        public int PushAlert()
        {
            int status = 0;
            using (var db = new AlertEntities())
            {
                _alert.AlertDate = DateTime.Now;

                if (_TTL != 0)
                    _alert.TimeOut = _alert.AlertDate.AddHours(_TTL);

                db.Alerts.Add(_alert);
                status = db.SaveChanges();
            }
            return status;
        }

        public async Task<int> PushAlertAsync()
        {
            int status = 0;
            using (var db = new AlertEntities())
            {
                _alert.AlertDate = DateTime.Now;

                if (_TTL != 0)
                    _alert.TimeOut = _alert.AlertDate.AddHours(_TTL);

                db.Alerts.Add(_alert);
                status = await db.SaveChangesAsync();
            }
            return status;
        }
    }
}