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
            List<TransactionCsv> list = new List<TransactionCsv>();
            List<PortfolioRecord> portfolioList = new List<PortfolioRecord>();

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");

            using (StreamReader reader = new StreamReader(transactions))
            {
                ////CsvReader csv = Create(reader);
                TransactionReader transactionReader = new TransactionReader();
                string line;
                reader.ReadLine(); // skip header
                while (null != (line = reader.ReadLine()))
                {
                    TransactionCsv tr = transactionReader.ReadTransaction(line);
                    list.Add(tr);
                }

                ////list = csv.GetRecords<TransactionCsv>().ToList();
            }

            if (!String.IsNullOrEmpty(portfolioFile) && File.Exists(portfolioFile))
            {
                ////reader1 = new FixingReader(portfolioFile);
                using (StreamReader reader = new StreamReader(portfolioFile))
                {
                    PortfolioReader porfolioReader = new PortfolioReader();
                    string line;
                    reader.ReadLine(); // skip header
                    while (null != (line = reader.ReadLine()))
                    {
                        PortfolioRecord record = porfolioReader.Read(line);
                        portfolioList.Add(record);
                    }
                    
                }

                List<TransactionCsv> finalList = new List<TransactionCsv>();
                foreach (var trans in list)
                {
                    PortfolioRecord match = portfolioList.FirstOrDefault(x => x.ISIN == trans.ISIN);
                    if (match != null)
                    {
                        trans.CurrentValue = match.Wert / match.Anzahl;
                        finalList.Add(trans);
                    }
                }

                finalList.Sort((x, y) => y.Perf.CompareTo(x.Perf));
                return finalList;
            }
            else
            {
                return list;
            }
        }
    }
}
