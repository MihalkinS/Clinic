using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models
{
    public class DoctorsContext : DbContext
    {
        public DoctorsContext() : base("DoctorsConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DoctorsContext>());
        }

        public DbSet<Doctor> Doctors { get; set; }
    }
}