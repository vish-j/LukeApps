using System.Collections.Generic;

namespace LukeApps.FileHandling.Interfaces
{
    public interface ICanDownload
    {
        FileDownload Download();

        FileInformation GetFileInformation();
        ICanSaveChanges RenameFile(string newname);
    }

    public interface ICanDownloadMultiple
    {
        IEnumerable<FileDownload> DownloadMultiple();

        FileDownload DownloadZipped(bool isUnZippedIfOneKey = false);

        IEnumerable<FileInformation> GetFilesInformation();
    }
}