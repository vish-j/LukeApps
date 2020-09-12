namespace LukeApps.Common.Models
{
    public class RadioListItem
    {
        public string ID { get; set; }
        public string Display { get; set; }
        public string Group { get; set; }
        public bool IsChecked { get; set; }
        public bool IsReadOnly { get; set; } = false;
    }
}