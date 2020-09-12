using LukeApps.ApprovalProcess.Classes;
using LukeApps.ApprovalProcess.Interfaces;
using LukeApps.TrackingExtended;

namespace LukeApps.GeneralPurchase.Models
{
    public class EnquiryTransition : Transition, ITransition, IEntity, IAuditDetail
    {
        public EnquiryTransition()
        {
            AuditDetail = new AuditDetail();
        }

        public EnquiryTransition(Transition t, long id) : base(t)
        {
            EnquiryID = id;
            AuditDetail = new AuditDetail();
        }

        public long EnquiryID { get; set; }

        public virtual Enquiry Enquiry { get; set; }

        public object GetID()
        {
            return EnquiryID;
        }

        public AuditDetail AuditDetail { get; set; }

        public bool IsDeleted { get; set; }
    }
}