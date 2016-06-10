using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Layer.IdentityEntities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace B_Layer.ManagersAndStores
{
    public class AppUserStore : UserStore<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public AppUserStore(DbContext context) : base(context)
        {
            
        }
    }
}
