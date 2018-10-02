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
    /// <summary> Converts a <see cref="DateTime"/> to and from a <see cref="string"/>.</summary>
	public class MyDateTimeConverter : CsvHelper.TypeConversion.DefaultTypeConverter
    {
        /// <summary>
        /// Converts the string to an object.
        /// </summary>
        /// <param name="text">The string to convert to an object.</param>
        /// <param name="row">The <see cref="IReaderRow"/> for the current record.</param>
        /// <param name="memberMapData">The <see cref="MemberMapData"/> for the member being created.</param>
        /// <returns>The object created from the string.</returns>
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text == null) { return base.ConvertFromString(null, row, memberMapData); }

            var formatProvider = (IFormatProvider)memberMapData.TypeConverterOptions.CultureInfo.GetFormat(typeof(DateTimeFormatInfo)) ?? memberMapData.TypeConverterOptions.CultureInfo;
            var dateTimeStyle = memberMapData.TypeConverterOptions.DateTimeStyle ?? DateTimeStyles.None;

            if (memberMapData.TypeConverterOptions.Formats == null || memberMapData.TypeConverterOptions.Formats.Length == 0)
            {
                DateTime dateTime;
                if (DateTime.TryParse(text, formatProvider, dateTimeStyle, out dateTime))
                {
                    return dateTime;
                }

                if (DateTime.TryParse(text, new CultureInfo("de-DE"), dateTimeStyle, out dateTime))
                {
                    return dateTime;
                }
            }


            return DateTime.ParseExact(text, memberMapData.TypeConverterOptions.Formats, formatProvider, dateTimeStyle);
        }
    }



}
