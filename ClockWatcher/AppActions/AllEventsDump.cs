using System.Collections.Generic;
using System.Diagnostics;

namespace ClockWatcher
{
    internal class AllEventsDump : AppAction
    {
        public AllEventsDump()
        {
            VerboseOutput = true;
        }

        protected override List<EventLogEntry> GetEntries()
        {
            var systemEntries = GetEventLogEntries("System", 100, new long[]{13});
            //var securityEntries = GetEventLogEntries("security", 100);
            //systemEntries.AddRange(securityEntries);
            return systemEntries;
        }
    }
}