namespace HealthCareAppointments.Models
{
    public class AppointmentModel
    {
        public int AppointmentId { get; set; }

        public string?  Username { get; set; }

        public string? HealthCareProfessionalName { get; set; }

        public DateTime? AppointmentStartTime { get; set; }

        public DateTime? AppointmentEndTime { get; set; }

        public string? AppointmentStatus { get; set; }

        public string? ReturnMessage { get; set; }

        public bool IsSuccess { get; set; }
    }
}
