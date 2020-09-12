using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.EmployeeData;
using PhilApprovalFlow.Enum;

namespace LukeApps.ApprovalProcess.ViewModels
{
    public class ReviewerVM
    {
        public ReviewerVM(ITransition transition)
        {
            var provider = EmployeeProvider.GetEmployeeProvider();
            var user = provider.GetUserData(transition.ApproverID);
            EmployeeNumber = user.Username;
            Name = user.DisplayName;
            Position = user.JobTitle;
            Date = (transition.ApproverDecision == DecisionType.Approved) ? transition.AcknowledgementDate?.ToString("dd/MM/yyyy") : null;
            Signature = (transition.ApproverDecision == DecisionType.Approved) ? user.ElectronicSignature : null;
        }

        public string EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Date { get; set; }

        public string Signature { get; set; }
    }
}