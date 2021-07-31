using System.Linq;
using MeterReading.Models;
using MeterReading.Repositories.Contracts;
using MeterReading.Services.Contracts;

namespace MeterReading.Services
{
    public class ReadingsService : IReadingsService
    {
        private readonly ICSVProcessingService csvProcessing;
        private readonly IValidationService validationService;
        private readonly IReadingsRepo readingsRepo;
        public ReadingsService(ICSVProcessingService csvProcessing, IValidationService validationService, IReadingsRepo readingsRepo)
        {
            this.csvProcessing = csvProcessing;
            this.validationService = validationService;
            this.readingsRepo = readingsRepo;
        }

        public ReadingsResult ProcesReadings(string csvAsString)
        {
            var readings = csvProcessing.CSVToModel(csvAsString);

            if(readings.ToList().Count() == 0)
            {
                return null;
            }

            var validatedReadings = validationService.ValidateReadings(readings);

            readingsRepo.AddReadings(validatedReadings.ValidResults);

            return validatedReadings;
        }
    }
}
