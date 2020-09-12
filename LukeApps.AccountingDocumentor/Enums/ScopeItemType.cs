using System.ComponentModel.DataAnnotations;

namespace LukeApps.AccountingDocumentor.Enums
{
    public enum ScopeItemType
    {
        [Display(Name = "Scope Item: Main")]
        Main,

        [Display(Name = "Scope Item: Optional")]
        Optional,

        [Display(Name = "Scope Item: Additional")]
        Additional
    }
}