using System.Collections.Generic;
using MeterReading.Models;

namespace MeterReading.Services.Contracts
{
    public interface ICSVProcessingService
    {
        IEnumerable<MeterReadingItem> CSVToModel(string csvAsString);
    }
}
