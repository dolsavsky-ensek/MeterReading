using System.Collections.Generic;
using MeterReading.Models;

namespace MeterReading.Repositories.Contracts
{
    public interface IReadingsRepo
    {
        bool AddReadings(IEnumerable<MeterReadingItem> readings);
        MeterReadingItem GetReadingByAccountID(string accountID);
    }
}
