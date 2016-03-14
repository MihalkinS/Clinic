using Clinic.Api.Models.AppModels;
using Clinic.Api.Models.Context;
using Clinic.Api.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Clinic.Api.Controllers
{
    [RoutePrefix("api/coment")]
    public class CommentController : ApiController
    {

        ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Comment?doctorId=
        [AllowAnonymous]
        public IHttpActionResult Get(string doctorId)
        {
            var doctor = db.Doctors.FirstOrDefault(x => x.UserId == doctorId);

            return Ok(db.Comments.Where(c => c.UserId == doctorId).Select(d => new
            {
                Text = d.Text,
                DateVisible = ( d.Date.Day > 10 ? (d.Date.Day.ToString()) : ("0" + d.Date.Day.ToString()) )+ "." +
                       ( d.Date.Month > 10 ? (d.Date.Month.ToString()) : ("0" + d.Date.Month.ToString()) ) + "." +
                       d.Date.Year.ToString(),
                DateNonVisible = d.Date,
                DoctorName = doctor.LastName + " " + doctor.FirstName + " " + doctor.MiddleName,
                DoctorAvatarURL = doctor.AvatarURL,
                Position = doctor.Position
            }).ToList());
        }


        // POST: api/Comment
        [Authorize(Roles = "Client")]
        public IHttpActionResult Post([FromBody]CommentViewModel commentRequest)
        {
            //проверка модели на null
            if (commentRequest == null)
            {
                return BadRequest("Bad request data");
            }

            // ищем доктора по ID
            var user = db.Users.Find(commentRequest.DoctorId);
            var roleDoctorId = db.Roles.SingleOrDefault(r => r.Name == "Doctor").Id;
            var isDoctor = user.Roles.First(r => r.RoleId == roleDoctorId);

            // нету доктора в БД, к которому помещается отзыв
            if (isDoctor == null)
            {
                return BadRequest("Doctor not found!");
            }
            else
            {
                //создаем новый отзыв
                Comment comment = new Comment()
                {
                    User = user,
                    Date = DateTime.Now,
                    Text = commentRequest.Text
                };

                db.Comments.Add(comment);
                db.SaveChanges();

                return Ok();
            }
        }


    }
}
