using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Clinic.Api.Models.Context;

namespace Clinic.Api.Models.AppModels
{
    public class Time
    {
        [Key]
        public int Id { get; set; }
        public TimeSpan HourAndMinutes { get; set; }

        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }

        public string VisitId { get; set; }
        public Visit Visit { get; set; }

        /*public ICollection<Visit> Visits { get; set; }
        public Time()
        {
            Visits = new List<Visit>();
        }
        */

        public int DayId { get; set; }
        public Day Day { get; set; }
    }
}