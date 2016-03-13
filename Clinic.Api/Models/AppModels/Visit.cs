using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Clinic.Api.Models.Context;

namespace Clinic.Api.Models.AppModels
{
    public class Visit
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
        public bool Сonfirmation { get; set; }

        public string ClientId { get; set; }
        public ApplicationUser Client { get; set; }

        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }

        public int? ProcedureId { get; set; }
        public Procedure Procedure { get; set; }


        //public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Time> Times { get; set; }
        public ICollection<Drug> Drugs { get; set; }
        public Visit()
        {
            Times = new List<Time>();
            //Users = new List<ApplicationUser>();
            Drugs = new List<Drug>();
        }


        //public string Procedure { get; set; }
        //public bool State { get; set; }

        //public int? DoctorId { get; set; }
        //public Doctor Doctor { get; set; }

        //public int? ClientId { get; set; }
        //public Client Client { get; set; }


    }
}