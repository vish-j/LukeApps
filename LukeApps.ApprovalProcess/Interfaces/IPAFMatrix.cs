using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeApps.ApprovalProcess.Interfaces
{
    public interface IPAFMatrix
    {
        string OriginatorID { get; set; }

        string ReviewerID { get; set; }

        string ApproverID { get; set; }
    }
}
