using LukeApps.Common.Helpers;
using LukeApps.EmployeeData;
using PhilApprovalFlow.Models;

namespace LukeApps.ApprovalProcess.Classes
{
    public class Transition : PAFTransition
    {
        public Transition()
        {
        }

        public Transition(PAFTransition t) : base(t)
        {
        }

        public Employee Approver => getUser(base.ApproverID);

        private Employee getUser(string user)
        {
            return EmployeeProvider.GetEmployeeProvider().GetUserData(user);
        }

        public Employee RequestedBy => getUser(RequesterID);

        public override string ToString()
        {
            var provider = EmployeeProvider.GetEmployeeProvider();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(Approver?.DisplayName);
            sb.Append(" (");
            sb.Append(AcknowledgementDate ?? RequestedDate);
            sb.Append(") - ");
            sb.Append(ApproverDecision.GetDisplay());
            if (ApproverComments != null)
            {
                sb.Append(", Comments: ");
                sb.Append(ApproverComments);
            }

            sb.Append(" Requested By ");
            sb.Append(RequestedBy?.DisplayName);
            if (RequesterComments != null)
            {
                sb.Append(", Comments: ");
                sb.Append(RequesterComments);
            }

            //send out our new string
            return sb.ToString();
        }

        public long GetParentID()
        {
            return 0;
        }
    }
}