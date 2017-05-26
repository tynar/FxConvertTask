using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net.Http;

namespace FxConvertTask
{
    public interface IFixerClient
    {
        FixerIo Get(DateTime valueDate);
    }

    public class FixerClient : IFixerClient
    {
        private readonly IOptions _options;

        public FixerClient(IOptions options)
        {
            _options = options;
        }

        public FixerIo Get(DateTime valueDate)
        {
            using (var client = new HttpClient())
            {
                string uri = string.Format("http://api.fixer.io/{0}?base={1}", valueDate.ToString(_options.DateFormat), _options.BaseCurrency);
                string jsonString = client.GetStringAsync(uri).Result;
                return JsonConvert.DeserializeObject<FixerIo>(jsonString, new IsoDateTimeConverter { DateTimeFormat = _options.DateFormat });
            }
        }
    }
}