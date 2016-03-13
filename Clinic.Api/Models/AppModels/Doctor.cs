using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Api.Models.AppModels
{
    public class Doctor : UserInfo
    {

        public string Position { get; set; }
        public string WorkTimeStart { get; set; }
        public string WorkTimeFinish { get; set; }
        
        /*
        public ICollection<Visit> Visits { get; set; }
       // public ICollection<Comment> Comments { get; set; }

        public Doctor()
        {
            Visits = new List<Visit>();
          //  Comments = new List<Comment>();
        }
        */

    }
}