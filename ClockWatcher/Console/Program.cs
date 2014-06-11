using System;
using System.Security;
using System.Text;
using Kraken.Core;
using Kraken.Core.Extensions;

namespace ClockWatcher
{
    class Program
    {

        static void Main(string[] args)
        {
            var metadata = ExecutionEnvironment.GetApplicationMetadata();
            Log(metadata + "\r\n");

            var options = new ConsoleOptions(args);

            if (options.ShowHelp)
            {
                Console.WriteLine("Options:");
                options.OptionSet.WriteOptionDescriptions(Console.Out);
                Console.ReadKey();
                return;
            }

            AppAction appAction = null;

            switch (options.Mode)
            {
                case Mode.AllEvents:
                    appAction = new AllEventsDump();
                    break;
                case Mode.InterestingEvents:
                    appAction = new InterestingEventDump();
                    break;
            }
                
                
            appAction.Run();
           
            Log("Press any key...");
            Console.ReadKey();
        }

        private static void Log(string output)
        {
            Console.WriteLine(output);
        }
    }
}

