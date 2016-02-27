using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Clinic.Api.Models
{
    public class AppDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        //пароль администратора
        private const string ADMINPASSWORD = "Qwerty6";

        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Создаем 3 роли (
            // Администратор
            // Врач
            // Зарегистрированный клиент
            var administratorRole = new IdentityRole("Administrator");
            var doctorRole = new IdentityRole("Doctor");
            var clientRole = new IdentityRole("Client");

            //Добавляем в БД
            roleManager.Create(administratorRole);
            roleManager.Create(doctorRole);
            roleManager.Create(clientRole);

            // Добавляем Админа (1 на весь проект)
            var administrator = new ApplicationUser()
            {
                Email = "admin@HealthyPet.com",
                UserName = "Admin"
            };

            
            var result = userManager.Create(administrator, ADMINPASSWORD);

            // Если администратор добавлен, то добавляем ему роль Администратора
            if (result.Succeeded)
            {
                userManager.AddToRole(administrator.Id, administratorRole.Name);
            }


            base.Seed(context);
        }

    }
}