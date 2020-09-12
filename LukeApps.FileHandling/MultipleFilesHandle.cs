using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LukeApps.FileHandling
{
    public class MultipleFilesHandle
    {
        public MultipleFilesHandle()
        {
        }

        public MultipleFilesHandle(string username, HttpFileCollectionBase fileCollection, bool isNoFolder = false)
        {
            var filer = Filer.SetUsername(username);

            if (isNoFolder)
                filer.SetCurrentFolder("WipeOut");

            FileKeys = filer.AddFiles(fileCollection)
                            .Upload()
                            .GetKeys();
        }

        public string FileKeys { get; set; }

        [SkipTracking]
        [NotMapped]
        public bool IsAnyFilePresent => FileKeys != null;

        [SkipTracking]
        [NotMapped]
        public string FileNames { get; set; }

        public List<string> FileKeyList => FileKeys?.Split(',').Where(k => !string.IsNullOrEmpty(k)).ToList();

        public FileDownload GetZippedFiles(bool isUnZippedIfOneKey = false) => Filer.SetFileKeys(FileKeys).DownloadZipped(isUnZippedIfOneKey);

        public IEnumerable<FileDownload> GetMultipleFiles() => Filer.SetFileKeys(FileKeys).DownloadMultiple();

        public IEnumerable<FileInformation> GetFilesInformation() => Filer.SetFileKeys(FileKeys).GetFilesInformation();

        public static MultipleFilesHandle operator +(MultipleFilesHandle file1, MultipleFilesHandle file2)
        {
            List<string> keys = new List<string>();

            if (file1.IsAnyFilePresent)
                keys.AddRange(file1.FileKeyList);

            if (file2.IsAnyFilePresent)
                keys.AddRange(file2.FileKeyList);

            if (!keys.Any())
            {
                return new MultipleFilesHandle();
            }
            else
            {
                return new MultipleFilesHandle()
                {
                    FileKeys = string.Join(",", keys.Where(k => !string.IsNullOrEmpty(k)).ToArray())
                };
            }
        }

        public void SetFileNames()
        {
            if (IsAnyFilePresent)
                FileNames = string.Join(",", GetFilesInformation().Select(f => f.FileName));
        }
    }
}