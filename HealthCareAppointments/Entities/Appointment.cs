using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCareAppointments.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUsers? ApplicationUsers { get; set; }

        public string UserId { get; set; }

        public HealthCareProfessional? HealthCareProfessional { get; set; }

        [ForeignKey("HealthCareProfessionalId")]
        public int HealthCareProfessionalId { get; set; }

        public DateTime? AppointmentStartTime { get; set; }

        public DateTime? AppointmentEndTime { get; set; }

        public string? AppointmentStatus { get; set; }
    }
}
