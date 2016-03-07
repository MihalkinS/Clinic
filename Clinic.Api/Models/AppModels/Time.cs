using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Api.Models.AppModels
{
    public class Time
    {
        [Key]
        public int Id { get; set; }
        public TimeSpan HourAndMinutes { get; set; }

        public ICollection<Visit> Visits { get; set; }
        public Time()
        {
            Visits = new List<Visit>();
        }

        public int DayId { get; set; }
        public Day Day { get; set; }
    }
}