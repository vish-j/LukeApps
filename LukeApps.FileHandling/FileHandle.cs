using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace LukeApps.FileHandling
{
    public class FileHandle
    {
        public FileHandle()
        {
        }

        public FileHandle(string username, HttpFileCollectionBase fileCollection, bool isNoFolder = false)
        {
            var filer = Filer.SetUsername(username);

            if (isNoFolder)
                filer.SetCurrentFolder("WipeOut");

            FileKey = filer.AddFiles(fileCollection)
                            .Upload()
                            .GetKey();
        }

        public string FileKey { get; set; }

        [SkipTracking]
        [NotMapped]
        public bool IsFilePresent => FileKey != null;

        [SkipTracking]
        [NotMapped]
        public string FileName { get; set; }

        public FileDownload GetFile() => Filer.SetFileKey(FileKey).Download();

        public FileInformation GetFileInformation() => Filer.SetFileKey(FileKey).GetFileInformation();

        public void SetFileName() => FileName = GetFileInformation()?.FileName;

        public void Delete() => FileKey = null;

        public static MultipleFilesHandle operator +(FileHandle file1, FileHandle file2) => new MultipleFilesHandle() { FileKeys = string.Join(",", file1.FileKey, file2.FileKey) };

        /// <summary>
        /// Set File Category and move file to respective folder
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        /// <exception cref = "System.ArgumentNullException" ></exception>
        public void SetCategory(string category, string user) => Filer.SetFileKey(FileKey, user).SetFileCategory(category);
    }
}