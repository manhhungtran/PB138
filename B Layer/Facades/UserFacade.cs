using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using B_Layer.DTO;
using B_Layer.ManagersAndStores;
using DA_Layer;
using DA_Layer.IdentityEntities;
using Microsoft.AspNet.Identity;

namespace B_Layer.Facades
{
    public class UserFacade
    {
        public void Register(UserDTO userDTO)
        {
            var userManager = new AppUserManager(new AppUserStore(new AppDbContext()));
            AppUser appUser = Mapping.Mapper.Map<AppUser>(userDTO);

            userManager.Create(appUser, userDTO.Password);
        }

        public ClaimsIdentity Login(string userName, string password)
        {
            var userManager = new AppUserManager(new AppUserStore(new AppDbContext()));
            var wantedUser = userManager.FindByName(userName);

            if (wantedUser == null)
            {
                return null;
            }

            AppUser user = userManager.Find(wantedUser.UserName, password);

            if (user == null)
            {
                return null;
            }

            return userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}
