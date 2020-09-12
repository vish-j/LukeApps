using System;

namespace LukeApps.ApprovalProcess
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DocumentPAFPathAttribute : PhilApprovalFlow.Attributes.PAFMetadataAttribute
    {
        public DocumentPAFPathAttribute(string path)
        {
            Key = "path";
            Value = path;
        }
    }
}