using Microsoft.AspNetCore.Identity;

namespace HealthCareAppointments.Entities
{
    public class ApplicationUsers : IdentityUser
    {
        public string NameofUser { get; set; }
    }
}
