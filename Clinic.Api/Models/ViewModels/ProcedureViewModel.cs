using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.ViewModels
{
    public class ProcedureViewModel
    {
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public int Cost { get; set; }
    }
}