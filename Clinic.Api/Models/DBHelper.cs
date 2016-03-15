using Clinic.Api.Models.AppModels;
using Clinic.Api.Models.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models
{
    static class DBHelper
    {
        static DBHelper()
        {
            INTERVALINHOUR = 2;
            WEEKCOUNT = 2;
            ALLINTERVALINDAY = 24 * INTERVALINHOUR;
            INATERVALDURATION = 60 / INTERVALINHOUR;
        }

        static class Roles
        {
            public static readonly string Administrator = "Administrator";
            public static readonly string Client = "Client";
            public static readonly string Doctor = "Doctor";
        }

        static class Admin
        {
            public static readonly string UserName = "Admin";
            public static readonly string Email = "admin@HealthyPet.com";
            public static readonly string Password = "Qwerty_6";
        }

        // 24 часа * (2 интервала по 30 минут) = 48 раз по 30 минут
        static readonly int ALLINTERVALINDAY;
        // количество интервалов в 1 час
        static readonly int INTERVALINHOUR;
        // время одного интервала
        static readonly int INATERVALDURATION;
        // количество начальных недель
        static readonly int WEEKCOUNT;

        public static void TestClient(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var client = new ApplicationUser()
            {
                Email = "client@HeathyPet.com",
                UserName = "cl",
                PhoneNumber = "5580808",
                Confirmation = true,
                EmailConfirmed = true
            };

            var result1 = userManager.Create(client, "Qwerty_6");
            if (result1.Succeeded)
            {
                userManager.AddToRole(client.Id, DBHelper.Roles.Client);
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Client profile = new Client()
                {
                    UserId = client.Id,
                    Address = "Колоса 76",
                    FirstName = "Антон",
                    LastName = "Твердоруков",
                    MiddleName = "Александрович",
                    Color = "черный",
                    Breed = "Хаски",
                    PetName = "Nick"
                };

                db.Clients.Add(profile);
                db.SaveChanges();
            }


        }

        public static void TestProcedures(ApplicationDbContext context)
        {
            Procedure pr1 = new Procedure()
            {
                Name = "Операция",
                Time = TimeSpan.FromHours(2),
                Cost = 100
            };

            Procedure pr2 = new Procedure()
            {
                Name = "Операция 2 ",
                Time = TimeSpan.FromMinutes(100),
                Cost = 210
            };

            context.Procedures.Add(pr1);
            context.Procedures.Add(pr2);
            context.SaveChanges();
        }

        public static void TestDoctors(ApplicationDbContext context)
        {

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var doctor1 = new ApplicationUser()
            {
                Email = "doctor1@HeathyPet.com",
                UserName = "Doctor1",
                PhoneNumber = "5580808",
                Confirmation = true,
                EmailConfirmed = true
            };



            var doctor2 = new ApplicationUser()
            {
                Email = "doctor2@HeathyPet.com",
                UserName = "Doctor2",
                PhoneNumber = "5457243",
                Confirmation = true
            };

            var result1 = userManager.Create(doctor1, "Qwerty_6");
            if (result1.Succeeded)
            {
                userManager.AddToRole(doctor1.Id, DBHelper.Roles.Doctor);
            }

            var result2 = userManager.Create(doctor2, "Qwerty_6");
            if (result1.Succeeded)
            {
                userManager.AddToRole(doctor2.Id, DBHelper.Roles.Doctor);
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Doctor profile1 = new Doctor()
                {
                    UserId = doctor1.Id,
                    Address = "Колоса 76",
                    FirstName = "Галина",
                    LastName = "Твердорукова",
                    MiddleName = "Александровна",
                    Position = "Врач",
                    AvatarURL = "../content/img/avatars/Твердорукова.jpg",
                    WorkTimeStart = "08:00:00",
                    WorkTimeFinish = "17:00:00"
                };
                Doctor profile2 = new Doctor()
                {
                    UserId = doctor2.Id,
                    Address = "Колоса 78",
                    FirstName = "Анатолий",
                    LastName = "Белохалатов",
                    MiddleName = "Иванович",
                    Position = "Главный специалист",
                    AvatarURL = "../content/img/avatars/Белохалатов.jpg",
                    WorkTimeStart = "09:00:00",
                    WorkTimeFinish = "18:00:00"
                };
                db.Doctors.Add(profile1);
                db.Doctors.Add(profile2);
                db.SaveChanges();

                DBHelper.FillTimesForDoctor(db, doctor1.Id);
                DBHelper.FillTimesForDoctor(db, doctor2.Id);

            }

        }

        public static void TestHistory(ApplicationDbContext context)
        {
            Drug drug1 = new Drug() { DrugName = "Conabis", Description = "Very strong drug!", Cost = 100 };
            Drug drug2 = new Drug() { DrugName = "Conabis2", Description = "Very strong drug!2", Cost = 200 };

            context.Drugs.AddRange(new List<Drug> { drug1, drug2 });
            context.SaveChanges();

            DrugStorage storage1 = new DrugStorage() { Drug = drug1, Count = 777 };
            DrugStorage storage2 = new DrugStorage() { Drug = drug2, Count = 101 };

            context.DrugStorage.AddRange(new List<DrugStorage> { storage1, storage2 });
            context.SaveChanges();
        }

        // Для заполнения пустыми значениями времени для каждого доктора
        public static void FillTimesForDoctor(ApplicationDbContext context, string doctorId)
        {

            var daysID = context.Days.Select(x => x.Id).ToList();
            var doctor = context.Users.Find(doctorId);

            //List<Time> timesList = new List<Time>();

            foreach (var dayID in daysID)
            {
                var day = context.Days.SingleOrDefault(x => x.Id == dayID);
                // начальный интервал в дне 00:00:00
                TimeSpan hourAndMinutes = TimeSpan.Zero;

                for (int interval = 0; interval < DBHelper.ALLINTERVALINDAY; interval++)
                {
                    
                    // создаем интервал и привязываем к Day
                    Time time = new Time()
                    {
                        HourAndMinutes = hourAndMinutes,
                        Day = context.Days.SingleOrDefault(x => x.Id == dayID),
                        Doctor = doctor
                    };

                    context.Times.Add(time);
                    context.SaveChanges();

                    context.Days.Attach(context.Days.SingleOrDefault(x => x.Id == dayID));
                    context.Days.SingleOrDefault(x => x.Id == dayID).Times.Add(time);
                    context.Entry(context.Days.SingleOrDefault(x => x.Id == dayID)).State = EntityState.Modified;
                    context.SaveChanges();

                    //timesList.Add(time);

                    // получаем время следующего интервала
                    hourAndMinutes = hourAndMinutes.Add(TimeSpan.FromMinutes(30));
                }
                
            }
          //  context.SaveChanges();

        }
        /*
        foreach (var time in timesList)
        {
            var day = context.Days.FirstOrDefault(x => x.Id == time.DayId);
            context.Days.Attach(day);
            day.Times.Add(time);
            context.Entry(day).State = EntityState.Modified;
            context.SaveChanges();
        }
       */


        // Для заполнения 2 недели пустыми значениями времени
        public static void FillStartDays(ApplicationDbContext context)
        {
            // получаем начальный день
            DateTime currDay = DateTime.Now.Date;

            // заполняем 7 дней * на количество недель
            for (int i = 0; i < 7 * WEEKCOUNT; i++)
            {
                // создаем день
                Day day = new Day() { Date = currDay, DayOfWeek = currDay.DayOfWeek.ToString() };

                // помещаем день в БД
                context.Days.Add(day);
                context.SaveChanges();

                // переходим на следующий день
                currDay = currDay.AddDays(1);
            }
        }


        #region Admin init
        public static void SetRoles(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Создаем 3 роли (
            // Администратор
            // Врач
            // Зарегистрированный клиент )
            var administratorRole = new IdentityRole(DBHelper.Roles.Administrator);
            var doctorRole = new IdentityRole(DBHelper.Roles.Doctor);
            var clientRole = new IdentityRole(DBHelper.Roles.Client);

            //Добавляем в БД
            roleManager.Create(administratorRole);
            roleManager.Create(doctorRole);
            roleManager.Create(clientRole);

        }

        // Для заполнения Администратора
        public static void SetAdministrator(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Добавляем Админа (1 на весь проект)
            var administrator = new ApplicationUser()
            {
                Email = DBHelper.Admin.Email,
                UserName = DBHelper.Admin.UserName,
                Confirmation = true,
                EmailConfirmed = true
            };

            var result = userManager.Create(administrator, DBHelper.Admin.Password);

            // Если администратор добавлен, то добавляем ему роль Администратора
            if (result.Succeeded)
            {
                userManager.AddToRole(administrator.Id, DBHelper.Roles.Administrator);
            }

        }
        #endregion
    }
}