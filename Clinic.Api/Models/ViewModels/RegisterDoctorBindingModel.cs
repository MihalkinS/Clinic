using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.ViewModels
{
    public class RegisterDoctorBindingModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public string WorkTimeStart { get; set; }
        public string WorkTimeFinish { get; set; }
    }
}