
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxConvertTask
{
    public class App
    {
        private readonly IOptions _options;
        private readonly IRatesRepository _ratesRepository;
        private readonly IConvertService _convertService;
        private readonly ILogger<App> _logger;

        public App(IOptions options, IConvertService convertService, IRatesRepository ratesRepository, ILogger<App> logger)
        {
            _options = options;
            _convertService = convertService;
            _ratesRepository = ratesRepository;
            _logger = logger;
        }

        public void Run()
        {
            List<FxTran> values = new List<FxTran>();

            _logger.LogInformation("Opening csv file.");

            //Load from csv, make operations and show on console
            using(var reader = File.OpenText("transactions-v2.csv"))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        var line = reader.ReadLine();
                        var fxLine = FxLine.ParseLine(line, _options.DateFormat, _options.DecimalCulture);
                        FxTran conversion = _convertService.Convert(fxLine);
                        values.Add(conversion);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                    }
                }
            }

            _logger.LogInformation("Ended parsing csv file");

            var grouped = values.GroupBy(x => x.fxTran.CounterCurrency, (k, v) => new { GroupKey = k, Value = v });

            foreach(var item in grouped)
            {
                Console.WriteLine(item.GroupKey + "\t" + item.Value.Sum(t=>t.ConvertedAmount).ToString("#.##"));
            }

            Console.ReadKey();
        }
    }
}
