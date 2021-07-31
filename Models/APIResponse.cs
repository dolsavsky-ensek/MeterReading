
namespace MeterReading.Models
{
    public class APIResponse
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public ResponseStatusCode ResponseStatusCode { get; set; }
    }
}
