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

    public class CommentController : ApiController
    {

        ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        // GET: api/Comment/5
        public IHttpActionResult Get(string doctorId)
        {
            return Ok(db.Comments.Where(c => c.UserId == doctorId).Select(d => d.Text).ToList());
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
                    Data = DateTime.Now,
                    Text = commentRequest.Text
                };

                db.Comments.Add(comment);
                db.SaveChanges();

                return Ok();
            }
        }


    }
}
