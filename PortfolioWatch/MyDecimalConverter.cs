using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioWatch
{
    /// <summary>	/// Converts a <see cref="decimal"/> to and from a <see cref="string"/></summary>
	public class MyDecimalConverter : CsvHelper.TypeConversion.DefaultTypeConverter
    {
        /// <summary>        /// Converts the string to an object.</summary>        /// <param name="text">The string to convert to an object.</param>
        /// <param name="row">The <see cref="IReaderRow"/> for the current record.</param>
        /// <param name="memberMapData">The <see cref="MemberMapData"/> for the member being created.</param>
        /// <returns>The object created from the string.</returns>
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var numberStyle = memberMapData.TypeConverterOptions.NumberStyle ?? NumberStyles.Float;

            decimal d;
            if (decimal.TryParse(text, numberStyle, memberMapData.TypeConverterOptions.CultureInfo, out d))
            {
                return d;
            }

            text = text.Replace(".", ",");
            if (decimal.TryParse(text, numberStyle, memberMapData.TypeConverterOptions.CultureInfo, out d))
            {
                return d;
            }

            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}
