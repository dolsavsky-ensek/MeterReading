using System.Collections.Generic;
using MeterReading.Helpers;
using MeterReading.Models;
using MeterReading.Services.Contracts;

namespace MeterReading.Services
{
    public class ValidationService : IValidationService
    {
        public ReadingsResult ValidateReadings(IEnumerable<MeterReadingItem> readings)
        {
            var result = new ReadingsResult();
            var validEntries = new List<string>();
            foreach (var reading in readings)
            {
                if(reading.MeterReadValue.IsDigitsOnly() 
                    && reading.MeterReadValue.Length == 5
                    && !validEntries.Contains(reading.AccountID))
                {
                    result.ValidResults.Add(reading);
                    validEntries.Add(reading.AccountID);
                }
                else
                {
                    result.InvalidResults.Add(reading);
                }
            }

            return result;
        }
    }
}
