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
        // количество недель и интервалов для начального заполнения
        static DBHelper()
        {
            INTERVALINHOUR = 2;
            WEEKCOUNT = 2;
            ALLINTERVALINDAY = 24 * INTERVALINHOUR;
            INATERVALDURATION = 60 / INTERVALINHOUR;
            FUTUREDAYS = 21;
        }

        public static class Roles
        {
            public static readonly string Administrator = "Administrator";
            public static readonly string Client = "Client";
            public static readonly string Doctor = "Doctor";
        }

        public static class Admin
        {
            public static readonly string UserName = "Admin";
            public static readonly string Email = "admin@HealthyPet.com";
            public static readonly string Password = "Qwerty_6";
        }

        // 24 часа * (2 интервала по 30 минут) = 48 раз по 30 минут
        public static readonly int ALLINTERVALINDAY;
        // количество интервалов в 1 час
        public static readonly int INTERVALINHOUR;
        // время одного интервала
        public static readonly int INATERVALDURATION;
        // количество начальных недель
        public static readonly int WEEKCOUNT;
        // необходимое количество НЕПРОШЕДШИХ дней, которые должны в БД
        public static readonly int FUTUREDAYS;

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

            foreach (var dayID in daysID)
            {

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

                    // получаем время следующего интервала
                    hourAndMinutes = hourAndMinutes.Add(TimeSpan.FromMinutes(30));
                }
                
            }

        }

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

        // проверка количества дней, которые хранятся в БД:
        // как только остается меньше 3х недель вперед -- заполняем
        public static void CheckNumberOfDays(ApplicationDbContext context)
        {
            // количество оставшихся дней
            var ResidueDayCount = context.Days.Where(d => d.Date >= DateTime.Now).Count();

            if( ResidueDayCount >= FUTUREDAYS )
            {
                return;
            }
            // если в БД осталось менее 21 дня
            else
            {
                // заполняем недостающее количество дней + 7 с запасом
                FillAdditionalDays(context, FUTUREDAYS - ResidueDayCount);
                return;
            }
        }

        // Для заполнения недостающего количества НЕПРОШЕДШИХ дней в БД
        public static void FillAdditionalDays(ApplicationDbContext context, int CountAdditionalDays)
        {
            // получаем конечный день в БД
            DateTime lastDay = context.Days.Max(x => x.Date);

            // выбираем день, который идет за последним
            DateTime satrtDayForFill = lastDay.AddDays(1);

            // получаем роль доктора
            var doctorRoleId = context.Roles.FirstOrDefault(x => x.Name == Roles.Doctor).Id;

            List<int> massOfDaysId = new List<int>();

            // заполняем недостающее количество дней
            for (int i = 0; i < CountAdditionalDays; i++)
            {
                // создаем день
                Day day = new Day() { Date = satrtDayForFill, DayOfWeek = satrtDayForFill.DayOfWeek.ToString() };

                // помещаем день в БД
                context.Days.Add(day);
                context.SaveChanges();

                massOfDaysId.Add(day.Id);

                // смещаем крайний день
                satrtDayForFill = satrtDayForFill.AddDays(1);
            }

            // получаем ID докторов
            var doctors = context.Users.Where(x => x.Roles.Any(r => r.RoleId == doctorRoleId)).Select(s => s.Id).ToList();

            // заполняем каждый день для отдельного доктора
            foreach (var doctorId in doctors)
            {
                FillAdditionalTimeIntervals(context, massOfDaysId, doctorId);
            }
        }

        // Для заполнения недостающего количества интервалов в НЕПРОШЕДШИХ днях
        public static void FillAdditionalTimeIntervals(ApplicationDbContext context, List<int> massOfDaysId, string doctorId)
        {

            var doctor = context.Users.Find(doctorId);

            foreach (var dayID in massOfDaysId)
            {

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

                    // получаем время следующего интервала
                    hourAndMinutes = hourAndMinutes.Add(TimeSpan.FromMinutes(30));
                }

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