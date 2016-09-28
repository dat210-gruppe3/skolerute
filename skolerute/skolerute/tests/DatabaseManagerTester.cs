using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using skolerute.db;
using skolerute.data;
using NUnit.Framework;

namespace skolerute.tests
{
    [TestFixture]
    public class DatabaseManagerTester
    {
        DatabaseManager dbm;

        [SetUp]
        private void SetUp()
        {
            dbm = new DatabaseManager();
            dbm.CreateNewDatabase();
        }

        [Test]
        private void GetSchoolsTest()
        {
            List<School> schools = new List<School>();
            schools.Add(new School("School 1", null));
            schools.Add(new School("School 2", null));
            schools.Add(new School("School 3", null));
            dbm.InsertList<School>(schools);

            List<School> result = dbm.GetSchools().ToList();

            Assert.IsTrue(result.Equals(schools));
        }
    }
}
