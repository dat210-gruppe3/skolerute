using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Core;
using skolerute.data;

namespace UnitTests.Calendar
{
    [TestFixture]
    public class GetRelevantFreeDaysTest
    {
        private DateTime testDate;
        private School testSchool;

        [SetUp]
        public void SetUp()
        {
            testDate = new DateTime(2016, 8, 1);
            testSchool = MakeSchool();
        }

        private School MakeSchool()
        {
            School school = new School();
            school.name = "Test school";
            DateTime startDate = testDate; 
            
            List<CalendarDay> calendar = new List<CalendarDay>();
            for (int i = 0; i < 365; i++)
            {
                if (i%2 == 0)
                {
                    calendar.Add(new CalendarDay(startDate.AddDays(i), true, true, false, i.ToString()));
                }
                else
                {
                    calendar.Add(new CalendarDay(startDate.AddDays(i), false, false, true, i.ToString()));
                }
            }

            school.calendar = calendar;
            return school;
        }

        [Test]
        public void NullValuesTest()
        {
            Assert.Catch<NullReferenceException>(() => skolerute.data.Calendar.GetRelevantFreeDays(null, DateTime.Now));
        }

        [Test]
        public void NoElementsInListTest()
        {
            Assert.Catch<ArgumentException>(() => skolerute.data.Calendar.GetRelevantFreeDays(new List<CalendarDay>(), DateTime.Now));
        }

        [Test]
        public void DateOutOfRangeLower()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() => skolerute.data.Calendar.GetRelevantFreeDays(testSchool.calendar, DateTime.MinValue));
        }

        [Test]
        public void DateOutOfRangeUpper()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() => skolerute.data.Calendar.GetRelevantFreeDays(testSchool.calendar, DateTime.MaxValue));
        }

        [Test]
        public void DateOutOfRangeLowerEdge()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() => skolerute.data.Calendar.GetRelevantFreeDays(testSchool.calendar, new DateTime(2016, 6, 30)));
        }

        [Test]
        public void DateOutOfRangeUpperEdge()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() => skolerute.data.Calendar.GetRelevantFreeDays(testSchool.calendar, testDate.AddDays(670)));
        }

        [Test]
        public void CheckFirstMonth()
        {
            
        }
    }
}
