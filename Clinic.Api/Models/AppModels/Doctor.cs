using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Api.Models.AppModels
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string AvatarURL { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public TimeSpan WorkTime { get; set; }

        public ICollection<Visit> Visits { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public Doctor()
        {
            Visits = new List<Visit>();
            Comments = new List<Comment>();
        }


    }
}