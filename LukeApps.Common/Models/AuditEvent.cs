using LukeApps.Common.Helpers;
using LukeApps.EmployeeData;
using System;
using System.Collections.Generic;

namespace LukeApps.Common.Models
{
    public class AuditEvent
    {
        private DateTime _dateUTC;

        public AuditEvent(DateTime DateUTC)
        {
            _dateUTC = DateUTC;
            LocalDate = DateUTC.FormatUTCDate();
        }

        public string RecordID { get; set; }

        public DateTime DateUTC => _dateUTC;

        public string LocalDate { get; private set; }
        public string Type { get; set; }
        public string User { get; set; }

        public string Name => EmployeeProvider.GetEmployeeProvider().GetDisplayName(User);
        public List<KeyValuePair<string, string>> Details { get; set; }
    }
}