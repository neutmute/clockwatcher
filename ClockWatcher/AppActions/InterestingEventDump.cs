using System.Collections.Generic;
using System.Diagnostics;

namespace ClockWatcher
{
    class InterestingEventDump : AppAction
    {
        protected override List<EventLogEntry> GetEntries()
        {
            var interestingSecurityEventTypes = new long[]
            {
                4800   // lock
                , 4801 // unlock
                , 4778 // A session was reconnected to a Window Station (for when connected via RDP or had RDP'd from home and then taking over console again)
                , 4779 // A session was disconnected from a Window Station. (RDP disconnected)
            };

            var logEntries = GetEventLogEntries("security", 100, interestingSecurityEventTypes);
            logEntries.AddRange(GetEventLogEntries("system", 100, EventInstanceIdShutdown)); 
            
            return logEntries;
        }
    }
}