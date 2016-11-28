using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
            Assert.Catch<NullReferenceException>(() => skolerute.data.Calendar.GetAllRelevantCalendarDays(null, DateTime.Now));
        }

        [Test]
        public void NoElementsInListTest()
        {
            Assert.Catch<ArgumentException>(() => skolerute.data.Calendar.GetAllRelevantCalendarDays(new List<School>(), DateTime.Now));
        }
    }
}
