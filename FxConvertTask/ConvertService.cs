namespace FxConvertTask
{
    public interface IConvertService
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
            decimal exchangeRate = _ratesRepository.GetRate(fxLine.ValueDate, fxLine.BaseCurrency, fxLine.CounterCurrency);
            return new FxTran(fxLine, exchangeRate * fxLine.Amount);
        }
    }
}
