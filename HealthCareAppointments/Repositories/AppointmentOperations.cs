using HealthCareAppointments.DBContext;
using HealthCareAppointments.Entities;
using HealthCareAppointments.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthCareAppointments.Repositories
{
    public class AppointmentOperations : IAppointment
    {
        private readonly HealthCareContext _healthCareContext;
        private readonly UserManager<ApplicationUsers> _userManager;

        public AppointmentOperations(HealthCareContext healthCareContext, UserManager<ApplicationUsers> userManager)
        {
            _healthCareContext = healthCareContext ?? throw new ArgumentNullException(nameof(healthCareContext));
            _userManager = userManager;
        }
        public ResponceModel<AppointmentModel> BookAppointment(BookAppointmentModel appointmentdata)
        {
            ResponceModel<AppointmentModel> responceModel = new ResponceModel<AppointmentModel>();
            try
            {
                if (appointmentdata != null)
                {
                    appointmentdata.AppointmentEndTime = appointmentdata.AppointmentStartTime.Value.AddMinutes(30);

                    ApplicationUsers? applicationUsers = GetUser(appointmentdata.UserEmail);
                    if (applicationUsers.Id != null)
                    {
                        List<Appointment> allAppointmentforUser = _healthCareContext.Appointments.Where(a => a.UserId == applicationUsers.Id
                                                                    && appointmentdata.AppointmentStartTime.Value.Date == a.AppointmentStartTime.Value.Date
                                                                    && a.HealthCareProfessionalId == appointmentdata.HealthCareProfessionalId
                                                                    && a.AppointmentStatus == "Booked").ToList();
                        if (allAppointmentforUser.Count > 0)
                        {
                            foreach (Appointment exisitingAppointment in allAppointmentforUser)
                            {
                                if (exisitingAppointment.AppointmentStartTime <= appointmentdata.AppointmentStartTime && exisitingAppointment.AppointmentEndTime >= appointmentdata.AppointmentStartTime)
                                {
                                    responceModel.IsSuccess = false;
                                    responceModel.ReturnMessage = "Appoinment can not be book, there is already booking on this slot.";

                                    return responceModel;
                                }
                                else if (exisitingAppointment.AppointmentStartTime <= appointmentdata.AppointmentEndTime && exisitingAppointment.AppointmentEndTime >= appointmentdata.AppointmentEndTime)
                                {
                                    responceModel.IsSuccess = false;
                                    responceModel.ReturnMessage = "Appoinment can not be book, there is already booking on this slot.";

                                    return responceModel;
                                }
                            }
                        }
                        Appointment appointment = new Appointment
                        {
                            HealthCareProfessionalId = appointmentdata.HealthCareProfessionalId,
                            UserId = applicationUsers.Id,
                            AppointmentStartTime = appointmentdata.AppointmentStartTime,
                            AppointmentEndTime = appointmentdata.AppointmentEndTime,
                            AppointmentStatus = "Booked"
                        };

                        _healthCareContext.Appointments.Add(appointment);
                        _healthCareContext.SaveChanges();

                        responceModel.IsSuccess = true;
                        responceModel.ReturnMessage = "Appoinment booked successuflly.";

                        return responceModel;
                    }
                }

                responceModel.IsSuccess = false;
                responceModel.ReturnMessage = "Appoinment not booked.";

                return responceModel;
            }
            catch (Exception ex)
            {
                responceModel.IsSuccess = false;
                responceModel.ReturnMessage = ex.Message;

                return responceModel;
            }
        }

        public ResponceModel<AppointmentModel> CancelAppointment(string userEmail, int appointmentId)
        {
            ResponceModel<AppointmentModel> responceModel = new ResponceModel<AppointmentModel>();
            try
            {
                if (!string.IsNullOrEmpty(userEmail))
                {
                    ApplicationUsers? userId = GetUser(userEmail);
                    if (userId != null)
                    {
                        Appointment appointment = _healthCareContext.Appointments.Where(a => a.UserId == userId.Id && a.AppointmentId == appointmentId).FirstOrDefault();

                        if (appointment.AppointmentId != null)
                        {
                            // Check first can we cancel it or not?
                            if (appointment.AppointmentStartTime.Value.AddHours(-24) <= DateTime.Now)
                            {
                                responceModel.IsSuccess = false;
                                responceModel.ReturnMessage = "Appointment can not be cancel within 24 hours.";

                                return responceModel;
                            }
                            appointment.AppointmentStatus = "Cancelled";

                            _healthCareContext.Appointments.Update(appointment);
                            _healthCareContext.SaveChanges();

                            responceModel.IsSuccess = true;
                            responceModel.ReturnMessage = "Appointment cancelled successfully.";

                            return responceModel;
                        }

                        responceModel.IsSuccess = false;
                        responceModel.ReturnMessage = "Appointment not found.";

                        return responceModel;
                    }
                }
                responceModel.IsSuccess = false;
                responceModel.ReturnMessage = "User not found.";

                return responceModel;
            }
            catch (Exception ex) 
            {
                responceModel.IsSuccess = false;
                responceModel.ReturnMessage = ex.Message;

                return responceModel;
            }
        }

        private ApplicationUsers? GetUser(string userEmail)
        {
            return _healthCareContext.Users.Where(u => u.UserName == userEmail).FirstOrDefault();
        }

        public IEnumerable<HealthCareProfessional> ListAllHealthProfessionals()
        {
            return _healthCareContext.HealthCareProfessionals.OrderBy(h => h.NameofHealthCareProfessional);
        }

        public ResponceModel<AppointmentModel> ViewAllAppointment(string userEmail)
        {
            ResponceModel<AppointmentModel> responceModel = new ResponceModel<AppointmentModel>();
            try
            {
                IEnumerable<AppointmentModel> appointmentModels = Enumerable.Empty<AppointmentModel>();
                if (!string.IsNullOrEmpty(userEmail))
                {
                    ApplicationUsers? userId = GetUser(userEmail);
                    if (userId != null)
                    {
                        appointmentModels = _healthCareContext.Appointments.Where(a => a.UserId == userId.Id).Select(d =>
                        new AppointmentModel
                        {
                            AppointmentId = d.AppointmentId,
                            HealthCareProfessionalName = d.HealthCareProfessional.NameofHealthCareProfessional,
                            AppointmentStartTime = d.AppointmentStartTime,
                            AppointmentEndTime = d.AppointmentEndTime,
                            AppointmentStatus = d.AppointmentStatus
                        });

                        if (appointmentModels.Any())
                        {
                            responceModel.ResponseList = appointmentModels;
                            responceModel.IsSuccess = true;
                            responceModel.ReturnMessage = "Appoinment found for the user.";

                            return responceModel;
                        }
                        else
                        {
                            responceModel.ResponseList = null;
                            responceModel.IsSuccess = false;
                            responceModel.ReturnMessage = "Appoinment not found for the user.";

                            return responceModel;
                        }
                    }

                    responceModel.ResponseList = null;
                    responceModel.IsSuccess = false;
                    responceModel.ReturnMessage = "User data not found.";

                    return responceModel;
                }

                responceModel.ResponseList = null;
                responceModel.IsSuccess = false;
                responceModel.ReturnMessage = "User not found.";

                return responceModel;
            }
            catch (Exception ex) 
            {
                responceModel.IsSuccess = false;
                responceModel.ReturnMessage = ex.Message;

                return responceModel;
            }
        }

        public ResponceModel<AppointmentModel> CompleteAppointment(string userEmail, int appointmentId)
        {
            ResponceModel<AppointmentModel> responceModel = new ResponceModel<AppointmentModel>();
            try
            {
                if (!string.IsNullOrEmpty(userEmail))
                {
                    ApplicationUsers? applicationUser = GetUser(userEmail);
                    if (applicationUser != null)
                    {
                        Appointment appointment = _healthCareContext.Appointments.Where(a => a.UserId == applicationUser.Id && a.AppointmentId == appointmentId).FirstOrDefault();

                        if (appointment.AppointmentId != null)
                        {
                            if (appointment.AppointmentStartTime <= DateTime.Now)
                            {
                                appointment.AppointmentStatus = "Completed";

                                _healthCareContext.Appointments.Update(appointment);
                                _healthCareContext.SaveChanges();

                                responceModel.IsSuccess = true;
                                responceModel.ReturnMessage = "Appointment marked as completed successfully.";

                                return responceModel;
                            }
                            else
                            {
                                responceModel.IsSuccess = false;
                                responceModel.ReturnMessage = "Future appointment can not be marked as completed.";

                                return responceModel;
                            }
                        }

                        responceModel.IsSuccess = false;
                        responceModel.ReturnMessage = "Appointment not found.";

                        return responceModel;
                    }
                }

                responceModel.IsSuccess = false;
                responceModel.ReturnMessage = "Appointment not found for the user.";

                return responceModel;
            }
            catch (Exception ex)
            {
                responceModel.IsSuccess = false;
                responceModel.ReturnMessage = ex.Message;

                return responceModel;
            }
        }
    }
}
