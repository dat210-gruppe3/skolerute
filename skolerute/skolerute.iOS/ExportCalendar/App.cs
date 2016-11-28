using EventKit;

namespace skolerute.iOS.ExportCalendar
{
    /// <summary>
    /// Singleton class to store instances to app-wide objects, we only need it for an instance of
    /// EKEventStore, as this is expensive to make multiple times. It can be thought of like a database
    /// of events. The reference is stored in a singleton fashion.
    /// </summary>
    public class App
    {
        public static App Current
        {
            get { return current; }
        }

        private static App current;

        public EKEventStore EventStore
        {
            get { return eventStore; }
        }

        protected EKEventStore eventStore;

        static App()
        {
            current = new App();
        }

        protected App()
        {
            eventStore = new EKEventStore();
        }
    }
}
