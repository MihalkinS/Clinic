using Clinic.Api.Models.AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.ViewModels
{
    public class DoctorViewModel
    {
        public string Id { get; set; }

        public string Position { get; set; }
        public string WorkTimeStart { get; set; }
        public string WorkTimeFinish { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string Address { get; set; }

        public string AvatarURL { get; set; }

//        public string Phone { get; set; }
//        public string Email { get; set; }



        public DoctorViewModel(Doctor doctor)
        {
            if (doctor != null)
            {
                Id = doctor.UserId;

                Position = doctor.Position;
                WorkTimeFinish = doctor.WorkTimeFinish;
                WorkTimeStart = doctor.WorkTimeStart;

                FirstName = doctor.FirstName;
                LastName = doctor.LastName;
                MiddleName = doctor.MiddleName;

                Address = doctor.Address;

                AvatarURL = doctor.AvatarURL;

//            Phone = doctor.Phone;
//            Email = doctor.Email;
            }

        }

    }
}