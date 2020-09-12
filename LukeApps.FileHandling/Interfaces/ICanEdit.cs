using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeApps.FileHandling.Interfaces
{
    public interface ICanEdit
    {
        /// <summary>
        /// Set File Category and move file to respective folder
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        /// <exception cref = "System.ArgumentNullException" ></exception>
        bool SetFileCategory(string category);
    }
}
