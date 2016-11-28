using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using skolerute.data;

namespace UnitTests.Calendar
{
    [TestFixture]
    class MonthToStringTest
    {
        [Test]
        public void JanuaryToString()
        {
            Assert.AreEqual("Januar", skolerute.data.Calendar.MonthToString(1));
        }

        [Test]
        public void FebruaryToString()
        {
            Assert.AreEqual("Februar", skolerute.data.Calendar.MonthToString(2));
        }

        [Test]
        public void MarsToString()
        {
            Assert.AreEqual("Mars", skolerute.data.Calendar.MonthToString(3));
        }

        [Test]
        public void AprilToString()
        {
            Assert.AreEqual("April", skolerute.data.Calendar.MonthToString(4));
        }

        [Test]
        public void MayToString()
        {
            Assert.AreEqual("Mai", skolerute.data.Calendar.MonthToString(5)); 
            
        }

        [Test]
        public void JuneToString()
        {
            Assert.AreEqual("Juni", skolerute.data.Calendar.MonthToString(6)); 
            
        }

        [Test]
        public void JulyToString()
        {
            Assert.AreEqual("Juli", skolerute.data.Calendar.MonthToString(7)); 
            
        }

        [Test]
        public void AugustToString()
        {
            Assert.AreEqual("August", skolerute.data.Calendar.MonthToString(8)); 
            
        }

        [Test]
        public void SeptemberToString()
        {
            Assert.AreEqual("September", skolerute.data.Calendar.MonthToString(9)); 
            
        }

        [Test]
        public void OctoberToString()
        {
            Assert.AreEqual("Oktober", skolerute.data.Calendar.MonthToString(10)); 
            
        }

        [Test]
        public void NovemberToString()
        {
            Assert.AreEqual("November", skolerute.data.Calendar.MonthToString(11)); 
            
        }

        [Test]
        public void DecemberToString()
        {
            Assert.AreEqual("Desember", skolerute.data.Calendar.MonthToString(12)); 
            
        }
    }
}
