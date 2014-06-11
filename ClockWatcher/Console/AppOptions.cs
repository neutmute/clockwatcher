using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;

namespace ClockWatcher
{
    public enum Mode
    {
        Help = 0,
        InterestingEvents= 1,
        AllEvents = 2
    }

    public class ConsoleOptions
    {
        public Mode Mode { get; set; }

       public bool ShowHelp;

        public OptionSet OptionSet { get; private set; }

        public ConsoleOptions(string[] args)
        {
            Mode = Mode.InterestingEvents;  // default

            OptionSet = new OptionSet {
                { "a|all", "Show all events",  v => Mode =Mode.AllEvents},
                { "h|?", "Show this help", v => ShowHelp = true }
            };
            OptionSet.Parse(args);
        }

        public override string ToString()
        {
            var s = new StringBuilder();

            s.AppendFormat("Mode={0}", Mode);

            return s.ToString();
        }

    }
}
