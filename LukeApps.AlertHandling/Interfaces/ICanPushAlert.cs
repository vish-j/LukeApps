using System.Threading.Tasks;

namespace LukeApps.AlertHandling.Interfaces
{
    public interface ICanPushAlert
    {
        /// <summary>
        /// Set Severity of Alert to Information
        /// </summary>
        ICanPushAlert IsInformation();

        /// <summary>
        /// Set Severity of Alert to Warning
        /// </summary>
        ICanPushAlert IsWarning();

        /// <summary>
        /// Set Severity of Alert to Minor
        /// </summary>
        ICanPushAlert IsMinor();

        /// <summary>
        /// Set Severity of Alert to Major
        /// </summary>
        ICanPushAlert IsMajor();

        /// <summary>
        /// Set Severity of Alert to Critical
        /// </summary>
        ICanPushAlert IsCritical();

        /// <summary>
        /// Set the TTL for the Alert.
        /// TTL refers to Time for the alert to live.
        /// The value is calucated in hours.
        /// Default is 0, which means the life of the alert is infinite.
        /// </summary>
        ICanPushAlert WithTTL(double TTL);

        ICanPushAlert WithURL(string URL);

        ICanPushAlert WithMessage(string Message);

        int PushAlert();

        Task<int> PushAlertAsync();
    }
}