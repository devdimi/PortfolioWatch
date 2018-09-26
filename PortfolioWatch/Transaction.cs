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

                var r = this.Wert.Replace("\"", String.Empty).Replace(",", ".");
                decimal result;
                if(decimal.TryParse(r, out result))
                {
                    return result;
                }

                return default(decimal);
            }
        }
    }
}
