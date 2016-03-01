using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Api.Models.AppModels
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Password { get; set; }
        public string AvatarURL { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Color { get; set; }

        public int PetName { get; set; }
        public string Breed { get; set; }

        public ICollection<Visit> Visits { get; set; }

        public Client()
        {
            Visits = new List<Visit>();
        }

    }

}