using PortfolioWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PortfolioWatchTests
{
    public class XUnitTest
    {
        [Fact]
        public void Test()
        {
            TransactionReader reader = new TransactionReader();
            TransactionCsv transaction = reader.ReadTransaction("13-12-2018,19:44,PT TELEKOMUNIKASI INDO,US7156841063,NSY,9,USD,26.6900,USD,-240.2100000,EUR,-211.3127370,1.1368,EUR,-0.53,EUR,-211.8427370");
            Assert.NotNull(transaction);
        }
    } 
}
