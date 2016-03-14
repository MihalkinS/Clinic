using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Clinic.Api.Models;
using Clinic.Api.Providers;
using Clinic.Api.Results;
using Clinic.Api.Models.Context;
using Clinic.Api.Models.AppModels;
using System.Net;
using System.Text;
using Clinic.Api.Models.ViewModels;

namespace Clinic.Api.Controllers
{
   // [Authorize]
    [RoutePrefix("api/Account")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        
        // Подтверждение Email
        [Route("ConfirmEmail")]
        public IHttpActionResult GetConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return BadRequest("User Id and Code are required");
            }

            // необходимо сделать, что бы код исправить ошибку 
            code = code.Replace(" ","+");

            // пытаемся подтвердить Email по присланному коду и ID пользователя
            var result = UserManager.ConfirmEmail(userId, code);

            if (result.Succeeded)
            {
                string request = "Вы подтвердили ваш Email";
                return Ok(request);
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        // Регистрация. В модель передается:
        // UserName, Email, Password, Comfirm Password, PhoneAddress, Role
        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public IHttpActionResult Register(RegisterBindingModel model)
        {

            if(model == null)
            {
                return BadRequest();
            }

            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            // ;

            // если мы добавляем клиента
            if (model.Role.CompareTo("Client") == 0)
            {
                user.IsClient = true;
                IdentityResult result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Client");
                    // генерируем код для подтверждения email
                    var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                    // отправляет сообщение клиенту о подтверждении регистрации со ссылкой на метод Account Controll
                    UserManager.SendEmail(user.Id, "Подтверждение Email", "Для завершения регистрации : <a href=\"http://localhost:49845/api/account/ConfirmEmail?userId=" + user.Id + "&code=" + code + "\">Завершить</a>");
                }
                else
                {
                    StringBuilder message = new StringBuilder();
                    foreach (var erorr in result.Errors)
                    {
                        message.AppendLine(erorr);
                    }
                    return Content(HttpStatusCode.BadRequest, message.ToString()); //BadRequest(result.Errors.ToString());
                }

                

            }
            // если мы добавляем доктора
            if (model.Role.CompareTo("Doctor") == 0)
            {
                // получаем ID пользователя, который обратился за добавлением врача
                var userId = User.Identity.GetUserId();

                if(userId == null)
                {
                    return BadRequest("Your ID not found!");
                }

                // входит ли пользователь в роль администратора
                var isAdmin = UserManager.IsInRoleAsync(userId, "Administrator");

                // если доктора пытается добавить НЕ админ -> ошибка
                if(isAdmin == null)
                {
                    return BadRequest("You are not an Administrator!");
                }
                // иначе пытаемся добавить доктора в БД
                else
                {
                    user.IsDoctor = true;
                    user.EmailConfirmed = true;

                    // подтверждение аккаунта для доктора
                    user.Confirmation = true;

                    IdentityResult result = UserManager.Create(user, model.Password);

                    if (result.Succeeded)
                    {
                        UserManager.AddToRole(user.Id, "Doctor");

                       // // должны заполнить визиты для доктора нулевыми значениями и связать их с датой и временем
                       // FillVisits(user.Id);
                    }
                    else
                    {
                        return BadRequest(result.ToString());
                    } 
                }
            }


            return Ok();
        }



        // Регистрация клиента. В модель передается:
        // UserName, Email, Password, Comfirm Password, PhoneAddress, Role, Breed, PetName
        // POST api/Account/RegisterClient
        [AllowAnonymous]
        [Route("RegisterDoctor")]
        public IHttpActionResult RegisterClient(RegisterDoctorBindingModel model)
        {

            if (model == null)
            {
                return BadRequest();
            }

            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

                // получаем ID пользователя, который обратился за добавлением врача
                var userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    return BadRequest("Your ID not found!");
                }

                // входит ли пользователь в роль администратора
                var isAdmin = UserManager.IsInRole(userId, "Administrator");

