using System.Runtime.InteropServices.ComTypes;

namespace skolerute.ExportCalendar
{
    public class MyCalendar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Accout { get; set; }

        public MyCalendar(int id, string name, string account)
        {
            Id = id;
            Name = name;
            Accout = account;
        }
    }
}