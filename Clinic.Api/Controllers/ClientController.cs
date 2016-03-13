using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Clinic.Api.Models;
using Clinic.Api.Models.Context;
using Clinic.Api.Models.AppModels;
using Clinic.Api.Models.ViewModels;
using System.Data.Entity;

namespace Clinic.Api.Controllers
{
    
    [RoutePrefix("api/Client")]
    public class ClientController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();


        // Получить всех клиентов (профили)
        // GET: api/Client
        [Authorize(Roles = "Administrator, Doctor")]
        public IHttpActionResult Get()
        {

            var users = db.Clients.Select(x => new
            {
                Id = x.UserId,
                FIO = string.Concat(x.LastName, " ", x.FirstName, " ", x.MiddleName),
                Breed = x.Breed,
                Color = x.Color,
                PetName = x.PetName,
                Address = x.Address
            });

            return Ok(users);

            /*
            List<ClientViewModel> listOfClients = new List<ClientViewModel>();

            var role = db.Roles.SingleOrDefault(m => m.Name == "Client");
            var usersInRoleId = db.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id)).Select(x => x.Id).ToList();


            if (usersInRoleId == null)
            {
                return BadRequest("Clients do not exist!");
            }
            else
            {

                foreach (var item in usersInRoleId)
                {
                    var client = db.Clients.FirstOrDefault(d => d.UserId == item);
                    if (client != null)
                    {
                        // так как client имеет кроме свойств еще и связи
                        // мы отправляем ClientViewModel массив (в ней есть только свойства id, name, breed и т.д.)
                        listOfClients.Add(new ClientViewModel(client));
                    }
                }

                return Ok(listOfClients);
            }
           */

        }


        // Получает информацию о клиенте (профиль)
        // GET: api/Client/5
        [Authorize(Roles = "Administrator, Client, Doctor")]
        public IHttpActionResult Get(string id)
        {

            var client = db.Clients.Find(id);
            if (client == null)
            {
                return BadRequest("Client not found!");
            }
            else
            {
                // так как doctor имеет кроме свойств еще и связи
                // мы отправляем DoctorViewModel (в ней есть только свойства id, name и т.д.)
                ClientViewModel convertToSend = new ClientViewModel(client);
                return Ok(convertToSend);
            }
        }


        // добавляет дополнительную информацию о клиенте (Профиль)
        // POST: api/Client
        [Authorize(Roles = "Client")]
        public IHttpActionResult Post([FromBody]ClientViewModel model)
        {

            if (model == null)
            {
                return BadRequest("Bad request data");
            }

            // нету пользователя, к которому нужно присоеденить профиль
            if (db.Users.Find(model.Id) == null)
            {
                return BadRequest("Client not found!");
            }

            Client client = new Client()
            {
                Address = model.Address,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                PetName = model.PetName,
                Breed = model.Breed,
                Color = model.Color,
                UserId = model.Id,
                AvatarURL = model.AvatarURL
            };

            db.Clients.Add(client);
            db.SaveChanges();

            return Ok();
        }


        // Редактирует информацию о клиенте (Профиль)
        // PUT: api/Client/5
        [Authorize(Roles = "Client")]
        public IHttpActionResult Put([FromBody]ClientViewModel model)
        {

            Client client = db.Clients.First(r => r.UserId == model.Id);

            // нету пользователя, к кторому нужно присоеденить профиль
            if (client == null)
            {
                return BadRequest("Client not found!");
            }
            else
            {
                db.Clients.Attach(client);

                client.Address = model.Address;
                client.FirstName = model.FirstName;
                client.LastName = model.LastName;
                client.MiddleName = model.MiddleName;
                client.PetName = model.PetName;
                client.Breed = model.Breed;
                client.Color = model.Color;
                client.UserId = model.Id;
                client.AvatarURL = model.AvatarURL;

                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();

                return Ok();
            }
        }


        // Удаляет информацию о клиенте (профиль)
        // DELETE: api/Client/5
        [Authorize(Roles = "Client")]
        public IHttpActionResult Delete(string id)
        {

            var clientProfile = db.Clients.Find(id);

            if (clientProfile == null)
            {
                return BadRequest("Profile not found!");
            }
            else
            {
                db.Clients.Remove(clientProfile);
                db.SaveChanges();
                return Ok();
            }
        }
    }
}