            // если доктора пытается добавить НЕ админ -> ошибка
            if (isAdmin == null)
            {
                return BadRequest("You are not an Administrator!");
            }
            // иначе пытаемся добавить доктора в БД
            else
            {
                user.IsDoctor = true;
                user.EmailConfirmed = true;

                // подтверждение аккаунта для доктора
                user.Confirmation = true;

                

                IdentityResult result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Doctor");

                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        Doctor profile = new Doctor()
                        {
                            UserId = user.Id,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            MiddleName = model.MiddleName,
                            Position = model.Position,
                            WorkTimeStart = model.WorkTimeStart,
                            WorkTimeFinish = model.WorkTimeFinish
                        };
                        db.Doctors.Add(profile);
                        db.SaveChanges();
                    }

                    // должны заполнить визиты для доктора нулевыми значениями и связать их с датой и временем
                    FillTimes(user.Id);
                }
                else
                {
                    return BadRequest(result.ToString());
                }
            }


            return Ok();
        }


        // Для заполнения 5 недель пустыми значениями времени
        private void FillTimes(string doctorId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var days = db.Days;
                var doctor = db.Users.Find(doctorId);

                foreach (var day in days)
                {
                    // начальный интервал в дне 00:00:00
                    TimeSpan hourAndMinutes = TimeSpan.Zero;

                    for (int interval = 0; interval < 24 * 2; interval++)
                    {
                        // создаем интервал и привязываем к Day
                        Time time = new Time()
                        {
                            HourAndMinutes = hourAndMinutes,
                            Day = day,
                            Doctor = doctor
                        };

                        // добавляем к БД
                        db.Times.Add(time);


                        // получаем время следующего интервала
                        hourAndMinutes = hourAndMinutes.Add(TimeSpan.FromMinutes(30));
                    }     
                }
                db.SaveChanges();
            }
        }


        // Регистрация клиента. В модель передается:
        // UserName, Email, Password, Comfirm Password, PhoneAddress, Role, Breed, PetName
        // POST api/Account/RegisterClient
        [AllowAnonymous]
        [Route("RegisterClient")]
        public IHttpActionResult RegisterClient(RegisterClientBindingModel model)
        {

            if (model == null)
            {
                return BadRequest();
            }

            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,

            };

                user.IsClient = true;
                IdentityResult result = UserManager.Create(user, model.Password);

            if (result.Succeeded)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Client profile = new Client()
                    {
                        Breed = model.Breed,
                        Color = model.Color,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        MiddleName = model.MiddleName,
                        PetName = model.PetName,
                        UserId = user.Id
                    };

                    db.Clients.Add(profile);
                    db.SaveChanges();

                }

                UserManager.AddToRole(user.Id, "Client");
                // генерируем код для подтверждения email
                var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                // отправляет сообщение клиенту о подтверждении регистрации со ссылкой на метод Account Controll
                UserManager.SendEmail(user.Id, "Подтверждение Email", "Для завершения регистрации : <a href=\"http://localhost:49845/api/account/ConfirmEmail?userId=" + user.Id + "&code=" + code + "\">Завершить</a>");
            }
            else
            {
                StringBuilder message = new StringBuilder();
                foreach (var erorr in result.Errors)
                {
                    message.AppendLine(erorr);
                }
                return Content(HttpStatusCode.BadRequest, message.ToString());
            }


            return Ok();
        }





        /*
        // Заполняем нулевыми значениями визиты на все время для нового доктора
        private void FillVisits(string doctorId)
        {

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                //ищем доктора по ID
                var user = db.Users.Find(doctorId);

                // получаем все интервалы времени, которые существуют
                var times = db.Times;
                List<Time> timesList = new List<Time>();
                foreach (var time in times)
                {
                    timesList.Add(time);
                }

                // каждый интервал времени связвываем с новым пустым визитом
                foreach (var timeInterval in timesList)
                {
                    Visit visit = new Visit()
                    {
                        Description = "emptyDescription",
                        Сonfirmation = false
                    };

                    visit.Users.Add(user);
                    visit.Times.Add(timeInterval);

                    db.Visits.Add(visit);
                    db.SaveChanges();
                }
            }

        }
        */

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
