namespace LukeApps.Common.Models
{
    public class AuditLogHistory
    {
        public string PropertyName { get; set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }
    }
}