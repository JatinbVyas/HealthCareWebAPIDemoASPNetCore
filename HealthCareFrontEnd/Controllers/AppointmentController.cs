using HealthCareFrontEnd.Utility;
using HealthCareFrontEnd.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace HealthCareFrontEnd.Controllers
{
    public class AppointmentController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7002/api/");

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _userEmail = string.Empty;

        public AppointmentController(IHttpContextAccessor contextAccessor)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            _contextAccessor = contextAccessor;
            var jwtToken = _contextAccessor.HttpContext.Request.Cookies[ConstantValues.HealthJWTToken];
            var userEmailaddress = _contextAccessor.HttpContext.Request.Cookies[ConstantValues.UserEmail];
            if (!string.IsNullOrEmpty(jwtToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwtToken);
            }
            if (!string.IsNullOrEmpty(userEmailaddress))
            {
                _userEmail = userEmailaddress;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewAppointments()
        {

            if (!string.IsNullOrEmpty(_userEmail))
            {
                ResponceViewModel<AppointmentViewModel> appointmentViewModels = new ResponceViewModel<AppointmentViewModel>();
                HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "appointments/ViewAppointment?userEmail=" + _userEmail + "");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    appointmentViewModels = JsonConvert.DeserializeObject<ResponceViewModel<AppointmentViewModel>>(apiResponse);
                    if (appointmentViewModels.IsSuccess == true)
                    {
                        IEnumerable<AppointmentViewModel> appointmentViews = new List<AppointmentViewModel>();
                        appointmentViews = appointmentViewModels.ResponseList;
                        return View(appointmentViews);
                    }
                    return View(appointmentViewModels);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> BookAppointment()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "appointments/GetAllHealthProfessionals");
            if (response.IsSuccessStatusCode) 
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                IEnumerable<HealthCareProfessinalViewModel> professinalViewModels = Enumerable.Empty<HealthCareProfessinalViewModel>();
                professinalViewModels = JsonConvert.DeserializeObject<IEnumerable<HealthCareProfessinalViewModel>>(apiResponse);

                var healthProffessionalsList = professinalViewModels.Select
            (x => new SelectListItem { Value = x.Id.ToString(), Text = x.NameofHealthCareProfessional }).ToList();

                ViewBag.HealthProffessionalsList = healthProffessionalsList;
                return View();
            }

            ViewBag.healthProffessionalsList = Empty.ToString();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(BookAppointmentViewModel bookAppointmentViewModel)
        {
            if (bookAppointmentViewModel != null)
            {
                bookAppointmentViewModel.UserEmail = _userEmail;
                var bookAppointdataInJson = JsonConvert.SerializeObject(bookAppointmentViewModel);
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "appointments/BookAppointment", new StringContent(bookAppointdataInJson, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode) 
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    ResponceViewModel<AppointmentViewModel> responceViewModel = new ResponceViewModel<AppointmentViewModel>();
                    responceViewModel = JsonConvert.DeserializeObject<ResponceViewModel<AppointmentViewModel>>(apiResponse);
                    if (responceViewModel.IsSuccess)
                    {
                        TempData["SMsg"] = responceViewModel.ReturnMessage;
                    }
                    else
                    {
                        TempData["EMsg"] = responceViewModel.ReturnMessage;
                    }
                    return RedirectToAction("ViewAppointments","Appointment");
                }
            }
            return View(bookAppointmentViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            if (!string.IsNullOrEmpty(_userEmail))
            {
                CancelAppointmentViewModel cancelAppointmentViewModel = new CancelAppointmentViewModel();
                cancelAppointmentViewModel.userEmail = _userEmail;
                cancelAppointmentViewModel.appointmentId = appointmentId;

                var cancelAppointdataInJson = JsonConvert.SerializeObject(cancelAppointmentViewModel);
                HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + "appointments/CancelAppointment", new StringContent(cancelAppointdataInJson, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    ResponceViewModel<AppointmentViewModel> responceViewModel = new ResponceViewModel<AppointmentViewModel>();
                    responceViewModel = JsonConvert.DeserializeObject<ResponceViewModel<AppointmentViewModel>>(apiResponse);
                    if (responceViewModel.IsSuccess)
                    {
                        TempData["SMsg"] = responceViewModel.ReturnMessage;
                    }
                    else
                    {
                        TempData["EMsg"] = responceViewModel.ReturnMessage;
                    }
                    return RedirectToAction("ViewAppointments", "Appointment");
                }
            }
            return RedirectToAction("ViewAppointments", "Appointment");
        }

        [HttpGet]
        public async Task<IActionResult> CompleteAppointment(int appointmentId)
        {
            if (!string.IsNullOrEmpty(_userEmail))
            {
                CancelAppointmentViewModel cancelAppointmentViewModel = new CancelAppointmentViewModel();
                cancelAppointmentViewModel.userEmail = _userEmail;
                cancelAppointmentViewModel.appointmentId = appointmentId;

                var cancelAppointdataInJson = JsonConvert.SerializeObject(cancelAppointmentViewModel);
                HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + "appointments/CompleteAppointment", new StringContent(cancelAppointdataInJson, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    ResponceViewModel<AppointmentViewModel> responceViewModel = new ResponceViewModel<AppointmentViewModel>();
                    responceViewModel = JsonConvert.DeserializeObject<ResponceViewModel<AppointmentViewModel>>(apiResponse);
                    if (responceViewModel.IsSuccess)
                    {
                        TempData["SMsg"] = responceViewModel.ReturnMessage;
                    }
                    else
                    {
                        TempData["EMsg"] = responceViewModel.ReturnMessage;
                    }
                    return RedirectToAction("ViewAppointments", "Appointment");
                }
            }
            return RedirectToAction("ViewAppointments", "Appointment");
        }
    }
}
