using System.Collections.Generic;
using MeterReading.Models;

namespace MeterReading.Services.Contracts
{
    public interface IValidationService
    {
        ReadingsResult ValidateReadings(IEnumerable<MeterReadingItem> readings);
    }
}
