namespace WebApplication9.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication9.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebApplication9.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Moderator" };

                manager.Create(role);
            }
            var userManager = new UserManager<ApplicationUser>(
           new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "kmetcalf@gtcc.edu"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "kmetcalf@gtcc.edu",
                    Email = "kmetcalf@gtcc.edu",
                    FirstName = "Kevin",
                    LastName = "Metcalf",
                    DisplayName = "kevin"
                }, "uncgweil09");
            }
            var userId = userManager.FindByEmail("kmetcalf@gtcc.edu").Id;
            userManager.AddToRole(userId, "Admin");
        }

    }
}
