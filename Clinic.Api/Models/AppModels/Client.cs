using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Api.Models.AppModels
{
    public class Client : UserInfo
    {
        
        public string PetName { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }

        public ICollection<Visit> Visits { get; set; }

        public Client()
        {
            Visits = new List<Visit>();
        }
    }

}