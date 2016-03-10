using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.AppModels
{
    public class Procedure
    {
        [Key]
        public int Id { get; set; }

        public TimeSpan Time { get; set; }
        public int Cost { get; set; }

        public ICollection<Visit> Visits { get; set; }
        public Procedure()
        {
            Visits = new List<Visit>();
        }
    }
}