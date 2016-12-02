using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Core;
using skolerute.data;

namespace UnitTests.Calendar
{
    [TestFixture]
    public class GetAllRelevantCalendarDaysTest
    {
        private DateTime testDate;
        private List<School> testSchools;

        [SetUp]
        public void SetUp()
        {
            testDate = new DateTime(2016, 8, 1);
            testSchools = MakeSchools();
            
        }

        private List<School> MakeSchools()
        {
            List<School> schools = new List<School>();
            School school1 = new School();
            School school2 = new School();
            School school3 = new School();

            school1.name = "Test school1";
            school2.name = "Test school2";
            school3.name = "Test school3";

            school1.calendar = new List<CalendarDay>();
            school2.calendar = new List<CalendarDay>();
            school3.calendar = new List<CalendarDay>();
            DateTime startDate = testDate;

            List<CalendarDay> calendars = new List<CalendarDay>();
            for (int i = 0; i < 365; i++)
            {
                if (i % 2 == 0)
                {
                    school1.calendar.Add(new CalendarDay(startDate.AddDays(i), true, true, false, i.ToString()));
                    school2.calendar.Add(new CalendarDay(startDate.AddDays(i), false, false, true, i.ToString()));
                    school3.calendar.Add(new CalendarDay(startDate.AddDays(i), false, false, false, i.ToString()));
                }
                else
                {
                    school1.calendar.Add(new CalendarDay(startDate.AddDays(i), false, false, true, i.ToString()));
                    school2.calendar.Add(new CalendarDay(startDate.AddDays(i), true, true, false, i.ToString()));
                    school3.calendar.Add(new CalendarDay(startDate.AddDays(i), true, true, true, i.ToString()));
                }
            }

            schools.Add(school1);
            schools.Add(school2);
            schools.Add(school3);
            return schools;
        }

        [Test]
        public void NullValuesTest()
        {
            Assert.IsNull(skolerute.data.Calendar.GetAllRelevantCalendarDays(null, DateTime.Now));
        }

        [Test]
        public void NoElementsInListTest()
        {
            Assert.IsNull(skolerute.data.Calendar.GetAllRelevantCalendarDays(new List<School>(), DateTime.Now));
        }

        [Test]
        public void FirstDayIsFreeDay()
        {
            List<List<CalendarDay>> allCalendarDaysTest = allCalendarDaysTest = skolerute.data.Calendar.GetAllRelevantCalendarDays(testSchools, testDate);

            Assert.IsTrue(allCalendarDaysTest[0][0].IsFreeDay);
        }

        [Test]
        public void LastDayIsFreeDay()
        {
            List<List<CalendarDay>> allCalendarDaysTest = allCalendarDaysTest = skolerute.data.Calendar.GetAllRelevantCalendarDays(testSchools, testDate);

            Assert.IsTrue(allCalendarDaysTest.Last().Last().IsFreeDay);
        }

        [Test]
        public void ContainsAllSchoolsTest()
        {
            List<List<CalendarDay>> allCalendarDaysTest = allCalendarDaysTest = skolerute.data.Calendar.GetAllRelevantCalendarDays(testSchools, testDate);

            Assert.AreEqual(3, allCalendarDaysTest.Count);
        }

        [Test]
        public void SecondDayNotFreeSchool1()
        {
            List<List<CalendarDay>> allCalendarDaysTest = allCalendarDaysTest = skolerute.data.Calendar.GetAllRelevantCalendarDays(testSchools, testDate);

            Assert.IsFalse(allCalendarDaysTest[0][1].IsFreeDay);
        }

        [Test]
        public void SecondDayFreeSchool2()
        {
            List<List<CalendarDay>> allCalendarDaysTest = allCalendarDaysTest = skolerute.data.Calendar.GetAllRelevantCalendarDays(testSchools, testDate);

            Assert.IsTrue(allCalendarDaysTest[1][1].IsFreeDay);
        }

        [Test]
        public void SecondDayFreeSchool3()
        {
            List<List<CalendarDay>> allCalendarDaysTest = allCalendarDaysTest = skolerute.data.Calendar.GetAllRelevantCalendarDays(testSchools, testDate);

            Assert.IsTrue(allCalendarDaysTest[2][1].IsFreeDay);
        }
    }
}
