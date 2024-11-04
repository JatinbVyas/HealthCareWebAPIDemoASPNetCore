using HealthCareAppointments.Entities;
using HealthCareAppointments.Models;

namespace HealthCareAppointments.Repositories
{
    public interface IAppointment
    {
        ResponceModel<AppointmentModel> BookAppointment(BookAppointmentModel appointmentdata);

        ResponceModel<AppointmentModel> ViewAllAppointment(string userEmail);

        IEnumerable<HealthCareProfessional> ListAllHealthProfessionals();

        ResponceModel<AppointmentModel> CancelAppointment(string userEmail, int appointmentId);

        ResponceModel<AppointmentModel> CompleteAppointment(string userEmail, int appointmentId);
    }
}
