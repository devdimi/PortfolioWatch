using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioWatch
{
    public class FixingReader
    {
        public FixingReader(String file)
        {
            String firstLine;
            String newFile = file + ".fixed";
            File.Copy(file, file + ".back", overwrite: true);
            using (StreamReader reader = new StreamReader(file))
            using (StreamWriter writer = new StreamWriter(newFile))
            {
                firstLine = reader.ReadLine();
                firstLine = FixLine(firstLine);

                writer.WriteLine(firstLine);

                while ((firstLine = reader.ReadLine()) != null)
                {
                    ////firstLine = firstLine.Replace(",", ";").Replace(".", ",");
                    if(firstLine.Contains("CASH FUND"))
                    {
                        continue;
                    }

                    writer.WriteLine(firstLine);
                }
            }

            File.Copy(newFile, file, overwrite: true);
            File.Delete(newFile);
        }

        public String FixLine(String line)
        {
            line = line
                .Replace("Symbol/ISIN", "ISIN")
                .Replace("Wert,,Wert in EUR", "Currency,Wert,Wert in EUR")
                .Replace(",Kurs ,,Wert in Lokalwährung,,Wert,,", ",Currency,Price,Wert in Lokalwährung,,Wert,,")
                .Replace("Schlußkurs", "ClosingRate");


            return line;
        }
    }

}
