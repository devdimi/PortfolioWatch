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
    }

    public class Transaction : TransactionCsv
    {
        public Transaction(TransactionCsv transactionCsv)
        {
            this.Datum = transactionCsv.Datum;
            this.Produkt = transactionCsv.Produkt;
            this.Currency = transactionCsv.Currency;
            this.Price = transactionCsv.Price;
            this.ISIN = transactionCsv.ISIN;
            this.Anzahl = transactionCsv.Anzahl;
        }

        public decimal CurrentValue { get; set; }
        
        public decimal Perf
        {
            get
            {
                if(this.CurrentValue == default(decimal) || this.Price == default(decimal))
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
                decimal numMonths = (decimal)(DateTime.Now - this.Datum).Days / (decimal)30;
                decimal perfPerMonth = this.Perf / numMonths;
                return perfPerMonth;
            }
        }
    }

    public class PortfolioRecord
    {
        public string ISIN { get; set; }
        public String Wert { get; set; }
        public String Currency { get; set; }
        public Int32 Anzahl { get; set; }
        public decimal WertDecimal {
            get
            {
                if(String.IsNullOrEmpty(this.Wert))
                {
                    return default(decimal);
                }

                var r = this.Wert.Replace("\"", String.Empty); ////.Replace(",", ".");
                decimal result;
                if(decimal.TryParse(r, out result))
                {
                    return result;
                }

                return default(decimal);
            }
        }
    }

    public class PortfolioAggregate
    {
        public String ISIN { get; set; }
        public String Produkt { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal Perf { get; set; }
    }
}
