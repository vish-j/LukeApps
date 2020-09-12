using LukeApps.EmployeeData;
using PhilApprovalFlow;

namespace LukeApps.ApprovalProcess.Interfaces
{
    public interface ITransition : IPAFTransition
    {
        Employee Approver { get; }

        Employee RequestedBy { get; }

        object GetID();
    }
}