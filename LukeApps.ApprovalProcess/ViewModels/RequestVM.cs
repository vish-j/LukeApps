using LukeApps.EmployeeData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LukeApps.ApprovalProcess
{
    public class RequestVM
    {
        public RequestVM()
        {
            Approvers = new List<string>();
        }
        public long ID { get; set; }

        public string Display { get; set; }

        public string Approver { get; set; }

        public Employee ApproverEmployee => EmployeeProvider.GetEmployeeProvider().GetUserData(Approver);

        public List<string> Approvers { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        public int RoleEnum { get; set; } = 0;
    }
}