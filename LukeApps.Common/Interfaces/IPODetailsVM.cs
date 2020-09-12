namespace LukeApps.Common.Interfaces
{
    public interface IPODetailVM
    {
        string TebJobNo { get; set; }

        string SrNo { get; set; }

        string PONo { get; set; }

        string Item { get; set; }

        string PODate { get; set; }

        string SuppliPO { get; set; }

        string PODescription { get; set; }

        string Area { get; set; }

        string Discipline { get; set; }

        string POAmount { get; set; }

        string WBSCode { get; set; }

        string NetworkNo { get; set; }

        string NWActivity { get; set; }

        string OldPONo { get; set; }

        string ApplicantName { get; set; }

        string InvAmount { get; set; }

        string POType { get; set; }

        string POClaim { get; set; }

        string Validity { get; set; }

        string POManHrs { get; set; }
    }
}