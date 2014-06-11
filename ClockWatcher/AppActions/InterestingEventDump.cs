using System.Collections.Generic;
using System.Diagnostics;

namespace ClockWatcher
{
    class InterestingEventDump : AppAction
    {
        protected override List<EventLogEntry> GetEntries()
        {
            var lockUnlockEventTypes = new long[] { 4800, 4801 };

            var logEntries = GetEventLogEntries("security", 100, lockUnlockEventTypes);
            logEntries.AddRange(GetEventLogEntries("system", 100, EventInstanceIdShutdown)); 
            
            return logEntries;
        }
    }
}