using System.IO;
using System.Web;

namespace LukeApps.FileHandling.Interfaces
{
    public interface ICanAddFilesOrFileCategories
    {
        ICanUpload AddFiles(HttpFileCollectionBase webFiles);
        ICanUpload AddFile(string fileName, Stream inputStream, string fileCategoryKey, string contentType);
        ICanGetSetFileCategory SetCurrentFolder(long FolderID);
        ICanGetSetFileCategory SetCurrentFolder(string FolderName);

    }
}