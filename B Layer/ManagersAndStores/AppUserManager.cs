using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Layer.IdentityEntities;
using Microsoft.AspNet.Identity;

namespace B_Layer.ManagersAndStores
{
    public class AppUserManager : UserManager<AppUser, int>
    {
        public AppUserManager(IUserStore<AppUser, int> store) : base(store)
        {
            
        }
    }
}
