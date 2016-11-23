using System.Runtime.InteropServices.ComTypes;

namespace skolerute.ExportCalendar
{
    public class MyCalendar
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Accout { get; set; }

        public MyCalendar(string id, string name, string account)
        {
            Id = id;
            Name = name;
            Accout = account;
        }
    }
}