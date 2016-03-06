﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Clinic.Api.Models.AppModels;

namespace Clinic.Api.Models.Context
{
    public class AuthDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            SetRoles(context);

            SetAdministrator(context);

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

    }
}