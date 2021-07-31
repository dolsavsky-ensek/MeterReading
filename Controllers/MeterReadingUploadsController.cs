using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MeterReading.Models;
using MeterReading.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MeterReading.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeterReadingUploadsController : ControllerBase
    {
        private readonly ILogger<MeterReadingUploadsController> logger;
        private readonly IReadingsService readingService;

        public MeterReadingUploadsController(IReadingsService readingService, ILogger<MeterReadingUploadsController> logger)
        {
            this.readingService = readingService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                string csvAsString = string.Empty;
                using (var reader = new StreamReader(Request.Body))
                {
                    csvAsString = await reader.ReadToEndAsync();
                }

                if (string.IsNullOrWhiteSpace(csvAsString))
                {
                    throw new Exception("No file was provided.");
                }

                var readingResults = readingService.ProcesReadings(csvAsString);

                if(readingResults == null)
                {
                    throw new Exception("File contains no data");
                }

                return Ok
                (
                    new APIResponse { 
                        ResponseStatusCode = ResponseStatusCode.Success, 
                        Data = new
                        {
                            ValidReadings = readingResults.ValidResults.ToList().Count,
                            InvalidReadings = readingResults.InvalidResults.ToList().Count
                        }
                    }
                );
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new APIResponse { ResponseStatusCode = ResponseStatusCode.Error, Message = $"Failed to process readings. :: {ex.Message}"});
            }

        }
    }
}


