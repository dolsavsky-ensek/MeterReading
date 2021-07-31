using MeterReading.Models;

namespace MeterReading.Services.Contracts
{
    public interface IReadingsService
    {
        ReadingsResult ProcesReadings(string csvAsString);
    }
}
