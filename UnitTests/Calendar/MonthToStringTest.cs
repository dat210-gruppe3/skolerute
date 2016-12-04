using System;
using NUnit.Framework;
using NUnit.Core;

namespace UnitTests.Calendar
{
    [TestFixture]
    public class MonthToStringTest
    {
        [Test]
        public void JanuaryTest()
        {
            Assert.AreEqual("Januar", skolerute.data.Calendar.MonthToString(1));
        }

        [Test]
        public void FebuaryTest()
        {
            Assert.AreEqual("Februar", skolerute.data.Calendar.MonthToString(2));
        }

        [Test]
        public void MarchTest()
        {
            Assert.AreEqual("Mars", skolerute.data.Calendar.MonthToString(3));
        }

        [Test]
        public void AprilTest()
        {
            Assert.AreEqual("April", skolerute.data.Calendar.MonthToString(4));
        }

        [Test]
        public void MayTest()
        {
            Assert.AreEqual("Mai", skolerute.data.Calendar.MonthToString(5));
        }

        [Test]
        public void JuneTest()
        {
            Assert.AreEqual("Juni", skolerute.data.Calendar.MonthToString(6));
        }

        [Test]
        public void JulyTest()
        {
            Assert.AreEqual("Juli", skolerute.data.Calendar.MonthToString(7));
        }

        [Test]
        public void AugustTest()
        {
            Assert.AreEqual("August", skolerute.data.Calendar.MonthToString(8));
        }

        [Test]
        public void SeptemberTest()
        {
            Assert.AreEqual("September", skolerute.data.Calendar.MonthToString(9));
        }

        [Test]
        public void OctoberTest()
        {
            Assert.AreEqual("Oktober", skolerute.data.Calendar.MonthToString(10));
        }

        [Test]
        public void NovemberTest()
        {
            Assert.AreEqual("November", skolerute.data.Calendar.MonthToString(11));
        }

        [Test]
        public void DecemberTest()
        {
            Assert.AreEqual("Desember", skolerute.data.Calendar.MonthToString(12));
        }

        [Test]
        public void OutOfRangeUpperTest()
        {
            Assert.Catch<IndexOutOfRangeException>(() => skolerute.data.Calendar.MonthToString(13));
        }

        [Test]
        public void OutOfRangeLowerTest()
        {
            Assert.Catch<IndexOutOfRangeException>(() => skolerute.data.Calendar.MonthToString(0));
        }
    }
}
