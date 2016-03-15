using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Clinic.Api.Models.AppModels;
using System;
using System.Collections.Generic;

namespace Clinic.Api.Models.Context
{
    public class DbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            // Создаем роли для пользователей
            DBHelper.SetRoles(context);
            // Создаем администратора
            DBHelper.SetAdministrator(context);

            // Создаем начальное количество дней
            DBHelper.FillStartDays(context);

            // Заполняем несколько недель(начиная с текущей) пустыми значениями по времени
            //  SetAnyWeek(context);

            DBHelper.TestHistory(context);
            DBHelper.TestProcedures(context);
            DBHelper.TestDoctors(context);
            DBHelper.TestClient(context);

            base.Seed(context);
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

               // каждый интервал времени связываем с новым пустым визитом
               foreach (var timeInterval in timesList)
               {
                   Visit visit = new Visit()
                   {
                       Description = "emptyDescription",
                       Сonfirmation = false,
                       Doctor = user
                   };

                   //visit.Times.Add(timeInterval);

                   db.Visits.Add(visit);
                   db.SaveChanges();
               }
           }

       }
       

        // Для заполнения 5 недель пустыми значениями времени
        private void SetAnyWeek(ApplicationDbContext context)
        {
            // получаем начальный день
            DateTime currDay = DateTime.Now.Date;
            //var currday = currDay.Date;

            // интервал времени в формате TimeSpan
            TimeSpan timeinterval = TimeSpan.FromMinutes(DBHelper.INATERVALDURATION);

            // заполняем 7 дней в неделю * на количество недель!!!!!!!!!!!!!!!!!!!!!!
            for (int i = 0; i < 7 * 2; i++)
            {
                // создаем день
                Day day = new Day() { Date = currDay, DayOfWeek = currDay.DayOfWeek.ToString() };
                
                // помещаем день в БД
                context.Days.Add(day);
                context.SaveChanges();

                // следующий день для заполнения
                currDay = currDay.AddDays(1);

                // начальный интервал в дне 00:00:00
                TimeSpan hourAndMinutes = TimeSpan.Zero;

                for (int interval = 0; interval < DBHelper.ALLINTERVALINDAY; interval++)
                {
                    // создаем интервал и привязываем к Day
                    Time time = new Time()
                    {
                        HourAndMinutes = hourAndMinutes,
                        Day = day
                    };

                    // добавляем к БД
                    context.Times.Add(time);
                    context.SaveChanges();

                    // получаем время следующего интервала
                    hourAndMinutes = hourAndMinutes.Add(timeinterval);
                }
            }
        }
        */
    }
}