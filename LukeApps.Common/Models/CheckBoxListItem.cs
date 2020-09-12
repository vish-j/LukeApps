namespace LukeApps.Common.Models
{
    public class CheckBoxListItem
    {
        public string ID { get; set; }
        public string Display { get; set; }
        public bool IsChecked { get; set; }
        public bool IsReadOnly { get; set; } = false;
    }
}