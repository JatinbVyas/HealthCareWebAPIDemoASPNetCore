using HealthCareAppointments.Entities;
using HealthCareAppointments.Models;
using HealthCareAppointments.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAppointments.Controllers
{
    [Route("api/appointments/[action]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointment _appointmentoperations;

        public AppointmentController(IAppointment appointmentoperations)
        {
            _appointmentoperations = appointmentoperations ?? throw new ArgumentNullException(nameof(appointmentoperations));
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(BookAppointmentModel appointmentdata)
        {
            if (appointmentdata != null)
            {
                var responceModel = _appointmentoperations.BookAppointment(appointmentdata);
                return Ok(responceModel);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult ViewAppointment(string userEmail)
        {
            if (userEmail != null)
            {
                var appointmentData = _appointmentoperations.ViewAllAppointment(userEmail);
                return Ok(appointmentData);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetAllHealthProfessionals()
        {
            var healthProfessionals = _appointmentoperations.ListAllHealthProfessionals();
            return Ok(healthProfessionals);
        }

        [HttpPut]
        public IActionResult CancelAppointment(CancelAppointmentModel cancelAppointmentModel) 
        {
            string userEmail = cancelAppointmentModel.userEmail;
            int appointmentId = cancelAppointmentModel.appointmentId;
            if (!string.IsNullOrEmpty(userEmail))
            {
                var cancelappointment = _appointmentoperations.CancelAppointment(userEmail, appointmentId);
                return Ok(cancelappointment);
            }
            return BadRequest();
        }

        [HttpPut]
        public IActionResult CompleteAppointment(CancelAppointmentModel cancelAppointmentModel)
        {
            string userEmail = cancelAppointmentModel.userEmail;
            int appointmentId = cancelAppointmentModel.appointmentId;
            if (!string.IsNullOrEmpty(userEmail))
            {
                var completeappointment = _appointmentoperations.CompleteAppointment(userEmail, appointmentId);
                return Ok(completeappointment);
            }
            return BadRequest();
        }

    }
}
