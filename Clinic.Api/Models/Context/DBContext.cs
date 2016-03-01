using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Clinic.Api.Models.AppModels;

namespace Clinic.Api.Models.Context
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DBConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DBContext>());
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Visit> Visits { get; set; }

        public DbSet<Day> Days { get; set; }
        public DbSet<Time> Times { get; set; }

        public DbSet<Comment> Comments { get; set; }


    }
}