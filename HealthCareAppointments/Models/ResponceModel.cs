using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCareAppointments.Models
{
    public class ResponceModel<T>
    {
        public string? ReturnMessage { get ; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<T> ResponseList { get; set; }
    }
}
