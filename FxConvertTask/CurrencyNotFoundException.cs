using System;
using System.Collections.Generic;
using System.Text;

namespace FxConvertTask
{
    public class CurrencyNotFoundException : Exception
    {
        public string Currency { get; set; }
        public DateTime RateDate { get; set; }

        public CurrencyNotFoundException(string currency, DateTime rateDate, string msg) : base(msg)
        {
            Currency = currency;
            RateDate = rateDate;
        }

        public override string Message
        {
            get
            {
                return base.Message + " Currency = " + Currency + " RateDate = " + RateDate;
            }
        }
    }
}
