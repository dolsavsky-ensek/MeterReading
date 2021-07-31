using System.Collections.Generic;

namespace MeterReading.Models
{
    public class ReadingsResult
    {
        public ReadingsResult()
        {
            ValidResults = new List<MeterReadingItem>();
            InvalidResults = new List<MeterReadingItem>();
        }
        public List<MeterReadingItem> ValidResults { get; set; }
        public List<MeterReadingItem> InvalidResults { get; set; }
    }
}
