using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCareFrontEnd.ViewModels
{
    public class BookAppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public string UserEmail { get; set; }

        [Required]
        public int HealthCareProfessionalId { get; set; }

        [Required]
        public DateTime? AppointmentStartTime { get; set; }

        public DateTime? AppointmentEndTime { get; set; }

        public string? AppointmentStatus { get; set; }


    }
}
