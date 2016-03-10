using Clinic.Api.Models.Context;
using System;
using System.Collections.Generic;
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
            if(visit == null)
            {
                return BadRequest("Visit not found!");
            }
            else
            {
                return Ok(visit);
            }
            
        }

    }
}
