namespace LukeApps.FileHandling
{
    public class FileInformation
    {
        public FileInformation()
        {
            FileCategory = new FileCategory();
        }

        public string FileCode { get; set; }
        public string FileName { get; set; }

        /// <summary>
        /// Size (Kilobytes)
        /// </summary>
        public float Size { get; set; }

        public string FileType { get; set; }

        public FileCategory FileCategory { get; set; }
    }
}