﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace skolerute
{
	public class CSVParser
	{
		private static string[] schools;
		private static int[] id;

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

		private static int HasSchoolBeenBefore(string school)
		{

			for (int i = 0; i < school.Length; i++)
			{
				if (schools[i] == school)
				{
					return i;
				}
			}
			schools[schools.Length + 1] = school;
			return (schools.Length + 1);
		}


		public static void StringParser(string csv)
		{
			char delimiter = '\n';
			string[] parameters = csv.Split(delimiter);

			string[] tuple = new string[5];

			List<data.School> schoolObjs = new List<data.School>();

			int j = 0;

			while (j <= parameters.Length)
			{
				tuple = Splitter(parameters[j]);
				string sch = tuple[1];


				//TODO: fjern HasSchoolBeenBefore
				schoolObjs.Add(new data.School(
					HasSchoolBeenBefore(tuple[1]), tuple[1], new List<data.CalendarDay>() ));


				while (tuple[1] == sch)
				{
					int da = schoolObjs.Count;
					schoolObjs[schoolObjs.Count].calendar.Add(
						new data.CalendarDay(1, Convert.ToDateTime("2011-01-01"), false, true, false, "yahoo.xom"));
					j++;
					tuple = Splitter(parameters[j]);
				}

			}



			/*for (int i = 0; i <= parameters.Length; i++)
			{
				tuple = (string[])Splitter(parameters[i]);

				//data.CalendarDay cal = new data.CalendarDay(
				//	i, Convert.ToDateTime(tuple[0]), WordsToBool(tuple[2]), WordsToBool(tuple[3]), WordsToBool(tuple[4]), tuple[5]);

				//data.School school = new data.School(
				//	HasSchoolBeenBefore(tuple[1]), tuple[1], cal);


			}*/

			//Console.WriteLine(day + school + pupilDay + teacherDay + SFODay + comment);
		}
		public static void Main()
		{
			string csvLine = "2016-08-01,Auglend skole, Nei, Nei, Ja,\n2016-08-02,Auglend skole,Nei,Nei,Ja,";
			StringParser(csvLine);

		}


	}
}
