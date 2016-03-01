using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Web.Http;
using Clinic.Api.Models;
using System.Web.Http.Description;
using System.Data.Entity.Infrastructure;
using Clinic.Api.Models.AppModels;
using Clinic.Api.Models.Context;

namespace Clinic.Api.Controllers
{

    [Authorize(Roles = "Administrator")]
    [RoutePrefix("api/Doctor")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DoctorController : ApiController
    {

        private DBContext db = new DBContext();

        [Route("ListOfDoctors")]
        public IQueryable<Doctor> GetDoctors()
        {
            return db.Doctors;
        }

        [ResponseType(typeof(Doctor))]
        public IHttpActionResult GetDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);

            if(doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        [ResponseType(typeof(Doctor))]
        public IHttpActionResult Post(Doctor doctor)
        {
            db.Doctors.Add(doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultAPI", new { doctor.Id }, doctor);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult Put(int id, Doctor doctor)
        {

            if(id != doctor.Id)
            {
                return BadRequest();
            }

            db.Entry(doctor).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool DoctorExists(int id)
        {
            return db.Doctors.Count(e => e.Id == id) > 0;
        }


        [ResponseType(typeof(Doctor))]
        public IHttpActionResult Delete(int id)
        {
            Doctor doctor = db.Doctors.Find(id);

            if(doctor == null)
            {
                return NotFound();
            }

            db.Doctors.Remove(doctor);
            db.SaveChanges();

            return Ok(doctor);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
