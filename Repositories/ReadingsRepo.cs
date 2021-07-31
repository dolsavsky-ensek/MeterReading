using System;
using System.Collections.Generic;
using MeterReading.Models;
using MeterReading.Repositories.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace MeterReading.Repositories
{
    public class ReadingsRepo : IReadingsRepo
    {
        private readonly IMemoryCache cache;

        public ReadingsRepo(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public bool AddReadings(IEnumerable<MeterReadingItem> readings)
        {
            try
            {
                foreach (var reading in readings)
                {
                    MeterReadingItem cacheEntry = null;

                    if (!cache.TryGetValue(reading.AccountID, out cacheEntry))
                    {
                        // Key not in cache, so get data.
                        cacheEntry = reading;

                        // Set cache options.
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            // Keep in cache for this time, reset time if accessed.
                            .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                        // Save data in cache.
                        cache.Set(reading.AccountID, cacheEntry, cacheEntryOptions);
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public MeterReadingItem GetReadingByAccountID(string accountID)
        {
            var reading = cache.Get(accountID);

            return (MeterReadingItem)reading;
        }
    }
}
