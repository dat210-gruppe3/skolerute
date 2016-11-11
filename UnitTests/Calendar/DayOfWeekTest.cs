using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using skolerute.data;

namespace UnitTests.Calendar
{
    /// <summary>
    /// This class will test the Calendar.DayOfWeek test to see if we get the expected result.
    /// </summary>
    [TestFixture] // This tells NUnit that this is a testing class
    public class DayOfWeekTest
    {
        // You can have some class variables here, for example:
        // School school;

        /* You can also use the [SetUp] tag to mark a method to be run before the tests if you need to. For example:
        [SetUp]
        public void SetUp() {
            school = new School(..., ... ,...);
        }
        This would give you a School object set up however you want it for use in later test methods
        */

        // To run these tests you can use the circle icons to the left of the line numbers to add them to a session.
        // You can add all the tests in a class by using the icon with two circles by the class definition(line 15 in this case).
        [Test]
        public void DayOfWeekMonday ()
        {
            // Expect 0 when giving a date which falls on a Monday (07-11-2016 is a Monday)
            Assert.AreEqual(0, skolerute.data.Calendar.DayOfWeek(new DateTime(2016, 11, 7)));
        }

        [Test]
        public void DayOfWeekTuesday()
        {
            // Expect 1 when giving a date which falls on a Tuesday (08-11-2016 is a Tuesday)
            Assert.AreEqual(1, skolerute.data.Calendar.DayOfWeek(new DateTime(2016, 11, 8)));
        }

        [Test]
        public void DayOfWeekWednesday()
        {
            // We use asserts to do the testing, Assert contains many methods, for instance the Assert.IsNull() method,
            // which checks to see if something is null.
            // Chcking for edge cases is a good thing to do, but in this case it is impossible, because you can't pass in
            // null for a DateTime object.

            // Expect 2 when giving a date which falls on a Wednesday (09-11-2016 is a Wednesday)
            Assert.AreEqual(2, skolerute.data.Calendar.DayOfWeek(new DateTime(2016, 11, 9)));
        }

        [Test]
        public void DayOfWeekThursday()
        {
            // Expect 3 when giving a date which falls on a Thursday (10-11-2016 is a Thursday)
            Assert.AreEqual(3, skolerute.data.Calendar.DayOfWeek(new DateTime(2016, 11, 10)));
        }

        [Test]
        public void DayOfWeekFriday()
        {
            // Expect 4 when giving a date which falls on a Friday (11-11-2016 is a Friday)
            Assert.AreEqual(4, skolerute.data.Calendar.DayOfWeek(new DateTime(2016, 11, 11)));
        }

        [Test]
        public void DayOfWeekSaturday()
        {
            // Expect 5 when giving a date which falls on a Saturday (12-11-2016 is a Saturday)
            Assert.AreEqual(5, skolerute.data.Calendar.DayOfWeek(new DateTime(2016, 11, 12)));
        }

        [Test]
        public void DayOfWeekSunday()
        {
            // Expect 6 when giving a date which falls on a Sunday (13-11-2016 is a Sunday)
            Assert.AreEqual(6, skolerute.data.Calendar.DayOfWeek(new DateTime(2016, 11, 13)));
        }
    }
}
