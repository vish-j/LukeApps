using System;
using System.Collections.Generic;

namespace LukeApps.FileHandling.Interfaces
{
    public interface ICanLog
    {
        string GetKey();

        string GetKeys();

        [Obsolete("Use GetFilesInformation() Instead")]
        IEnumerable<KeyValuePair<Guid, string>> GetKeysWithCategory();

        IEnumerable<FileInformation> GetFilesInformation();
    }
}