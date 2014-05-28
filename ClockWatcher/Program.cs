using System;
using System.Linq;
using System.Diagnostics;
using System.Security;
using System.Text;
using System.Collections.Generic;
using Kraken.Core;
using Kraken.Core.Extensions;

namespace ClockWatcher
{
    class Program
    {
        private static ObjectDump GetDump(EventLogEntry entry)
        {
            var dump = new ObjectDump();
            dump.Headers.Add("Time");
            dump.Headers.Add("Message");
            dump.Data.Add(entry.TimeGenerated.ToString("dd-MMM-yyyy HH:mm"));
            dump.Data.Add(entry.Message.Substring(0, entry.Message.IndexOf(".")));
            return dump;
        }

        static void Main(string[] args)
        {

            string logType = "security";
            var lockUnlockEVentTypes = new long[] { 4800, 4801 };
            const string machine = "."; // local machine

            Log("Opening Event Log...");
            var securityLog = new EventLog(logType, machine);

            var logEntries = (
                            from EventLogEntry logEntry in securityLog.Entries
                            where lockUnlockEVentTypes.Contains(logEntry.InstanceId)
                            select logEntry
                        )
                        .OrderBy(e => e.TimeGenerated)
                        .Take(100)
                        .ToList();

            if (logEntries.Count == 0)
            {
                Log("No interesting entries found");
            }
            else
            {
                var dumper = new ObjectDumper<EventLogEntry>(GetDump);
                Log(dumper.Dump(logEntries));    
            }

            Log("Press any key...");
            Console.ReadKey();
        }

        private static void Log(string output)
        {
            Console.WriteLine(output);
        }
    }
}

