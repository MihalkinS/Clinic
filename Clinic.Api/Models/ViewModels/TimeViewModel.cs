using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.ViewModels
{
    public class TimeViewModel
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int DayId { get; set; }

    }
}