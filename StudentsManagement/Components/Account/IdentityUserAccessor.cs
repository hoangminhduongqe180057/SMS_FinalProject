using Microsoft.AspNetCore.Identity;
using StudentsManagement.Data;

namespace StudentsManagement.Components.Account
{
    //internal sealed class IdentityUserAccessor(UserManager<ApplicationUser> userManager, IdentityRedirectManager redirectManager)
    //{
    //    public async Task<ApplicationUser> GetRequiredUserAsync(HttpContext context)
    //    {
    //        var user = await userManager.GetUserAsync(context.User);

    //        if (user is null)
    //        {
    //            redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
    //        }

    //        return user;
    //    }
    //}

    internal sealed class IdentityUserAccessor
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IdentityRedirectManager redirectManager;

        public IdentityUserAccessor(IServiceScopeFactory serviceScopeFactory, IdentityRedirectManager redirectManager)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.redirectManager = redirectManager;
        }

        public async Task<ApplicationUser?> GetRequiredUserAsync(HttpContext context)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
                return null;
            }

            return user;
        }
    }
}
