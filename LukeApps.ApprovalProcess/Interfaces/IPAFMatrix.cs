namespace LukeApps.ApprovalProcess.Interfaces
{
    public interface IPAFMatrix
    {
        string OriginatorID { get; set; }

        string ReviewerID { get; set; }

        string ApproverID { get; set; }
    }
}