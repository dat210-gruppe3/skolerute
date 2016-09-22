using System;
using System.IO;
//using PrjTest;
namespace skolerute
{
	public class CSVParser
	{
		private static DateTime day;
		private static string school;
		private static bool pupilDay;
		private static bool teacherDay;
		private static bool SFODay;
		private static string comment;

		



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

		public static void StringParser(string csv)
		{
			//char[] delimiters = { ',', ',', ',', ',' };
			//string[] parameters = csv.Split(delimiters);
			/*
			day = Convert.ToDateTime(parameters[0]);
			school = parameters[1];
			pupilDay = WordsToBool() parameters[2];
			teacherDay = parameters[3];
			SFODay = parameters[4];
			comment = parameters[5];*/


			char delimiter = ',';
			string[] parameters = csv.Split(delimiter);
			
			for (int i = 0; i < parameters.Length; i++) {
				day = Convert.ToDateTime(parameters[i]);
				school = parameters[i];
				pupilDay = WordsToBool(parameters[i]);
				teacherDay = WordsToBool(parameters[i]);
				SFODay = WordsToBool(parameters[i]);
				comment = parameters[i];

			}
		}
		public static void Main()
		{
			string csvLine = "2016-08-01,Auglend skole, Nei, Nei, Ja,\n2016-08-02,Auglend skole,Nei,Nei,Ja,";
			StringParser(csvLine);

		}


	}
}
