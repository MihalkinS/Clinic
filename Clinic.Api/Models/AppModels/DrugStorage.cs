using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.AppModels
{
    public class DrugStorage
    {
        [Key]
        public int Id { get; set; }

        public int? DrugId { get; set; }
        public Drug Drug { get; set; }

        public int Count { get; set; }
    }
}