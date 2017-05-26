using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;

namespace FxConvertTask
{
    public interface IRatesRepository : IDisposable
    {
        decimal GetRate(DateTime valueDate, string baseCurrency, string counterCurrency);
    }

    public class RatesRepository : IRatesRepository
    {
        private readonly IFixerClient _fixerClient;
        private readonly IOptions _options;

        public RatesRepository(IFixerClient fixerClient, IOptions options)
        {
            _fixerClient = fixerClient;
            _options = options;
        }

        private Dictionary<DateTime, FixerIo> _rateCache = new Dictionary<DateTime, FixerIo>();
        public decimal GetRate(DateTime valueDate, string baseCurrency, string counterCurrency)
        {
            if (baseCurrency == counterCurrency) return 1;

            FixerIo fi = null;
            if (_rateCache.ContainsKey(valueDate))
            {
                fi = _rateCache[valueDate];
            }
            else {
                fi = _fixerClient.Get(valueDate);
                _rateCache.Add(valueDate, fi);
            }

            //EUR -> JPY
            if (baseCurrency == _options.BaseCurrency)
            {
                if (!CurrencyExistsInCache(counterCurrency, valueDate))
                {
                    throw new CurrencyNotFoundException(counterCurrency, valueDate, "Currency cannot be found.");
                }
                return _rateCache[valueDate].Rates[counterCurrency];
            }

            //JPY -> EUR
            else if (counterCurrency == _options.BaseCurrency)
            {
                if (!CurrencyExistsInCache(baseCurrency, valueDate))
                {
                    throw new CurrencyNotFoundException(baseCurrency, valueDate, "Currency cannot be found.");
                }
                return 1 / _rateCache[valueDate].Rates[baseCurrency];
            }

            //USD -> CHF
            if (!CurrencyExistsInCache(baseCurrency, valueDate))
            {
                throw new CurrencyNotFoundException(baseCurrency, valueDate, "Currency cannot be found.");
            }
            if (!CurrencyExistsInCache(counterCurrency, valueDate))
            {
                throw new CurrencyNotFoundException(counterCurrency, valueDate, "Currency cannot be found.");
            }
            return _rateCache[valueDate].Rates[counterCurrency] / _rateCache[valueDate].Rates[baseCurrency];
        }

        private bool CurrencyExistsInCache(string currency, DateTime date)
        {
            return _rateCache[date].Rates.ContainsKey(currency);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _fixerClient.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

    public class FixerIo
    {
        [JsonProperty("base")]
        public string BaseCurrency { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
