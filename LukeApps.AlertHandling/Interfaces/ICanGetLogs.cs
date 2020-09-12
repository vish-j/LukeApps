using System.Collections.Generic;
using System.Threading.Tasks;

namespace LukeApps.AlertHandling.Interfaces
{
    public interface ICanGetLogs
    {
        ICanGetLogs WithCategory(string Category);

        ICanGetLogs ForInitiator(string user);

        ICanGetLogs ForUser(string user);

        ICanGetLogs WithArchivedLogs();

        IEnumerable<AlertNotification> Serialize();

        Task<IEnumerable<AlertNotification>> SerializeAsync();
    }
}