using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioWatch
{
    [DebuggerDisplay("{Produkt}-{Price}-{Currency}-{Datum}")]
    public class TransactionCsv
    {
        public DateTime Datum { get; set; }
        public String Produkt { get; set; }
        public String Currency { get; set; }
        public decimal Price { get; set; }
        public String ISIN { get; set; }
        public Int32 Anzahl { get; set; }

        public decimal CurrentValue { get; set; }
        public decimal Perf
        {
            get
            {
                if (this.CurrentValue == default(decimal) || this.Price == default(decimal))
                {
                    return 0;
                }

                var r = (CurrentValue - this.Price) / Price;
                return r * 100;
            }
        }

        public decimal PerfMonth
        {
            get
            {
                decimal numDays = (DateTime.Now - this.Datum).Days;
                decimal numMonths = (decimal)(DateTime.Now - this.Datum).Days / (decimal)30;
                decimal perfPerMonth = this.Perf / numMonths;
                return perfPerMonth;
            }
        }
    }

    public class PortfolioRecord
    {
        public String Produkt { get; set; }
        public string ISIN { get; set; }
        public decimal Wert { get; set; }
        public decimal WertEUR { get; set; }
        public String Currency { get; set; }
        public Int32 Anzahl { get; set; }
        public decimal ClosingRate { get; set; }
        ////public decimal WertDecimal {
        ////    get
        ////    {
        ////        if(String.IsNullOrEmpty(this.Wert))
        ////        {
        ////            return default(decimal);
        ////        }

        ////        var r = this.Wert.Replace("\"", String.Empty); ////.Replace(",", ".");
        ////        decimal result;
        ////        if(decimal.TryParse(r, out result))
        ////        {
        ////            return result;
        ////        }

        ////        return default(decimal);
        ////    }
        ////}
    }

    public class PortfolioAggregate
    {
        private Func<decimal> PortfolioValue;
        public PortfolioAggregate(Func<decimal> portfolioValue)
        {
            this.PortfolioValue = portfolioValue;
        }

        public String ISIN { get; set; }
        public String Produkt { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal Perf { get { return (this.CurrentPrice - this.AveragePrice) / this.AveragePrice * 100; } }
        public Int32 Anzahl { get; set; }
        public decimal Value { get { return this.Anzahl * this.CurrentPrice; } }
        public decimal Percent { get { return this.Value / PortfolioValue() * 100; } }
    }
}
