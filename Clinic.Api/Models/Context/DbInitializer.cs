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
            SetRoles(context);

            // Создаем администратора
            SetAdministrator(context);

            // Заполняем несколько недель(начиная с текущей) пустыми значениями по времени
       //     SetAnyWeek(context);

         //   TestHistory(context);

            TestDoctors(context);

            base.Seed(context);
        }

        private void TestDoctors(ApplicationDbContext context)
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
                userManager.AddToRole(doctor1.Id, ROLES.DOCTOR);
            }

            var result2 = userManager.Create(doctor2, "Qwerty_6");
            if (result1.Succeeded)
            {
                userManager.AddToRole(doctor2.Id, ROLES.DOCTOR);
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
                    WorkTimeStart = "00:08:00",
                    WorkTimeFinish = "00:17:00"
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
                    WorkTimeStart = "00:09:00",
                    WorkTimeFinish = "00:18:00"
                };
                db.Doctors.Add(profile1);
                db.Doctors.Add(profile2);
                db.SaveChanges();

                FillVisits(doctor1.Id);
                FillVisits(doctor2.Id);

            }

        }

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



        private void TestHistory(ApplicationDbContext context)
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

        #region Admin init
        // Для заполнения информации о ролях
        private struct ROLES
        {
            public const string ADMINISTRATOR = "Administrator";
            public const string CLIENT = "Client";
            public const string DOCTOR = "Doctor";
        }

        private void SetRoles(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Создаем 3 роли (
            // Администратор
            // Врач
            // Зарегистрированный клиент )
            var administratorRole = new IdentityRole("Administrator");
            var doctorRole = new IdentityRole("Doctor");
            var clientRole = new IdentityRole("Client");

            //Добавляем в БД
            roleManager.Create(administratorRole);
            roleManager.Create(doctorRole);
            roleManager.Create(clientRole);

        }

        // Для заполнения информации об администраторе
        private struct ADMIN
        {
            public const string USERNAME = "Admin";
            public const string EMAIL = "admin@HealthyPet.com";
            public const string PASSWORD = "Qwerty_6";
        }

        // Для заполнения Администратора
        private void SetAdministrator(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Добавляем Админа (1 на весь проект)
            var administrator = new ApplicationUser()
            {
                Email = ADMIN.EMAIL,
                UserName = ADMIN.USERNAME,
                Confirmation = true,
                EmailConfirmed = true
            };

            var result = userManager.Create(administrator, ADMIN.PASSWORD);

            // Если администратор добавлен, то добавляем ему роль Администратора
            if (result.Succeeded)
            {
                userManager.AddToRole(administrator.Id, ROLES.ADMINISTRATOR);
            }

        }
        #endregion


        // 24 часа * (2 интервала по 30 минут) = 48 раз по 30 минут
        private const int ALLINTERVALINDAY = 24 * INTERVALINHOUR;

        // количество интервалов в 1 час
        private const int INTERVALINHOUR = 2;

        // время одного интервала
        private const int INATERVALDURATION = 60 / INTERVALINHOUR;


        // Для заполнения 5 недель пустыми значениями времени
        private void SetAnyWeek(ApplicationDbContext context)
        {
            // получаем начальный день
            DateTime currDay = DateTime.Now.Date;
            //var currday = currDay.Date;

            // интервал времени в формате TimeSpan
            TimeSpan timeinterval = TimeSpan.FromMinutes(INATERVALDURATION);

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

                for (int interval = 0; interval < ALLINTERVALINDAY; interval++)
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

    }
}