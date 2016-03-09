using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Clinic.Api.Models.AppModels;
using System;

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
            SetAnyWeek(context);

            base.Seed(context);
        }

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
            public const string PASSWORD = "Qwerty6";
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
                UserName = ADMIN.USERNAME
            };

            var result = userManager.Create(administrator, ADMIN.PASSWORD);

            // Если администратор добавлен, то добавляем ему роль Администратора
            if (result.Succeeded)
            {
                userManager.AddToRole(administrator.Id, ROLES.ADMINISTRATOR);
            }

        }



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
            for (int i = 0; i < 7 * 3; i++)
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