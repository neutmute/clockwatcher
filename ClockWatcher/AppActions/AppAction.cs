using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kraken.Core;

namespace ClockWatcher
{
    abstract class AppAction
    {
        public const int EventInstanceIdShutdown = 13;

        public bool VerboseOutput { get; set; }
        protected static void Log(string outputFormat, params object[] outputArgs)
        {
            Console.WriteLine(outputFormat, outputArgs);
        }
        private ObjectDump GetDump(EventLogEntry entry)
        {
            var posOfPeriod = entry.Message.IndexOf(".");
            var message = entry.Message.Trim();

            if (posOfPeriod > 0)
            {
                message = message.Substring(0, posOfPeriod);
            }

            if (entry.InstanceId == EventInstanceIdShutdown)
            {
                message = "The System is shutting down ";// + entry.Message;
            }

            var dump = new ObjectDump();
            dump.Headers.Add("Time");
            dump.Headers.Add("Message");
            dump.Data.Add(entry.TimeGenerated.ToString("dd-MMM-yyyy HH:mm"));
            dump.Data.Add(message);

            if (VerboseOutput)
            {
                dump.Headers.Insert(1, "InstanceId");
                dump.Data.Insert(1, entry.InstanceId.ToString());
            }


            return dump;
        }

        protected abstract List<EventLogEntry> GetEntries();

        public void Run()
        {
            var logEntries = GetEntries()
                .OrderBy(e => e.TimeGenerated) // want the most recent at the bottom;
                .ToList();

            if (logEntries.Count == 0)
            {
                Log("No entries found");
            }
            else
            {
                var dumper = new ObjectDumper<EventLogEntry>(GetDump);
                Log(dumper.Dump(logEntries));
            }
        }

        protected List<EventLogEntry> GetEventLogEntries(string logType, int maxEvents = 100, params long[] filterEventTypes)
        {
            const string machine = "."; // local machine

            Log(
                "Opening {0} Event Log for {1} entries, targeting {2}"
                , logType
                , maxEvents
                , filterEventTypes.ToCsv(", "));

            var eventLog = new EventLog(logType, machine);


            var logEntries = (
                    from EventLogEntry logEntry in eventLog.Entries
                    where  (filterEventTypes == null || filterEventTypes.Length == 0) || (filterEventTypes.Contains(logEntry.InstanceId)) 
                    select logEntry
                )
                .OrderByDescending(e => e.TimeGenerated)
                .Take(maxEvents)
                .ToList();

            return logEntries;
        }

    }
}