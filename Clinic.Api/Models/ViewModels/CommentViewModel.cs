using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.ViewModels
{
    public class CommentViewModel
    {
        public string DoctorId { get; set; }
        public string Text { get; set; }
    }
}