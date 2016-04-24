using System.Collections.Generic;
using DocoptNet;

namespace Gemini.Commander.Core
{
    public class MainArgs
    {
        public const string Usage = @"Magic Times

Usage:
    magictimes log <time> <id> [--when=<date>] [--log-type=<type>] <message>... 
    magictimes create ticket (<project> <state> <title>...) [--parent=<id>]
    magictimes show project all
    magictimes show logs
    magictimes show logs project <id> [--from=<date>] [--to=<date>]
    magictimes show logs ticket  <id> [my] [--from=<date>] [--to=<date>]
    magictimes show logs user <username> [--from=<date>]  [--to=<date>]
    magictimes show ticket assigned
    magictimes show ticket <id>
    magictimes show hours [by <user>] [--from=<date>]  [--to=<date>]  [--working-hours=<hours>]  
    magictimes show words [--stemmed] [--all]

Options:
    -h --help                 Show this screen.
    --when=<date>             The date for wen time log entry YYYY-MM-DD [default:now]
    --log-type=<type>         Type of timelogging billable [default:30]
    --from=<date>             The first inclussive date of the time period [default:today-30days]
    --to=<date>               The last inclussive date of the time period [default:today]
    --working-hours=<hours>   The number of working hours in a working day [default:8]
    ";
        private readonly IDictionary<string, ValueObject> args;
        public MainArgs(ICollection<string> argv, bool help = true, object version = null, bool optionsFirst = false, bool exit = false)
        {
            args = new Docopt().Apply(Usage, argv, help, version, optionsFirst, exit);
            Options = new Option(args);
            Arguments = new Argument(args);
        }

        public Option Options { get; set; }
        public Argument Arguments { get; set; }
        public IDictionary<string, ValueObject> Args => args;
    }
}