using System;
using System.Collections.Generic;
using System.Text;

namespace FxConvertTask
{
    public interface IConvertService : IDisposable
    {
        FxTran Convert(FxLine fxTran);
    }
    public class ConvertService : IConvertService
    {
        private readonly IOptions _options;
        private readonly IRatesRepository _ratesRepository;

        public ConvertService(IOptions options, IRatesRepository ratesRepository)
        {
            _ratesRepository = ratesRepository;
            _options = options;
        }

        public FxTran Convert(FxLine fxLine)
        {
            FxTran returnVal = null;
            decimal exchangeRate = _ratesRepository.GetRate(fxLine.ValueDate, fxLine.BaseCurrency, fxLine.CounterCurrency);

            returnVal = new FxTran(fxLine, exchangeRate * fxLine.Amount);
            return returnVal;
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _ratesRepository.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

}
