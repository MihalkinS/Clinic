using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Api.Models.AppModels
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Data { get; set; }

        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; }

    }
}