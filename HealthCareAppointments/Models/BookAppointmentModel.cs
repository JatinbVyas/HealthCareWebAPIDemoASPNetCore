namespace HealthCareAppointments.Models
{
    public class BookAppointmentModel
    {
        public int AppointmentId { get; set; }
        public string UserEmail { get; set; }
        public int HealthCareProfessionalId { get; set; }

        public DateTime? AppointmentStartTime { get; set; }

        public DateTime? AppointmentEndTime { get; set; }

        public string? AppointmentStatus { get; set; }
    }
}
