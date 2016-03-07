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

    [Authorize(Roles ="Administrator")]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {

        ApplicationDbContext db = new ApplicationDbContext();


        // Получает информацию о всех пользователях (Id, UserName, Email)
        // GET: api/User
        public List<UserViewModel> Get()
        {
            List<UserViewModel> listOfUsers = new List<UserViewModel>();

            var users = db.Users;

            foreach (var item in users)
            {
                listOfUsers.Add(new UserViewModel()
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber
                });
            }
            return listOfUsers;
        }


        // Получает информацию о всех пользователях 
        // входящих в роль (Id, UserName, Email)
        [Route("GetUsersInRole")]
        // GET: api/User/GetUsersInRole?role=Administrator
        public List<UserViewModel> GetUsersInRole(string role)
        {
            List<UserViewModel> listOfUsers = new List<UserViewModel>();


            var usersRole = db.Roles.SingleOrDefault(m => m.Name == role);
            var usersInRole = db.Users.Where(m => m.Roles.Any(r => r.RoleId == usersRole.Id));

            foreach (var item in usersInRole)
            {
                listOfUsers.Add(new UserViewModel()
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber
                });
            }



            return listOfUsers;
        }


        // Получить информацию о конкретном пользоватиле по ID 
        // (только email и username)
        // GET: api/User/5
        public IHttpActionResult Get(string id)
        {
            UserViewModel result = new UserViewModel();

            var user = db.Users.Find(id);
            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                result.Id = user.Id;
                result.UserName = user.UserName;
                result.Email = user.Email;
                result.PhoneNumber = user.PhoneNumber;
                return Ok(result);
            }
        }


        // Удаляет пользователя вместе с его профилем
        // DELETE: api/User/5
        public IHttpActionResult Delete(string id)
        {
            var user = db.Users.Find(id);

            var adminRole = db.Roles.SingleOrDefault(m => m.Name == "Administrator");
            var isAdmin = user.Roles.Any(r => r.RoleId == adminRole.Id);
            if (isAdmin)
            {
                return BadRequest("you are trying to delete Administrator!");
            }


            if (user == null)
            {
                return BadRequest("User not found!");
            }
            else
            {
                // Если у пользователя профиль доктора
                var DoctorProfile = db.Doctors.Find(id);
                if (DoctorProfile != null)
                {
                    db.Doctors.Remove(DoctorProfile);
                    db.SaveChanges();
                }

                // Если у пользователя профиль клиента
                var ClientProfile = db.Clients.Find(id);
                if (ClientProfile != null)
                {
                    db.Clients.Remove(ClientProfile);
                    db.SaveChanges();
                }

                db.Users.Remove(user);
                db.SaveChanges();

                return Ok();
            }
        }
    }
}
