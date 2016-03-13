using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.ViewModels
{
    public class VisitPostModel
    {
        public int TimeId { get; set; }
        public string DoctorId { get; set; }
        public string ClientId { get; set; }
        public int ProcedureId { get; set; }
        public int DayId { get; set; }
    }
}