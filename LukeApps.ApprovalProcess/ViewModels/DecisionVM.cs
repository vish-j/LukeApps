using LukeApps.EmployeeData;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.ApprovalProcess
{
    public class DecisionVM
    {
        public long ID { get; set; }

        public string Display { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Requester Comments")]
        public string RequesterComments { get; set; }

        [Display(Name = "Next Approver")]
        public string Approver { get; set; }

        public Employee ApproverEmployee => EmployeeProvider.GetEmployeeProvider().GetUserData(Approver);

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
    }
}