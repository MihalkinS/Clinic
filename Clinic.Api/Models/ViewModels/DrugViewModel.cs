using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.ViewModels
{
    public class DrugViewModel
    {
        public string DrugName { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
    }
}