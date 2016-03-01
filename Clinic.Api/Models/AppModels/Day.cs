using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Api.Models.AppModels
{
    public class Day
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Time> Times { get; set; }

        public Day()
        {
            Times = new List<Time>();
        }
    }
}