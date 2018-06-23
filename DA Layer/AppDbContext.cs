using System.Data.Entity;
using DA_Layer.Entities;
using DA_Layer.IdentityEntities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DA_Layer
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public DbSet<Person> People { get; set; }

        public AppDbContext() : base("FinalDbSolution")
	    {
        }
    }

}
