using System;
using System.Collections.Generic;
using System.Text;

namespace FxConvertTask
{
    public class FxTran
    {
        public FxLine fxTran;
        public decimal ConvertedAmount { get; set; }

        public FxTran(FxLine fxLine, decimal convertedAmount)
        {
            this.fxTran = fxLine;
            this.ConvertedAmount = convertedAmount;
        }
    }
}
