using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeApps.AlertHandling.Interfaces
{
    public interface ICanGetUser
    {
        IAlertOperations ByUser(string User);
    }
}
