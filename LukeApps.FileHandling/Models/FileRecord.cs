using LukeApps.TrackingExtended;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LukeApps.FileHandling.Models
{
    internal class FileRecord : IAuditDetail
    {
        public FileRecord()
        {
            this.AuditDetail = new AuditDetail();
        }

        public FileRecord(string username)
        {
            this.AuditDetail = new AuditDetail() { CreatedEntryUserID = username };
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid FileRecordID { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public long ContentLength { get; set; }
        public long FolderID { get; set; }
        public int SerialNumber { get; set; }

        public virtual Folder Folder { get; set; }
        public AuditDetail AuditDetail { get; set; }

        public bool IsDeleted { get; set; }
    }
}