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
    [RoutePrefix("api/Drug")]
    public class ProcedureController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();


        // GET: api/Procedure
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            return Ok(db.Procedures);
        }

        // GET: api/Procedure/5
        [AllowAnonymous]
        public IHttpActionResult Get(int procedureId)
        {

            var procedure = db.Procedures.Find(procedureId);

            if (procedure == null)
            {
                return BadRequest("Procedure not found!");
            }
            else
            {
                return Ok(procedure);
            }
           
        }

        // POST: api/Procedure
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult Post([FromBody]ProcedureViewModel procedure)
        {
            if(procedure == null)
            {
                return BadRequest("Model invalid!");
            }

            var exist = db.Procedures.FirstOrDefault(x => x.Name == procedure.Name);
            if(exist != null)
            {
                return BadRequest("Procedure exist!");
            }

            Procedure newProcedure = new Procedure()
            {
                Name = procedure.Name,
                Cost = procedure.Cost,
                Time = procedure.Time
            };

            db.Procedures.Add(newProcedure);
            db.SaveChanges();

            return Ok();
            
        }

        // DELETE: api/Procedure?procedureId=
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult Delete(int procedureId)
        {
            var procedure = db.Procedures.FirstOrDefault(x => x.Id == procedureId);

            if (procedure != null)
            {
                db.Procedures.Remove(procedure);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest("Procedure not exist");
            }
        }
    }
}
