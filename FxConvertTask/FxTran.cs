using System;
using System.Collections.Generic;
using System.Text;

namespace FxConvertTask
{
    public class FxTran
    {
        public FxLine fxTran;

        public FxTran(FxLine fxLine, decimal convertedAmount)
        {
            this.fxTran = fxLine;
            this.ConvertedAmount = convertedAmount;
        }

        public decimal ConvertedAmount { get; set; }
    }
}
