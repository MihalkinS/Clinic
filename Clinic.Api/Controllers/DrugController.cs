using Clinic.Api.Models.AppModels;
using Clinic.Api.Models.Context;
using Clinic.Api.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Clinic.Api.Controllers
{
    [RoutePrefix("api/Drug")]
    public class DrugController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Drug/
        [Authorize(Roles = "Doctor, Administrator")]
        public IHttpActionResult Get()
        {
            var drugs = db.Drugs;
            if(drugs != null)
            {
                return Ok(drugs);
            }
            else
            {
                return BadRequest("No drugs");
            }  
        }



        // GET: api/Drug/5
        [Authorize(Roles ="Doctor, Administrator")]
        public IHttpActionResult Get(int drugId)
        {
            var drug = db.Drugs.Find(drugId);
            if (drug != null)
            {
                return Ok(drug);
            }
            else
            {
                return BadRequest("No drug");
            }
        }



        // POST: api/Drug
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult Post([FromBody]DrugViewModel drug)
        {
            var result = db.Drugs.FirstOrDefault(x => x.DrugName == drug.DrugName);
            if(result != null)
            {
                return BadRequest("Drug Exist");
            }
            else
            {
                Drug newDrug = new Drug()
                {
                    DrugName = drug.DrugName,
                    Description = drug.Description,
                    Cost = drug.Cost

                };

                db.Drugs.Add(newDrug);
                db.SaveChanges();

                DrugStorage newItemInStorage = new DrugStorage()
                {
                    Drug = newDrug,
                    DrugId = newDrug.Id,
                    Count = 0
                };

                db.DrugStorage.Add(newItemInStorage);
                db.SaveChanges();

                return Ok();
            }
        }



        // DELETE: api/Drug/5
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult Delete(int drugId)
        {
            var drug = db.Drugs.Find(drugId);

            if (drug != null)
            {
                db.Drugs.Remove(drug);
                return Ok();
            }
            else
            {
                return BadRequest("Not exist");
            }
        }



        // GET: api/Drug/AllDrugs
        [Route("AllDrugs")]
        [Authorize(Roles = "Doctor, Administrator")]
        public IHttpActionResult GetAllDrugs()
        {
            var allDrug = db.DrugStorage;
            if (allDrug != null)
            {
                var result = allDrug.Select(x => new
                {
                    id = x.Id,
                    drugName = x.Drug.DrugName,
                    drugId = x.Drug.Id,
                    count = x.Count,
                    description = x.Drug.Description,
                    cost = x.Drug.Cost
                });

                return Ok(result);
            }
            else
            {
                return BadRequest("No drug");
            }
        }



        // Post: api/Drug/AddDrug
        [Route("AddDrug")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult AddDrug(int drugId, int count)
        {
            var drug = db.DrugStorage.Find(drugId);
            if (drug != null)
            {
                
                db.DrugStorage.Attach(drug);
                drug.Count = drug.Count + count;

                db.Entry(drug).State = EntityState.Modified;
                db.SaveChanges();

                return Ok();
            }
            else
            {
                return BadRequest("Drug not exist");
            }
        }


        // Post: api/Drug/SubDrug
        [Route("SubDrug")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult SubDrug(int drugId, int count)
        {
            var drug = db.DrugStorage.Find(drugId);
            if (drug != null)
            {

                db.DrugStorage.Attach(drug);
                if((drug.Count - count) < 0)
                {
                    return BadRequest();
                }
                else
                {
                    drug.Count = drug.Count - count;
                }

                db.Entry(drug).State = EntityState.Modified;
                db.SaveChanges();

                return Ok();
            }
            else
            {
                return BadRequest("Drug not exist");
            }
        }


    }
}
