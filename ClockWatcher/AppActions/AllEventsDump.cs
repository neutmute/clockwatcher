using System;
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
            var entries = GetEventLogEntries("System", 1000 /*, new long[]{13}*/);
            var securityEntries = GetEventLogEntries("security", 10000, 4648);
            entries.AddRange(securityEntries);
            entries.Sort((x,y) => DateTime.Compare(x.TimeGenerated, y.TimeGenerated));
            return entries;
        }
    }
}