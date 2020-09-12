using LukeApps.AlertHandling.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LukeApps.AlertHandling.Models
{
    internal class Alert
    {
        public Alert()
        {
            Target = new Target();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid AlertID { get; set; }

        public DateTime AlertDate { get; set; }

        public Target Target { get; set; }

        public Severity Severity { get; set; }
        public string App { get; set; }
        public string Category { get; set; }

        [Column(TypeName = "ntext")]
        public string Message { get; set; }

        public bool IsAcknowledged { get; set; }

        public string AcknowledgedBy { get; set; }
        public string URL { get; set; }

        public string Initiator { get; set; }

        public DateTime TimeOut { get; set; }

        public bool IsArchived => (TimeOut < DateTime.Now || IsAcknowledged);

        public override string ToString()
        {
            return string.Format("{0:dd/MM/yyyy} | {1}", AlertDate, Message);
        }
    }
}