using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Core;

namespace UnitTests.Calendar
{
    [TestFixture]
    public class GetWeekNumberTest
    {
        [Test]
        public void FirstDayOfYearWeekNumberTestFor52()
        {
            Assert.AreEqual(52, skolerute.data.Calendar.GetWeekNumber(new DateTime(2017, 1, 1)));
        }

        [Test]
        public void FirstDayOfYearWeekNumberTestFor53()
        {
            Assert.AreEqual(53, skolerute.data.Calendar.GetWeekNumber(new DateTime(2016,1, 1)));
        }

        [Test]
        public void FirstDayOfYearWeekNumberTestFor1()
        {
            Assert.AreEqual(1, skolerute.data.Calendar.GetWeekNumber(new DateTime(2015, 1, 1)));
        }

        [Test]
        public void LastDayOfYearWeekNumberTestFor52()
        {
            Assert.AreEqual(52, skolerute.data.Calendar.GetWeekNumber(new DateTime(2016, 12, 31)));
        }

        [Test]
        public void LastDayOfYearWeekNumberTestFor53()
        {
            Assert.AreEqual(53, skolerute.data.Calendar.GetWeekNumber(new DateTime(2015, 12, 31)));
        }

        [Test]
        public void MiddleWeekTest()
        {
            Assert.AreEqual(26, skolerute.data.Calendar.GetWeekNumber(new DateTime(2016, 6, 29)));
        }
    }
}
