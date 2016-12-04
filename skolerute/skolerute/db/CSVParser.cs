using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using skolerute.data;
using SQLiteNetExtensions.Extensions;
using System.Threading;
using System.Diagnostics;

namespace skolerute.db
{
    public class CSVParser
    {
        public string url;
        private IDatabaseManagerAsync database;

        public CSVParser(string url)
        {
            this.url = url;
            database = new DatabaseManagerAsync();
        }

        public CSVParser(string url, DatabaseManagerAsync database)
        {
            this.url = url;
            this.database = database;
        }

        private bool WordsToBool(string s)
        {
            if (s == "Ja") return true;
            return false;
        }


        private string[] Splitter(string s)
        {
            char delimiter = ',';
            string[] rows = s.Split(delimiter);
            return rows;
        }

        private double StringToDouble(string s)
        {
			return double.Parse(s, CultureInfo.InvariantCulture);
        }

        public void InsertToCalAndSch(string[] cols, List<School> schoolList)
        {
            CalendarDay calTemp = new CalendarDay(
                        Convert.ToDateTime(cols[0]).ToUniversalTime(), !WordsToBool(cols[2]), WordsToBool(cols[3]), WordsToBool(cols[4]), cols[5]);

            schoolList[schoolList.Count - 1].calendar.Add(calTemp);
        }

        public async Task RetrieveSchools()
        {
            var csv = await GetContent(Constants.URL);
			var csv2 = await GetContent(Constants.positionURL);
            char[] delimiter = { '\r', '\n' };
            string[] rows = csv.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            string[] cols = new string[5];
            List<School> schools = new List<School>();
            string oldschool = "";

            await database.GetConnection().RunInTransactionAsync(connection =>
            {
                for (int i = 1; i < rows.Length; i++)
                {
                    cols = Splitter(rows[i]);
                    string schname = cols[1];

                    if (schname != oldschool)
                    {
                        oldschool = schname;
                        School school = new School(cols[1], null);
                        schools.Add(school);

                        RetrievePosition(school, csv2)/*.Wait()*/;
                        connection.Insert(schools.Last());
                    }
                }
            });
        }

        public async Task RetrieveCalendar(School sch)
        {
            var csv = await GetContent(Constants.URL);
            char[] delimiter = { '\r', '\n' };
            string[] rows = csv.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            string[] cols = new string[5];
            List<CalendarDay> schCalendar = new List<CalendarDay>();

            await database.GetConnection().RunInTransactionAsync(connection => 
            {
                for (int i = 1; i < rows.Length; i++)
                {
                    cols = Splitter(rows[i]);

                    if (cols[1] == sch.name)
                    {
                        DateTime date = Convert.ToDateTime(cols[0]);
                        
                        schCalendar.Add(new CalendarDay(new DateTime(date.Year, date.Month, date.Day, 12, 0, 0), !WordsToBool(cols[2]), WordsToBool(cols[3]), WordsToBool(cols[4]), cols[5]));
                    }
                }
                sch.calendar = schCalendar;
                connection.InsertOrReplaceWithChildren(sch);
            });
        }

        public async Task<String> GetContent(String url)
        {
			if (Constants.skoleruterCSV != null && url == Constants.URL)
			{
				return Constants.skoleruterCSV;
			}
			else {
				WebRequest request = WebRequest.Create(url);

				Timeout(request, 60000);

				HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

				if (response != null && response.StatusCode == HttpStatusCode.OK)
				{
					StreamReader stream = new StreamReader(response.GetResponseStream());
					if (url == Constants.URL)
					{
						Constants.skoleruterCSV = stream.ReadToEnd();
						return Constants.skoleruterCSV;
					}
					return stream.ReadToEnd();

				}
				throw new WebException("Could not interact with \n" + Constants.URL);
			}
        }

        public async void Timeout(WebRequest request, int timeUntilTimeout)
        {
            await Task.Delay(timeUntilTimeout);
            request.Abort();
        }


        public void RetrievePosition(School sch, string csv)
        {
            char[] delimiter = { '\r', '\n' };
            string[] rows = csv.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            string[] cols = new string[13];
            for (int i = 1; i < rows.Length; i++)
            {
                cols = Splitter(rows[i]);
                string schname = cols[9];

				if (schname.ToLower() == sch.name.ToLower())
                {
					sch.latitude = StringToDouble(cols[2]);
					sch.longitude = StringToDouble(cols[3]);
                    sch.address = cols[10];
                    sch.website = cols[11];
                }
            }
        }
    }
}
