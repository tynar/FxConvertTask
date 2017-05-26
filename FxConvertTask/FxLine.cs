using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FxConvertTask
{
    public class FxLine
    {
        public DateTime TradeDate { get; set; }
        public String BaseCurrency { get; set; }
        public String CounterCurrency { get; set; }
        public Decimal Amount { get; set; }
        public DateTime ValueDate { get; set; }

        public static FxLine ParseLine(string line, string dateFormat, CultureInfo decimalCulture)
        {
            var splitted = line.Split(',');

            return new FxLine()
            {
                TradeDate = DateTime.ParseExact(splitted[0], dateFormat, CultureInfo.InvariantCulture),
                BaseCurrency = splitted[1],
                CounterCurrency = splitted[2],
                Amount = Convert.ToDecimal(splitted[3], decimalCulture),
                ValueDate = DateTime.ParseExact(splitted[4], dateFormat, CultureInfo.InvariantCulture)
            };
        }
    }
}
