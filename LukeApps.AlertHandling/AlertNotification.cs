namespace LukeApps.AlertHandling
{
    public class AlertNotification
    {
        public string Key { get; set; }
        public string Category { get; set; }
        public string TimeStamp { get; set; }
        public string Message { get; set; }
        public string URL { get; set; }
        public string Severity { get; set; }
        public bool IsArchived { get; set; }
    }
}