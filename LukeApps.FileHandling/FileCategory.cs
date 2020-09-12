using System.ComponentModel.DataAnnotations;

namespace LukeApps.FileHandling
{
    public class FileCategory
    {
        [Key]
        public long ID { get; set; }

        [Display(Name = "Unique Alias")]
        [RegularExpression("[^a-zA-Z0-9-]", ErrorMessage = "Please enter alpha numeric characters or '-'")]
        public string FileCategoryName { get; set; }

        [Display(Name = "Name")]
        public string FileCategoryDisplayName { get; set; }
    }
}