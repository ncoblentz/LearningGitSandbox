using ASPNETCoreKestrelResearch.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ASPNETCoreKestrelResearch.Security
{
    public class CustomPasswordValidator<TUser> : PasswordValidator<TUser> where TUser : IdentityUser
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {            
            IdentityResult baseResult = await base.ValidateAsync(manager, user, password);

            if (!baseResult.Succeeded)
                return baseResult;
            else
            {
                if (password.ToLower().Contains(user.UserName.ToLower()))
                {                    
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "UsernameInPassword",
                        Description = "Your password cannot contain your username"
                    });
                }
                else
                    return IdentityResult.Success;
            }           
        }
    }
}
