using HealthCareFrontEnd.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using HealthCareFrontEnd.Utility;

namespace HealthCareFrontEnd.Controllers
{
    public class AccountController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7002/api/");

        private readonly HttpClient _httpClient;

        public AccountController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "Your Oauth token");

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var registerdataInJson = JsonConvert.SerializeObject(registerViewModel);
            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "accounts/Register", new StringContent(registerdataInJson, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) 
        {
            if (loginViewModel != null) 
            {
                var logindataInJson = JsonConvert.SerializeObject(loginViewModel);
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "accounts/Login", new StringContent(logindataInJson, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode) 
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    Response.Cookies.Append(
                        ConstantValues.HealthJWTToken,
                        apiResponse,
                        new CookieOptions 
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict
                        });

                    Response.Cookies.Append(
                        ConstantValues.UserEmail,
                        loginViewModel.Email,
                        new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict
                        });
                    return RedirectToAction("ViewAppointments", "Appointment");
                }
            }

            return View();
        }
    }
}
