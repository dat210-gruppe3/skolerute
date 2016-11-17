using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using skolerute.data;

namespace UnitTests.StartUpPage
{

    [TestFixture]
    class CoordinateDistanceTest
    {
        private readonly Coordinate _gpsCoordinate = new Coordinate(58.937699, 5.691627);

        [Test]
        public void DistanceTest()
        {  
            double distance = Math.Round((_gpsCoordinate.HaversineDistance(58.940822, 5.710242)),2);
            Assert.AreEqual(1.12, distance,0.1);
        }

        [Test]
        public void DistanceTest2()
        {
            double distance = Math.Round((_gpsCoordinate.HaversineDistance(58.964843, 5.724622)), 2);
            Assert.AreEqual(3.57, distance,0.1);
        }

        [Test]
        public void DistanceTest3()
        {
            double distance = Math.Round((_gpsCoordinate.HaversineDistance(58.996919, 5.732202)), 2);
            Assert.AreEqual(7.00, distance,0.1);
        }



    }
}
