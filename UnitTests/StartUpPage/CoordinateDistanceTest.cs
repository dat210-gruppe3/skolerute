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
        public void DistanceTest1()
        {
            double distance = Math.Round((_gpsCoordinate.HaversineDistance(58.940822, 5.710242)), 2);
            Assert.AreEqual(1.13, distance, 0.1);
        }

        [Test]
        public void DistanceTest2()
        {
            double distance = Math.Round((_gpsCoordinate.HaversineDistance(58.964843, 5.724622)), 2);
            Assert.AreEqual(3.57, distance, 0.1);
        }

        [Test]
        public void DistanceTest3()
        {
            double distance = Math.Round((_gpsCoordinate.HaversineDistance(58.996919, 5.732202)), 2);
            Assert.AreEqual(7.00, distance, 0.1);
        }

        [Test]
        public void DistanceTest4()
        {
            double distance = Math.Round((_gpsCoordinate.HaversineDistance(58.972026, 5.676786)), 2);
            Assert.AreEqual(3.92, distance, 0.1);
        }

        [Test]
        public void DistanceTest5()
        {
            double distance = Math.Round((_gpsCoordinate.HaversineDistance(59.005435, 5.607235)), 2);
            Assert.AreEqual(8.97, distance, 0.1);
        }

        [Test]
        public void DistanceTest6()
        {
            double distance = Math.Round((_gpsCoordinate.HaversineDistance(59.044057, 5.754096)), 2);
            Assert.AreEqual(12.38, distance, 0.1);
        }

        [Test]
        public void DistanceTest7()
        {
            double distance = Math.Round((_gpsCoordinate.HaversineDistance(0.0, 0.0)), 2);
            Assert.AreEqual(6568, distance, 5.0);
        }
    }
}
