using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.AppModels
{
    public class Drug
    {
        [Key]
        public int Id { get; set; }

        public string DrugName { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }

    }
}