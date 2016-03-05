using Clinic.Api.Models.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models
{
    public class UserInfo
    {
        [Key, ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string Address { get; set; }

        public string AvatarURL { get; set; }

//        public string Phone { get; set; }
//        public string Email { get; set; }

    }
}