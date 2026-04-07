using Microsoft.AspNetCore.Identity;

namespace Presentation.Helpers
{
    public class RolesManagementHelper
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public RolesManagementHelper(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        { 
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void AllocatesRoleToUser(string userId, string role)
        {
            // Logic to allocate a role to a user
            var task = _userManager.FindByIdAsync(userId);
            task.Wait();

            if(task.Result != null)
                 _userManager.AddToRoleAsync(task.Result, role).Wait();
        }

        public void DeallocatesRoleFromUser(string userId, string role)
        {
            // Logic to deallocate a role from a user

            var task = _userManager.FindByIdAsync(userId);
            task.Wait();

            if (task.Result != null)
                _userManager.RemoveFromRoleAsync(task.Result, role).Wait();


        }

        //it sets the database with a pre-set list of roles
        //allocate all users the default role of "User"
        public void DefaultRolesSetup()
        {
            // Logic to set up default roles in the system

            string[] defaultRoles = new string[] { "admin", "user", "moderator", "organizer" };
            foreach (var defaultRole in defaultRoles) {
                if (!_roleManager.RoleExistsAsync(defaultRole).Result)
                {
                    _roleManager.CreateAsync(new IdentityRole(defaultRole)).Wait();
                }
            }


            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                if (!_userManager.IsInRoleAsync(user, "user").Result)
                {
                    _userManager.AddToRoleAsync(user, "user").Wait();
                }
            }

        }

    }
}
