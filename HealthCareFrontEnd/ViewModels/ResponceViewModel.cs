namespace HealthCareFrontEnd.ViewModels
{
    public class ResponceViewModel<T>
    {
        public string? ReturnMessage { get; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<T> ResponseList { get; set; }
    }
}
