using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Layer.IdentityEntities;
using Microsoft.AspNet.Identity;

namespace B_Layer.ManagersAndStores
{
    public class AppRoleManager : RoleManager<AppRole, int>
    {
        public AppRoleManager(IRoleStore<AppRole, int> store) : base(store)
        {
            
        }
    }
}
