using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.ViewModels
{
    public class VisitViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Сonfirmation { get; set; }

        public string ClientId { get; set; }
        public string DoctorId { get; set; }
        public int ProcedureId { get; set; }

    }
}