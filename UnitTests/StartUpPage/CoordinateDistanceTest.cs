using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTests.StartUpPage
{

    [TestFixture]
    class CoordinateDistanceTest
    {
        [Test]
        public void DistanceTest()
        {
            Assert.AreEqual(0, skolerute.data.Coordinate.HaversineDistance(58.937699, 5.691627));
        }

    }
}
