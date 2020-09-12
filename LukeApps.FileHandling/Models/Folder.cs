using LukeApps.TrackingExtended;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.FileHandling.Models
{
    internal class Folder : IAuditDetail
    {
        public Folder()
        {
            FileRecords = new HashSet<FileRecord>();
        }

        [Key]
        public long FolderID { get; set; }

        public string FolderName { get; set; }

        public string FolderDisplayName { get; set; }
        public virtual Folder ParentFolder { get; set; }
        public AuditDetail AuditDetail { get; set; }
        public bool IsDeleted { get; set; }

        public string FullPath
        {
            get
            {
                if (_fullPath == "")
                    setfullpath(this);
                return _fullPath;
            }
        }

        private string _fullPath = "";

        private void setfullpath(Folder folder)
        {
            _fullPath = folder.FolderName + @"\" + _fullPath;
            if (folder.ParentFolder != null)
            {
                setfullpath(folder.ParentFolder);
            }
            else
            {
                _fullPath = @"\" + _fullPath;
            }
        }

        public virtual ICollection<FileRecord> FileRecords { get; set; }
    }
}