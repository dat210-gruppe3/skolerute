using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Linq;

namespace skolerute
{
	public class CSVParser
	{
		//private static db.DatabaseManager database = new db.DatabaseManager();



		private static string[] schools;
		//private static int[] id;

		// FIXME: Lag metode som laster inn CSV-fila og lagrer den som en streng. Kall på StringParser()
		//public static void LoadIntoDB(string filename)
		//{

		//	string path = "/data/skolerute-2016-17.csv/";
		//	private static StreamReader streamReader;
		//	streamReader = new StreamReader(filename);
		//}

		//Console.WriteLine("hei");

		/// <summary>This method is to be used in </summary>
		/// <returns><c>true</c>, if string is 'Ja', <c>false</c> otherwise.</returns>
		/// <param name="">.</param>
		private static bool WordsToBool(string s)
		{
			if (s == "Ja")
			{
				return true;
			}
			return false;
		}


		/// <summary>
		/// This method takes in a line (tuple) from StringParser() and returns an array of the columns
		/// </summary>
		private static string[] Splitter(string s)
		{
			char delimiter = ',';
			string[] parameters = s.Split(delimiter);


			return parameters;
		}



		public static async void StringParser(string csv)
		{
			//database.CreateNewDatabase();
			//StreamReader sr = new StreamReader("/../data/skolerute-2016-17.csv");
			csv = await GetContent("http://open.stavanger.kommune.no/dataset/86d3fe44-111e-4d82-be5a-67a9dbfbfcbb/resource/32d52130-ce7c-4282-9d37-3c68c7cdba92/download/skolerute-2016-17.csv");


			char[] delimiter = new char[] { '\r', '\n' };
			string[] parameters = csv.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);


			//char delimiter = '\n';
			//string[] parameters = csv.Split(delimiter);
			string[] tuple = new string[5];

			List<data.School> schoolObjs = new List<data.School>();

			int j = 1;

			//while (j < parameters.Length)
			while (j < parameters.Length)
			{
				tuple = Splitter(parameters[j]);
				string sch = tuple[1];

				schoolObjs.Add(new data.School(
					j, tuple[1], new List<data.CalendarDay>() ));


				while (tuple[1] == sch)
				{
					data.CalendarDay calTemp = new data.CalendarDay(
						j, Convert.ToDateTime(tuple[0]), WordsToBool(tuple[2]), WordsToBool(tuple[3]), WordsToBool(tuple[4]), tuple[5]);

					schoolObjs[schoolObjs.Count - 1].calendar.Add(calTemp);
					j++;

					//if (j >= parameters.Length)
					if (j >= parameters.Length)
					{
						break;
					}

					tuple = Splitter(parameters[j]);

				}



			}

			//database.InsertList(schoolObjs);

			//List<data.School> testschool = database.GetSchools().ToList();


			//List<data.School> gc = new List<data.School>();
		}

        public static async Task<String> GetContent(String url)
        {
            //Console.Write("Started");
            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader stream = new StreamReader(response.GetResponseStream());
                //String csvfile = await stream.ReadToEndAsync();
                //Console.Write(csvfile);
                return stream.ReadToEnd();
            }
            else
            {
                //TODO something went wrong!
                return null;
            }
            //Console.Write("Ended");
        }

        public static void Main()
		{
			string csvLine = "2016-08-01,Auglend skole, Nei, Nei, Ja,\n2016-08-02,Auglend skole,Nei,Nei,Ja,";
			StringParser(csvLine);

		}


	}
}
