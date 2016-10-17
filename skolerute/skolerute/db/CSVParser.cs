using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using skolerute.db;

namespace skolerute.db
{
	public class CSVParser
	{
		public string url;
		private IDatabaseManagerAsync database;
		private string[] schools;

        public CSVParser(string url)
        {
            this.url = url;
            this.database = new DatabaseManagerAsync();
        }

		public CSVParser(string url, DatabaseManagerAsync database)		
 		{		
 			this.url = url;		
 			this.database = database;		
 			//this.database.DeleteDatabase();		
 			//this.database.CreateNewDatabase();		
 		}

		//public static void LoadIntoDB(string filename)
		//{

		//	string path = "/data/skolerute-2016-17.csv/";
		//	private static StreamReader streamReader;
		//	streamReader = new StreamReader(filename);
		//}

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


		public void InsertToCalAndSch(string[] cols, List<data.School> schoolList)
		{
			data.CalendarDay calTemp = new data.CalendarDay(
						Convert.ToDateTime(cols[0]), !WordsToBool(cols[2]), WordsToBool(cols[3]), WordsToBool(cols[4]), cols[5]);

			schoolList[schoolList.Count - 1].calendar.Add(calTemp);
			string hei = "hei";
		}

        public async Task RetrieveSchools()
        {
            var csv = await GetContent(Constants.URL);
            char[] delimiter = new char[] { '\r', '\n' };
            string[] rows = csv.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            string[] cols = new string[5];
            List<data.School> schools = new List<data.School>();
            string oldschool = "";

            for (int i = 1; i < rows.Length; i++)
            {
                cols = Splitter(rows[i]);
                string schname = cols[1];

                if(schname != oldschool)
                {
                    oldschool = schname;
                    schools.Add(new data.School(cols[1], null));
                    await database.InsertSingle(schools[schools.Count - 1]);
                }
            }
        }

        public async Task RetrieveCalendar(data.School sch)
        {
            var csv = await GetContent(Constants.URL);
            char[] delimiter = new char[] { '\r', '\n' };
            string[] rows = csv.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            string[] cols = new string[5];
            List<data.CalendarDay> schCalendar = new List<data.CalendarDay>();

            for (int i = 1; i < rows.Length; i++)
            {
                cols = Splitter(rows[i]);

                if (cols[1] == sch.name)
                {
                    schCalendar.Add(new data.CalendarDay(Convert.ToDateTime(cols[0]), 
                        !WordsToBool(cols[2]), WordsToBool(cols[3]), WordsToBool(cols[4]), cols[5]));
                }
            }
            sch.calendar = schCalendar;
            await database.UpdateSingle(sch);
        }

        public async Task StringParser()
		{
			var csv = await GetContent(Constants.URL);
			char[] delimiter = new char[] { '\r', '\n' };
			string[] rows = csv.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
			string[] cols = new string[5];
			List<data.School> schoolObjs = new List<data.School>();

			int j = 1;
			while (j < rows.Length)
			{
				cols = Splitter(rows[j]);
				string sch = cols[1];

				schoolObjs.Add(new data.School(cols[1], new List<data.CalendarDay>() ));


				while (cols[1] == sch)
				{
					InsertToCalAndSch(cols, schoolObjs);
					j++;
					if (j >= rows.Length) break;
					cols = Splitter(rows[j]);
				}

				//if (j % 3 == 0)
				//{
				//	string hei = "";
				//}

				await database.InsertSingle(schoolObjs[schoolObjs.Count - 1]);
			}
			//await database.InsertList(schoolObjs);
			//await database.InsertSingle(schoolObjs[schoolObjs.Count - 1]);

			List<data.School> schoolsList = await database.GetSchools();
			GC.KeepAlive(schoolsList);
			string test = "hei";
			schoolsList[23] = null;

			//TODO: error handling
		}


        public async Task<String> GetContent(String url)
        {
            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader stream = new StreamReader(response.GetResponseStream());
                return stream.ReadToEnd();
            }
            else
            {
				throw new WebException("Could not interact with \n" + Constants.URL);
            }
        }
	}
}
