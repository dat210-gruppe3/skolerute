using System;
using NUnit.Framework;

namespace UnitTests.Calendar
{
    [TestFixture]
    public class GetFirstRelevantDateTimeTest
    {
        [Test]
        public void TestAugust()
        {
            Assert.AreEqual(new DateTime(2016, 8, 1).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2016, 8, 6)).DayOfYear);
        }

        [Test]
        public void TestSeptember()
        {
            Assert.AreEqual(new DateTime(2016, 8, 29).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2016, 9, 2)).DayOfYear);
        }

        [Test]
        public void TestOctober()
        {
            Assert.AreEqual(new DateTime(2016, 9, 26).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2016, 10, 15)).DayOfYear);
        }

        [Test]
        public void TestNovember()
        {
            Assert.AreEqual(new DateTime(2016, 10, 31).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2016, 11, 12)).DayOfYear);
        }

        [Test]
        public void TestDecember()
        {
            Assert.AreEqual(new DateTime(2016, 11, 28).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2016, 12, 24)).DayOfYear);
        }

        [Test]
        public void TestJanuary()
        {
            Assert.AreEqual(new DateTime(2016, 12, 26).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2017, 1, 10)).DayOfYear);
        }

        [Test]
        public void TestFebuary()
        {
            Assert.AreEqual(new DateTime(2017, 1, 30).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2017, 2, 19)).DayOfYear);
        }

        [Test]
        public void TestMarch()
        {
            Assert.AreEqual(new DateTime(2017, 2, 27).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2017, 3, 1)).DayOfYear);
        }

        [Test]
        public void TestApril()
        {
            Assert.AreEqual(new DateTime(2017, 3, 27).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2017, 4, 2)).DayOfYear);
        }

        [Test]
        public void TestMay()
        {
            Assert.AreEqual(new DateTime(2017, 5, 1).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2017, 5, 28)).DayOfYear);
        }

        [Test]
        public void TestJune()
        {
            Assert.AreEqual(new DateTime(2017, 5, 29).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2017, 6, 22)).DayOfYear);
        }

        [Test]
        public void TestJuly()
        {
            Assert.AreEqual(new DateTime(2017, 6, 26).DayOfYear, skolerute.data.Calendar.GetFirstRelevantDateTime(new DateTime(2017, 7, 25)).DayOfYear);
        }
    }
}
