using System.Collections.Generic;
using System.Linq;
using MeterReading.Models;
using MeterReading.Services.Contracts;

namespace MeterReading.Services
{
    public class CSVProcessingService : ICSVProcessingService
    {
        public IEnumerable<MeterReadingItem> CSVToModel(string csvAsString)
        {
            var csvLines = csvAsString.Split("\r\n");
            var result = new List<MeterReadingItem>();
            var headers = csvLines[0].Split(',');

            // remove empty lines from csv
            csvLines = csvLines.Where(x => x != "").ToArray();
            
            for (int i = 1; i < csvLines.Length; i++)
            {
                var values = csvLines[i].Split(',');

                var obj = new MeterReadingItem
                {
                    AccountID = values[0],
                    MeterReadingDateTime = System.DateTime.Parse(values[1]),
                    MeterReadValue = values[2]
                };
                
                result.Add(obj);
            }

            return result;
        }
    }
}
