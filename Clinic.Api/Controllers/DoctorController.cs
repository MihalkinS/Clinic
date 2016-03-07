using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Clinic.Api.Models.AppModels;
using Clinic.Api.Models.Context;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;
using Clinic.Api.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Clinic.Api.Controllers
{
    [RoutePrefix("api/Doctor")]
    public class DoctorController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();


        // Получить всех докторов (профили)
        // GET: api/Doctor
        [AllowAnonymous]
        public IHttpActionResult Get()
        {

            List<DoctorViewModel> listOfDoctors = new List<DoctorViewModel>();

            //var role = db.Roles.SingleOrDefault(m => m.Name == "Doctor");
            //var usersInRole = db.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id));

            var role = db.Roles.SingleOrDefault(m => m.Name == "Doctor");
            var usersInRoleId = db.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id)).Select(x => x.Id).ToList();


            if (usersInRoleId == null)
            {
                return BadRequest("Doctors do not exist!");
            }
            else
            {
                
                foreach (var item in usersInRoleId)
                {
                    var doctor = db.Doctors.SingleOrDefault(d => d.UserId == item);
                    if(doctor != null)
                    {
                        // так как doctor имеет кроме свойств еще и связи
                        // мы отправляем DoctorViewModel массив (в ней есть только свойства id, name и т.д.)
                        listOfDoctors.Add(new DoctorViewModel(doctor));
                    }       
                }

                return Ok(listOfDoctors);
            }

            
        }


        // Получает информацию о докторе (профиль)
        // GET: api/Doctor/5
        [AllowAnonymous]
        public IHttpActionResult Get(string id)
        {

            var doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return BadRequest("Doctor not found!");
            }
            else
            {
                // так как doctor имеет кроме свойств еще и связи
                // мы отправляем DoctorViewModel (в ней есть только свойства id, name и т.д.)
                DoctorViewModel convertToSend = new DoctorViewModel(doctor);
                return Ok(convertToSend);
            }           
        }



        // добавляет дополнительную информацию о докторе (Профиль)
        // POST: api/Doctor
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult Post([FromBody]DoctorViewModel model)
        {

            if(model == null)
            {
                return BadRequest("Bad request data");
            }

            // нету пользователя, к которому нужно присоеденить профиль
            if(db.Users.Find(model.Id) == null)
            {
                return BadRequest("Doctor not found!");
            }

            Doctor doctor = new Doctor()
            {
                Address = model.Address,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Position = model.Position,
                WorkTimeStart = model.WorkTimeStart,
                WorkTimeFinish = model.WorkTimeFinish,
                UserId = model.Id,
                AvatarURL = model.AvatarURL
            };

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return Ok();
        }



        // Редактирует информацию о докторе (Профиль)
        // PUT: api/Doctor/5
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult Put([FromBody]DoctorViewModel model)
        {

            Doctor doctor = db.Doctors.First(r => r.UserId == model.Id);

            // нету пользователя, к кторому нужно присоеденить профиль
            if (doctor == null)
            {
                return BadRequest("Doctor not found!");
            }
            else
            {
                db.Doctors.Attach(doctor);

                doctor.Address = model.Address;
                doctor.FirstName = model.FirstName;
                doctor.LastName = model.LastName;
                doctor.MiddleName = model.MiddleName;
                doctor.Position = model.Position;
                doctor.WorkTimeStart = model.WorkTimeStart;
                doctor.WorkTimeFinish = model.WorkTimeFinish;
                doctor.AvatarURL = model.AvatarURL;

                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();

                return Ok();
            }
        }



        // Удаляет информацию о докторе (профиль)
        // DELETE: api/Doctor/5
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult Delete(string id)
        {

            var doctorProfile = db.Doctors.Find(id);

            if(doctorProfile == null)
            {
                return BadRequest("Profile not found!");
            }
            else
            {
                db.Doctors.Remove(doctorProfile);
                db.SaveChanges();
                return Ok();
            }
        }
    }
}
