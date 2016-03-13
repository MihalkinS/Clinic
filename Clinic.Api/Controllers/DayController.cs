using Clinic.Api.Models.AppModels;
using Clinic.Api.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Clinic.Api.Controllers
{
    [RoutePrefix("api/Day")]
    public class DayController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();


        // Получаем 7 дней начиная от текущего
        // GET: api/Day/CurrWeek
        [AllowAnonymous]
        [Route("CurrWeek")]
        public IHttpActionResult GetCurrWeek(string doctorId)
        {

            // получаем начальную дату
            DateTime dateNow = DateTime.Now.Date;

            // получаем крайний 7 день
            DateTime dateLast = dateNow.Date.AddDays(6);

            // выбираем время, у которого день входит в интервал [текущий день, текущий день + 6]
            var days = db.Days.Where(d => d.Date >= dateNow && d.Date <= dateLast && d.Times.All(t => t.Doctor.Id == doctorId)).Select(d => new
            {
                dayOfWeek = d.DayOfWeek,
                day = d.Date.Day,
                month = d.Date.Month > 10 ? d.Date.Month.ToString() : "0" + d.Date.Month.ToString(),
                year = d.Date.Year,
                id = d.Id
            });

            if (days == null || days.Count() != 7)
            {
                return BadRequest("Week not found!");
            }
            else
            {
                return Ok(days);
            }

        }


        // Получаем следующих 7 дней, начиная со следующего от присланного
        // GET: api/Day/NextWeek?doctorId=1f6f999b-72fa-45a4-8f84-48c&lastDayId=107
        [AllowAnonymous]
        [Route("NextWeek")]
        public IHttpActionResult GetNextWeek(string doctorId, int lastDayId)
        {
            // проверка на существования крайнего дня
            var day = db.Days.FirstOrDefault(d => d.Id == lastDayId);
            if (day == null)
            {
                return BadRequest("Day not found!");
            }

            // получаем дату последнего дня предыдущей недели
            DateTime dateFirst = day.Date;

            // получаем начальную дату для следующей недели
            dateFirst = dateFirst.AddDays(1);

            // получаем крайний 7 день
            DateTime dateLast = dateFirst.Date.AddDays(6);

            // выбираем время, у которого день входит в интервал [начальный день, начальный день + 6]
            var days = db.Days.Where(d => d.Date >= dateFirst.Date && d.Date <= dateLast.Date && d.Times.All(t => t.Doctor.Id == doctorId)).Select(d => new
            {
                dayOfWeek = d.DayOfWeek,
                day = d.Date.Day,
                month = d.Date.Month > 10 ? d.Date.Month.ToString() : "0" + d.Date.Month.ToString(),
                year = d.Date.Year,
                id = d.Id
            });

            if (days == null || days.Count() != 7)
            {
                return BadRequest("Week not found!");
            }
            else
            {
                return Ok(days);
            }

        }


        // Получаем предыдущих 7 дней, начиная со предыдущего от присланного
        // GET: api/Day/PrevWeek?doctorId=1f6f999b-72fa-45a4-8f84-48c&lastDayId=107
        [AllowAnonymous]
        [Route("PrevWeek")]
        public IHttpActionResult GetPrevWeek(string doctorId, int firstDayId)
        {

            // проверка на существования крайнего дня
            var day = db.Days.FirstOrDefault(d => d.Id == firstDayId);
            if (day == null)
            {
                return BadRequest("Day not found!");
            }

            // получаем дату 1ого дня предыдущей недели
            DateTime dateLast = day.Date;

            // получаем конечную дату для предыдущей недели
            dateLast = dateLast.AddDays(-1);

            // получаем 1ый день предыдущей недели
            DateTime dateFirst = dateLast.Date.AddDays(-6);

            // выбираем время, у которого день входит в интервал 
            var days = db.Days.Where(d => d.Date >= dateFirst.Date && d.Date <= dateLast.Date && d.Times.All(t => t.Doctor.Id == doctorId)).Select(d => new
            {
                dayOfWeek = d.DayOfWeek,
                day = d.Date.Day,
                month = d.Date.Month > 10 ? d.Date.Month.ToString() : "0" + d.Date.Month.ToString(),
                year = d.Date.Year,
                id = d.Id
            });

            if (days == null || days.Count() != 7)
            {
                return BadRequest("Week not found!");
            }
            else
            {
                return Ok(days);
            }

        }

        // Получаем интервалы времени для 7 дней, начиная c текущего
        // GET: api/Day/CurrWeekTimes?doctorId=1f6f999b-72fa-45a4-8f84-48c
        [AllowAnonymous]
        [Route("CurrWeekTimes")]
        public IHttpActionResult GetCurrWeekTimes(string doctorID)
        {

            // проверяем, если доктор с таким ID
            var user = db.Users.Find(doctorID);
            var roleDoctorId = db.Roles.SingleOrDefault(r => r.Name == "Doctor").Id;
            var isDoctor = user.Roles.First(r => r.RoleId == roleDoctorId);

            // нету доктора в БД для которого нужно получить интервалы времени
            if (isDoctor == null)
            {
                return BadRequest("Doctor not found!");
            }
            else
            {
                // получаем начальную дату
                DateTime dateNow = DateTime.Now.Date;

                // получаем крайний 7 день
                DateTime dateLast = dateNow.Date.AddDays(6);

                // получаем все интервалы времени, которые входят в нужный диапозон дней
                var times = db.Times.Where(t => t.Day.Date >= dateNow && t.Day.Date <= dateLast);

                // оставляем только те интервалы времени, которые предназначаются отдельному доктору
                var timesForDoctor = db.Times.Where(x => x.Doctor.Id == doctorID).Select(r => new
                {
                    TimeId = r.Id,
                    DayId = r.Day.Id,
                    HM = r.HourAndMinutes,
                    DoctorID = r.Doctor.Id,
                    ClientId = r.Visit.Client.Id == null ? "empty" : r.Visit.Client.Id,
                    VisitId = r.Visit == null ? "empty" : r.Visit.Client.Id,
                });

                if (times == null)
                {
                    return BadRequest("Week not found!");
                }
                else
                {
                    return Ok(timesForDoctor);
                }
            }



        }


        // Получаем интервалы времени для 7 дней, начиная со следующего от присланного
        // GET: api/Day/NextWeekTimes?doctorId=1f6f999b-72fa-45a4-8f84-48c
        [AllowAnonymous]
        [Route("NextWeekTimes")]
        public IHttpActionResult GetNextWeekTimes(string doctorID, int lastDayId)
        {

            // проверяем, если доктор с таким ID
            var user = db.Users.Find(doctorID);
            var roleDoctorId = db.Roles.SingleOrDefault(r => r.Name == "Doctor").Id;
            var isDoctor = user.Roles.First(r => r.RoleId == roleDoctorId);

            // нету доктора в БД для которого нужно получить интервалы времени
            if (isDoctor == null)
            {
                return BadRequest("Doctor not found!");
            }
            else
            {
                // проверка на существования крайнего дня
                var day = db.Days.FirstOrDefault(d => d.Id == lastDayId);
                if (day == null)
                {
                    return BadRequest("Day not found!");
                }

                // получаем дату последнего дня предыдущей недели
                DateTime dateFirst = day.Date;

                // получаем начальную дату для следующей недели
                dateFirst = dateFirst.AddDays(1);

                // получаем крайний 7 день
                DateTime dateLast = dateFirst.Date.AddDays(6);

                // получаем все интервалы времени, которые входят в нужный диапозон дней
                var times = db.Times.Where(t => t.Day.Date >= dateFirst && t.Day.Date <= dateLast);



                // оставляем только те интервалы времени, которые предназначаются отдельному доктору
                var timesForDoctor = db.Times.Where(x => x.Doctor.Id == doctorID).Select(r => new
                {
                    TimeId = r.Id,
                    DayId = r.Day.Id,
                    HM = r.HourAndMinutes,
                    DoctorID = r.Doctor.Id,
                    ClientId = r.Visit.Client.Id == null ? "empty" : r.Visit.Client.Id,
                    VisitId = r.Visit == null ? "empty" : r.Visit.Client.Id,
                });

                if (times == null)
                {
                    return BadRequest("Week not found!");
                }
                else
                {
                    return Ok(timesForDoctor);
                }
            }



        }


        // Получаем интервалы времени для 7 дней, начиная со предыдущего от присланного
        // GET: api/Day/NextWeekTimes?doctorId=1f6f999b-72fa-45a4-8f84-48c
        [AllowAnonymous]
        [Route("PrevWeekTimes")]
        public IHttpActionResult GetPrevWeekTimes(string doctorID, int firstDayId)
        {

            // проверяем, если доктор с таким ID
            var user = db.Users.Find(doctorID);
            var roleDoctorId = db.Roles.SingleOrDefault(r => r.Name == "Doctor").Id;
            var isDoctor = user.Roles.First(r => r.RoleId == roleDoctorId);

            // нету доктора в БД для которого нужно получить интервалы времени
            if (isDoctor == null)
            {
                return BadRequest("Doctor not found!");
            }
            else
            {
                // проверка на существования крайнего дня
                var day = db.Days.FirstOrDefault(d => d.Id == firstDayId);
                if (day == null)
                {
                    return BadRequest("Day not found!");
                }

                // получаем дату 1ого дня предыдущей недели
                DateTime dateLast = day.Date;

                // получаем конечную дату для предыдущей недели
                dateLast = dateLast.AddDays(-1);

                // получаем 1ый день предыдущей недели
                DateTime dateFirst = dateLast.Date.AddDays(-6);

                // получаем все интервалы времени, которые входят в нужный диапозон дней
                var times = db.Times.Where(t => t.Day.Date >= dateFirst && t.Day.Date <= dateLast);

                // оставляем только те интервалы времени, которые предназначаются отдельному доктору
                var timesForDoctor = db.Times.Where(x => x.Doctor.Id == doctorID).Select(r => new
                {
                    TimeId = r.Id,
                    DayId = r.Day.Id,
                    HM = r.HourAndMinutes,
                    DoctorID = r.Doctor.Id,
                    ClientId = r.Visit.Client.Id == null ? "empty" : r.Visit.Client.Id,
                    VisitId = r.Visit == null ? "empty" : r.Visit.Client.Id,
                });

                if (times == null)
                {
                    return BadRequest("Week not found!");
                }
                else
                {
                    return Ok(timesForDoctor);
                }
            }



        }


        /*
        
        // GET: api/Day/5
        public IHttpActionResult Get(int id)
        {
            var item2 = db.Days.Where(d => d.Id == id)
                .Select(x => x.Times.Where(t => t.DayId == id)
                .Select(r => r));

            return Ok(item2);
        }
        

        // Получаем предыдущих 7 дней, начиная со предыдущего от присланного
        // GET: api/Day/PrevWeek?doctorId=1f6f999b-72fa-45a4-8f84-48c&lastDayId=107
        [AllowAnonymous]
        [Route("PrevWeek")]
        public IHttpActionResult GetPrevWeek(string doctorId, int firstDayId)
        {

            // проверка на существования крайнего дня
            var day = db.Days.FirstOrDefault(d => d.Id == firstDayId);
            if (day == null)
            {
                return BadRequest("Day not found!");
            }

            // получаем дату 1ого дня предыдущей недели
            DateTime dateLast = day.Date;

            // получаем конечную дату для предыдущей недели
            dateLast = dateLast.AddDays(-1);

            // получаем 1ый день предыдущей недели
            DateTime dateFirst = dateLast.Date.AddDays(-6);

            // выбираем время, у которого день входит в интервал 
            var days = db.Days.Where(d => d.Date >= dateFirst && d.Date <= dateLast).Select(d => new
            {
                dayOfWeek = d.DayOfWeek,
                day = d.Date.Day,
                month = d.Date.Month,
                year = d.Date.Year,
                id = d.Id
            });

            if (days == null)
            {
                return BadRequest("Week not found!");
            }
            else
            {
                return Ok(days);
            }

        }


        // Получаем 7 дней начиная от текущего
        // GET: api/Day/CurrWeek
        [AllowAnonymous]
        [Route("CurrWeek")]
        public IHttpActionResult GetCurrWeek()
        {

            // получаем начальную дату
            DateTime dateNow = DateTime.Now.Date;

            // получаем крайний 7 день
            DateTime dateLast = dateNow.Date.AddDays(6);
           
            // выбираем время, у которого день входит в интервал [текущий день, текущий день + 6]
            var days = db.Days.Where(d => d.Date >= dateNow && d.Date <= dateLast).Select(d => new
            {
                dayOfWeek = d.DayOfWeek,
                day = d.Date.Day,
                month = d.Date.Month > 10 ? d.Date.Month.ToString() : "0" + d.Date.Month.ToString(),
                year = d.Date.Year,
                id = d.Id
            });

            if (days == null)
            {
                return BadRequest("Week not found!");
            }
            else
            {
                return Ok(days);
            }
            
        }


        // Получаем следующих 7 дней, начиная со следующего от присланного
        // GET: api/Day/NextWeek?doctorId=1f6f999b-72fa-45a4-8f84-48c&lastDayId=107
        [AllowAnonymous]
        [Route("NextWeek")]
        public IHttpActionResult GetNextWeek(string doctorId, int lastDayId)
        {
            // проверка на существования крайнего дня
            var day = db.Days.FirstOrDefault(d => d.Id == lastDayId);
            if(day == null)
            {
                return BadRequest("Day not found!");
            }

            // получаем дату последнего дня предыдущей недели
            DateTime dateFirst = day.Date;

            // получаем начальную дату для следующей недели
            dateFirst = dateFirst.AddDays(1);

            // получаем крайний 7 день
            DateTime dateLast = dateFirst.Date.AddDays(6);

            // выбираем время, у которого день входит в интервал 
            var days = db.Days.Where(d => d.Date >= dateFirst && d.Date <= dateLast).Select(d => new
            {
                dayOfWeek = d.DayOfWeek,
                day = d.Date.Day,
                month = d.Date.Month,
                year = d.Date.Year,
                id = d.Id
            });
            if (days == null)
            {
                return BadRequest("Week not found!");
            }
            else
            {
                return Ok(days);
            }

        }



        // Получаем интервалы времени для 7 дней, начиная c текущего
        // GET: api/Day/CurrWeekTimes?doctorId=1f6f999b-72fa-45a4-8f84-48c
        [AllowAnonymous]
        [Route("CurrWeekTimes")]
        public IHttpActionResult GetCurrWeekTimes(string doctorID)
        {

            // проверяем, если доктор с таким ID
            var user = db.Users.Find(doctorID);
            var roleDoctorId = db.Roles.SingleOrDefault(r => r.Name == "Doctor").Id;
            var isDoctor = user.Roles.First(r => r.RoleId == roleDoctorId);

            // нету доктора в БД для которого нужно получить интервалы времени
            if (isDoctor == null)
            {
                return BadRequest("Doctor not found!");
            }
            else
            {
                // получаем начальную дату
                DateTime dateNow = DateTime.Now.Date;

                // получаем крайний 7 день
                DateTime dateLast = dateNow.Date.AddDays(6);

                // получаем все интервалы времени, которые входят в нужный диапозон дней
                var times = db.Times.Where(t => t.Day.Date >= dateNow && t.Day.Date <= dateLast);

                // оставляем только те интервалы времени, которые предназначаются отдельному доктору
                var timesForDoctor = times.Where(t => t.Visits.All(v => v.Users.Any(u => u.Id == doctorID))).Select(r => new
                {
                    id = r.Id,
                    hourAndMinutes = r.HourAndMinutes,
                    dayId = r.DayId,
                    visit = r.Visits.ToList().Select(vis => new
                    {
                        id = vis.Id,
                        description = vis.Description,
                        confirmation = vis.Сonfirmation,
                        procedureId = vis.Procedure.Id
                    })
                });

                if (times == null)
                {
                    return BadRequest("Week not found!");
                }
                else
                {
                    return Ok(timesForDoctor);
                }
            }

                

        }

        [AllowAnonymous]
        [Route("TimeIntervals")]
        public IHttpActionResult GetTimeIntervals()
        {

            var result = db.Times.Select(r => r.Visits.Select(t => new
            {
                user = t.Users.All(u => u.IsDoctor == true)
            }));

            // оставляем только те интервалы времени, которые предназначаются отдельному доктору
            var times = db.Times.Select(x => new
            {
                Id = x.Id,
                HM = x.HourAndMinutes,
                doctorId = x.Visits.Where(v => v.Users.Any(u => u.IsDoctor == true)).Select(r => r.Users.Where(i => i.IsDoctor == true).Select(y => y.Id))
            });

            if (times == null)
            {
                return BadRequest("Week not found!");
            }
            else
            {
                return Ok(times);
            }
        }

        */


    }
}
