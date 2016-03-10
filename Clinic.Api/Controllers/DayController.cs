﻿using Clinic.Api.Models.AppModels;
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

        /*
        // GET: api/Day/5
        public IHttpActionResult Get(int id)
        {
            var item2 = db.Days.Where(d => d.Id == id)
                .Select(x => x.Times.Where(t => t.DayId == id)
                .Select(r => r));

            return Ok(item2);
        }
        */

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


        
        
    }
}