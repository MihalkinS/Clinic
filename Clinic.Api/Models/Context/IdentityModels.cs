using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Clinic.Api.Models.AppModels;
using System.Data.Entity;
using System.Collections.Generic;

namespace Clinic.Api.Models.Context
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public virtual Client Client { get; set; }
        public virtual Doctor Doctor { get; set; }

        // для более быстрого поиска клиента и доктора
        public bool IsDoctor { get; set; }
        public bool IsClient { get; set; }

        // подтверждение аккаунта для клиентов. потверждение делает доктор
        public bool Confirmation { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Visit> Visits { get; set; }

        public ApplicationUser() : base()
        {
            Comments = new List<Comment>();
            Visits = new List<Visit>();
            IsClient = false;
            IsDoctor = false;
            Confirmation = false;
        }




        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("VetConnection", throwIfV1Schema: false)
        {
            //Database.Initialize(true);
        }


        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Visit> Visits { get; set; }

        public DbSet<Day> Days { get; set; }
        public DbSet<Time> Times { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Drug> Drugs { get; set; }
        public DbSet<DrugStorage> DrugStorage { get; set; }

        public DbSet<Procedure> Procedures { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    //    public System.Data.Entity.DbSet<Clinic.Api.Models.Context.ApplicationUser> ApplicationUsers { get; set; }
    }
}