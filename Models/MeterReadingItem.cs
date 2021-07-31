using System;

namespace MeterReading.Models
{
    public class MeterReadingItem
    {
        public string AccountID { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
}
