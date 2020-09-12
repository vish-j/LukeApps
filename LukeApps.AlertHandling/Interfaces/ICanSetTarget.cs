namespace LukeApps.AlertHandling.Interfaces
{
    public interface ICanSetTarget
    {
        ICanPushAlert ToTargetGroups(string[] Groups);

        ICanPushAlert ToTargetEmployees(string[] EmployeeNumbers);

        /// <summary>
        /// Set Target to Current Application
        /// </summary>
        ICanPushAlert ToTargetApp();

        /// <summary>
        /// Set Target to Given Application
        /// </summary>
        ICanPushAlert ToTargetApp(string App);
    }
}