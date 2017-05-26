using System.Globalization;

namespace FxConvertTask
{
    public interface IOptions
    {
        string BaseCurrency { get; }
        string DateFormat { get; }
        CultureInfo DecimalCulture { get; }
    }

    public class Options : IOptions
    {
        public string BaseCurrency
        {
            get { return "EUR"; }
        }

        public string DateFormat
        {
            get {return "yyyy-MM-dd"; }
        }

        public CultureInfo DecimalCulture
        {
            get { return new CultureInfo("en-US"); }
        }
    }
}
