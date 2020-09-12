using LukeApps.FileHandling.Models;
using LukeApps.TrackingExtended;
using System.Data.Entity;
using TrackerEnabledDbContext.Common.Extensions;

namespace LukeApps.FileHandling.DAL
{
    internal partial class FileStoreEntities : ExtendedEntities
    {
        public FileStoreEntities(string username)
            : base(username, "name=FileStoreEntities")
        {
        }

        public virtual DbSet<FileRecord> FileStore { get; set; }
        public virtual DbSet<Folder> Folders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var FolderEntity = modelBuilder.Entity<Folder>();
            FolderEntity.HasOptional(s => s.ParentFolder);
            FolderEntity.TrackAllProperties();
            FolderEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);
            
            var FileRecordEntity = modelBuilder.Entity<FileRecord>();
            FileRecordEntity.TrackAllProperties();
            FileRecordEntity.Map(m => m.Requires("IsDeleted").HasValue(false))
                .Ignore(m => m.IsDeleted);
        }
    }
}