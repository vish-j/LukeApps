using LukeApps.AspIdentity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LukePurchaseSystem.Startup))]

namespace LukePurchaseSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup, creating first Admin Role and creating a default Admin User
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //create a Admin super user who will maintain the website
                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@gmail.com";
                user.FirstName = "Vishvas";
                user.LastName = "Janardhan";
                user.JobTitle = "Dev";
                user.Initials = "VJ";
                user.PhoneNumber = "94374000";

                string userPWD = "A@Z200711";

                var chkUser = userManager.Create(user, userPWD);

                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Admin");
                }
            }

            if (userManager.FindById("sully") == null)
            {
                var user = new ApplicationUser();
                user.UserName = "sully";
                user.Email = "sully@gmail.com";
                user.FirstName = "Suleiman";
                user.LastName = "Al Habsi";
                user.JobTitle = "COO";
                user.Initials = "SAH";
                user.PhoneNumber = "90000000";

                string userPWD = "A@Z200711";

                var chkUser = userManager.Create(user, userPWD);

                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Manager");
                }
            }

            if (userManager.FindById("naseer") == null)
            {
                var user = new ApplicationUser();
                user.UserName = "naseer";
                user.Email = "naseer@gmail.com";
                user.FirstName = "Naseer";
                user.LastName = "Al Habsi";
                user.JobTitle = "CEO";
                user.Initials = "NAH";
                user.PhoneNumber = "90000000";

                string userPWD = "A@Z200711";

                var chkUser = userManager.Create(user, userPWD);

                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Manager");
                }
            }

            // creating Creating CEO role
            if (!roleManager.RoleExists("CEO"))
            {
                var role = new IdentityRole();
                role.Name = "CEO";
                roleManager.Create(role);
            }

            // creating Creating Manager role
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);
            }

            // creating Creating Finance role
            if (!roleManager.RoleExists("Finance"))
            {
                var role = new IdentityRole();
                role.Name = "Finance";
                roleManager.Create(role);
            }
        }
    }
}