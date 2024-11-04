using HealthCareAppointments.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthCareAppointments.DBContext
{
    public class HealthCareContext : IdentityDbContext<ApplicationUsers>
    {
        public HealthCareContext(DbContextOptions<HealthCareContext> options) : base(options) { }

        public DbSet<HealthCareProfessional> HealthCareProfessionals { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
    }
}
