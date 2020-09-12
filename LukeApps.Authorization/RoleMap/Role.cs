using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web.Security;

namespace LukeApps.Authorization.RoleMap
{
    public static partial class Role
    {
        #region General

        public const string Dev = "Admin";

        public const string AllUsersOman = "Domain\\Role";

        #endregion General

        #region Management Reference System

        [Display(Name = "Management-Reference Procurement")]
        public const string MRProcurement = "Domain\\Role";

        [Display(Name = "Management-Reference Procurement Admin")]
        public const string MRProcurementAdmin = "Domain\\Role";

        [Display(Name = "Management-Reference Finance")]
        public const string MRFinance = "Domain\\Role";

        #endregion Management Reference System

        #region Small Orders

        [Display(Name = "Small Orders Focal Point")]
        public const string SOFocalPoint = "Domain\\Role";

        [Display(Name = "Small Orders Managers")]
        public const string SOManagers = "Domain\\Role";

        [Display(Name = "Small Orders Authorized Signatories")]
        public const string SOAuthSignatories = "Domain\\Role";

        #endregion Small Orders

        public static string[] GetUserGroups(this IPrincipal user)
            => Roles.GetRolesForUser(user.Identity.Name);

        public static bool CheckIfUserInRoles(this IPrincipal user, params string[] roles)
        {
            // Cleaner -> In Case if multiple roles already joined in an array location
            var inputtedRoles = string.Join(",", roles).Split(',');

            //Get all Roles of user to commpare
            var userRoles = Roles.GetRolesForUser(user.Identity.Name);

            foreach (var item in userRoles)
            {
                if (inputtedRoles.Contains(item))
                    return true;
            }

            return false;
        }
    }
}