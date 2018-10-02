using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PortfolioWatch
{
   
   
    public class PortfolioDao
    {
        public CsvReader Create(TextReader reader)
        {
            var csv = new CsvReader(reader);
            MyDecimalConverter decimalConverter = new MyDecimalConverter();
            MyDateTimeConverter dateTimeConverter = new MyDateTimeConverter();
            MyInt32Converter intConverter = new MyInt32Converter();

            csv.Configuration.TypeConverterCache.AddConverter<decimal>(decimalConverter);
            csv.Configuration.TypeConverterCache.AddConverter(typeof(decimal), decimalConverter);

            csv.Configuration.TypeConverterCache.AddConverter<DateTime>(dateTimeConverter);
            csv.Configuration.TypeConverterCache.AddConverter(typeof(DateTime), dateTimeConverter);

            csv.Configuration.TypeConverterCache.AddConverter<Int32>(intConverter);
            csv.Configuration.TypeConverterCache.AddConverter(typeof(Int32), intConverter);

            return csv;

        }
        public object GetData(String transactionsFile, String portfolioFile)
        {
            String transactions = transactionsFile;
            List<TransactionCsv> list = null;
            List<PortfolioRecord> portfolioList = null;

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            FixingReader reader1 = new FixingReader(transactions);
            using (StreamReader reader = new StreamReader(transactions))
            {
                CsvReader csv = Create(reader);    
                list = csv.GetRecords<TransactionCsv>().ToList();
            }

            if (!String.IsNullOrEmpty(portfolioFile) && File.Exists(portfolioFile))
            {
                reader1 = new FixingReader(portfolioFile);
                using (StreamReader reader = new StreamReader(portfolioFile))
                {
                    CsvReader csv = Create(reader);
                    portfolioList = csv.GetRecords<PortfolioRecord>().ToList();
                }

                List<Transaction> finalList = new List<Transaction>();
                foreach (var trans in list)
                {
                    PortfolioRecord match = portfolioList.FirstOrDefault(x => x.ISIN == trans.ISIN);
                    if (match != null)
                    {
                        finalList.Add(new Transaction(trans) { CurrentValue = match.WertDecimal / match.Anzahl });
                    }
                }

                finalList.Sort((x, y) => x.Perf.CompareTo(y.Perf));
                return finalList;
            }
            else
            {
                return list;
            }
        }
    }
}
