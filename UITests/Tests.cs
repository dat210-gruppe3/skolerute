using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITests
{

    [TestFixture(Platform.Android)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        [Test]
        public void AppLoadsFirstSchool()
        {
            Func<AppQuery, AppQuery> firstSchoolQuery = c => c.Text("Auglend skole");

            app.WaitForElement(firstSchoolQuery);
            app.Screenshot("Looking for the first school on the screen");
            var school = app.Query(firstSchoolQuery);

            Assert.IsNotNull(school);
        }

        [Test]
        public void AddFavoriteSchool()
        {
            Func<AppQuery, AppQuery> firstSchoolQuery = c => c.Text("Auglend skole");
            Func<AppQuery, AppQuery> confirmButton = c => c.Id("button1");
            Func<AppQuery, AppQuery> favoriteSchoolQuery =
                c => c.Descendant("favoriteSchoolsList").Text("Auglend skole");

            app.WaitForElement(firstSchoolQuery);
            app.Screenshot("Schools have been loaded");
            app.Tap(firstSchoolQuery);
            app.WaitForElement(confirmButton);
            app.Screenshot("Option to add or cancel school is shown");
            app.Tap(confirmButton);
            app.WaitForElement(favoriteSchoolQuery);
            app.Screenshot("School has been added to favorites");
            var favoriteSchool = app.Query(favoriteSchoolQuery);

            Assert.IsNotNull(favoriteSchool);
        }

        [Test]
        public void RemoveFavoriteSchool()
        {
            Func<AppQuery, AppQuery> firstSchoolQuery = c => c.Text("Auglend skole");
            Func<AppQuery, AppQuery> confirmButtonQuery = c => c.Id("button1");
            Func<AppQuery, AppQuery> favoriteSchoolQuery =
                c => c.Descendant("favoriteSchoolsList").Text("Auglend skole");

            app.WaitForElement(firstSchoolQuery);
            app.Screenshot("Schools have been loaded");
            app.Tap(firstSchoolQuery);
            app.WaitForElement(confirmButtonQuery);
            app.Screenshot("Prompt whether or not to add school");
            app.Tap(confirmButtonQuery);
            app.WaitForElement(favoriteSchoolQuery);
            app.Screenshot("School added to favorites");
            app.Tap(favoriteSchoolQuery);
            app.WaitForElement(confirmButtonQuery);
            app.Screenshot("Ask whether or not to remove selected school");
            app.Tap(confirmButtonQuery);
            app.Screenshot("Favorite school removed");
            var favoriteSchool = app.Query(favoriteSchoolQuery).FirstOrDefault();

            Assert.IsNull(favoriteSchool);
        }

        [Test]
        public void SearchSchool()
        {
            Func<AppQuery, AppQuery> searchFieldQuery = c => c.Marked("schoolSearchField");
            Func<AppQuery, AppQuery> searchResultQuery = c => c.Text("Vardenes skole");

            app.Screenshot("Ready to search for school");
            app.EnterText(searchFieldQuery, "Vard");
            app.Screenshot("Entered search term");
            app.DismissKeyboard();
            app.WaitForElement(searchResultQuery);
            app.Screenshot("Dismissed keyboard");
            var searchResult = app.Query(searchResultQuery);

            Assert.IsNotNull(searchResult);
        }

        [Test]
        public void ShowDistanceToSchools()
        {
            Func<AppQuery, AppQuery> closestSchoolsButtonQuery = c => c.Button("closestSchoolsButton");
            Func<AppQuery, AppQuery> endsWithKmQuery = c => c.Property("text").EndsWith("km");

            app.Screenshot("Before pressing closest schools button");
            app.Tap(closestSchoolsButtonQuery);
            app.WaitForElement(endsWithKmQuery);
            app.Screenshot("After pressing closest schools button");
            var textEndingWithKm = app.Query(endsWithKmQuery);

            Assert.IsNotNull(textEndingWithKm);
        }

        [Test]
        public void ReturnToAllSchools()
        {
            Func<AppQuery, AppQuery> firstSchoolQuery = c => c.Text("Auglend skole");
            Func<AppQuery, AppQuery> closestSchoolsButtonQuery = c => c.Button("closestSchoolsButton");
            Func<AppQuery, AppQuery> endsWithKmQuery = c => c.Property("text").EndsWith("km");
            Func<AppQuery, AppQuery> backAtAllSchoolsQuery = c => c.Text("VIS N�RMESTE");

            app.Screenshot("Before pressing closest schools button");
            app.WaitForElement(firstSchoolQuery);
            app.Tap(closestSchoolsButtonQuery);
            app.Screenshot("Showing closest schools");
            app.WaitForElement(endsWithKmQuery); // Waiting for km to appear
            app.Tap(closestSchoolsButtonQuery);
            app.WaitForNoElement(endsWithKmQuery); // Wait for it to disappear
            app.WaitForElement(firstSchoolQuery);
            app.Screenshot("Showing all schools again");
            var backAtAllSchools = app.Query(backAtAllSchoolsQuery);

            Assert.IsNotNull(backAtAllSchools);
        }
    }
}
