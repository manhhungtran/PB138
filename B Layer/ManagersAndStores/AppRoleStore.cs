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
    public class AppRoleStore : RoleStore<AppRole, int, AppUserRole>
    {
        public AppRoleStore(DbContext context) : base(context)
        {
            
        }
    }
}
