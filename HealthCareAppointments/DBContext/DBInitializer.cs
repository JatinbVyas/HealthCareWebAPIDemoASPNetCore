using HealthCareAppointments.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HealthCareAppointments.DBContext
{
    public static class DBInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            HealthCareContext context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<HealthCareContext>();

            if (!context.HealthCareProfessionals.Any())
            {
                context.AddRange
                (
                    new HealthCareProfessional { NameofHealthCareProfessional = "Dr. Ashutosh", Specialty = "General Physician" },
                    new HealthCareProfessional { NameofHealthCareProfessional = "Dr. Munshi", Specialty = "MD Physio" },
                    new HealthCareProfessional { NameofHealthCareProfessional = "Dr. Kale", Specialty = "General Physician" },
                    new HealthCareProfessional { NameofHealthCareProfessional = "Dr. Gorpode", Specialty = "Eye surgeon" },
                    new HealthCareProfessional { NameofHealthCareProfessional = "Dr. Tope", Specialty = "Heart surgeon" }
                );
            }

            var user = new ApplicationUsers
            {
                NameofUser = "Default User",
                Email = "default@example.com",
                NormalizedEmail = "XXXX@EXAMPLE.COM",
                UserName = "default@example.com",
                NormalizedUserName = "OWNER",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUsers>();
                var hashed = password.HashPassword(user, "secret");
                user.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUsers>(context);
                var result = userStore.CreateAsync(user);
            }

            

            context.SaveChanges();
        }
    }
}
