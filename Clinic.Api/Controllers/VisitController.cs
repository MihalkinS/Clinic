using Clinic.Api.Models.AppModels;
using Clinic.Api.Models.Context;
using Clinic.Api.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Clinic.Api.Controllers
{
    public class VisitController : ApiController
    {

        ApplicationDbContext db = new ApplicationDbContext();

        
        // Получить визит по ID
        // GET: api/Visit/5
        [Authorize(Roles = "Doctor, Client")]
        public IHttpActionResult Get(int visitId)
        {

            var visit = db.Visits.FirstOrDefault(v => v.Id == visitId);

            // если не найден визит 
            if(visit == null)
            {
                return BadRequest("Visit not found!");
            }
            else
            {
                return Ok(visit);
            }
            
        }



        // Записать визит
        // Post: api/Visit
        [Authorize(Roles = "Doctor, Client")]
        public IHttpActionResult Post([FromBody]VisitPostModel model)
        {

            var doctor = db.Users.FirstOrDefault(x => x.Id == model.DoctorId);
            if(doctor == null)
            {
                return BadRequest("Doctor not found!");
            }
            var client = db.Users.FirstOrDefault(x => x.Id == model.ClientId);
            if (client == null)
            {
                return BadRequest("Doctor not found!");
            }
            var procedure = db.Procedures.FirstOrDefault(x => x.Id == model.ProcedureId);
            if (procedure == null)
            {
                return BadRequest("Doctor not found!");
            }

            var startInterval = db.Times.FirstOrDefault(x => x.Id == model.TimeId);

            var finishInterval = startInterval.HourAndMinutes.Add(procedure.Time);

            var timesForDoctor = db.Times.Where(x => x.DoctorId == model.DoctorId && x.DayId == model.DayId);
            var times = timesForDoctor.Where(x => x.HourAndMinutes >= startInterval.HourAndMinutes && x.HourAndMinutes < finishInterval);
            var timeIsBusy = times.FirstOrDefault(x => x.VisitId != null);

            if(timeIsBusy != null)
            {
                return BadRequest("Time is busy!");
            }

            Visit newVisit = new Visit()
            {
                Doctor = doctor,
                Client = client,
                Procedure = procedure,
                Description = "empty",
                Сonfirmation = false
            };

            db.Visits.Add(newVisit);
            db.SaveChanges();
            
            foreach (var t in times)
            {
                t.Visit = newVisit;
                db.Entry(t).State = EntityState.Modified;
            }

            foreach(var t in times)
            {
                newVisit.Times.Add(t);
            }
            db.Entry(newVisit).State = EntityState.Modified;
            db.SaveChanges();

            return Ok();
        }



        // Записать визит по ID
        // PUT: api/Visit/5
        [Authorize(Roles = "Doctor, Client")]
        public IHttpActionResult Put([FromBody]VisitViewModel model)
        {

            Visit visit = db.Visits.First(r => r.Id == model.Id);

            var doctor = db.Users.First(r => r.Id == model.DoctorId);

            if (doctor == null)
            {
                return BadRequest("Doctor not found!");
            }

            var client = db.Users.First(r => r.Id == model.ClientId);

            if (client == null)
            {
                return BadRequest("Client not found!");
            }

            var procedure = db.Procedures.First(r => r.Id == model.ProcedureId);

            if (procedure == null)
            {
                return BadRequest("Procedure not found!");
            }

            // не существует визита, который мы хотим отредактировать
            if (visit == null)
            {
                return BadRequest("Visit not found!");
            }
            else
            {
                db.Visits.Attach(visit);

                visit.Description = model.Description;
                visit.Сonfirmation = model.Сonfirmation;
                visit.Doctor = doctor;
                visit.Client = client;
                visit.Procedure = procedure;

                db.Entry(visit).State = EntityState.Modified;
                db.SaveChanges();

                return Ok();
            }

        }










        /*
        // Получить визит по ID
        // GET: api/Visit
        [AllowAnonymous]
        public IHttpActionResult Get()
        {

            var visit = db.Visits.Select(r => new
            {
                Id = r.Id,
                Description = r.Description,
                Confiration = r.Сonfirmation,
                ProcedureId = r.Procedure.Id,
                ProcedureCost = r.Procedure.Cost,
                ProcedureTime = r.Procedure.Time,
                DoctorId = r.Doctor.Id,
                ClientId = r.Client.Id
            });

            // если не найден визит 
            if (visit == null)
            {
                return BadRequest("Visit not found!");
            }
            else
            {
                return Ok(visit);
            }

        }

        /*
        // Получить визит по ID
        // GET: api/Visit/5
        [Authorize(Roles = "Doctor, Client")]
        public IHttpActionResult Get(int visitId)
        {

            var visit = db.Visits.Where(v => v.Id == visitId).Select(r => new
            {
                Id = r.Id,
                Description = r.Description,
                Confiration = r.Сonfirmation,
                ProcedureId = r.Procedure.Id,
                ProcedureCost = r.Procedure.Cost,
                ProcedureTime = r.Procedure.Time,
                DoctorId = r.Users.FirstOrDefault(u => u.IsDoctor == true).Id,
                ClientId = r.Users.FirstOrDefault(u => u.IsClient == true).Id
            });

            // если не найден визит 
            if (visit == null)
            {
                return BadRequest("Visit not found!");
            }
            else
            {
                return Ok(visit);
            }

        }
        */
    }
}
