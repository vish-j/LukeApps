using System.Collections.Generic;
using System.Web;

namespace LukeApps.FileHandling.Interfaces
{
    public interface ICanGetSetFileCategory
    {
        List<FileCategory> GetFileCategories();

        ICanSaveChanges AddNewFileCategory(string FileCategory);
        ICanUpload AddFiles(HttpFileCollectionBase Files);
        ICanLog MoveFiles(string keys);

    }
}