using System.Collections.Generic;
using DocoptNet;

namespace Gemini.Commander.Core
{
    public class MainArgs
    {
        public const string Usage = @"
Usage:
    gemini log <time> <id> [--when=<date>] [--log-type=<type>] <message>... 
    gemini create ticket (<project> <state> <title>...) [--parent=<id>]
    gemini show project all
    gemini show logs
    gemini show logs (project | ticket) <id> [--from=<date>] [--to=<date>]
    gemini show logs user <username> [--from=<date>]  [--to=<date>]
    gemini show ticket (assigned | <id>)
    gemini show hours [by <user>] [--from=<date>] [--to=<date>] [--working-hours=<hours>]  
    gemini show words (all | trend | everyone) [--stemmed] [--from=<date>] [--to=<date>] [--user=<id>] [--take=<number>]
    gemini show stats [--from=<date>] [--to=<date>] [--user=<id>]

Options:
    -h --help                 Show this screen.
    --when=<date>             The date for wen time log entry YYYY-MM-DD [default:now]
    --log-type=<type>         Type of timelogging billable [default:30]
    --from=<date>             The first inclussive date of the time period [default:today-30days]
    --to=<date>               The last inclussive date of the time period [default:today]
    --working-hours=<hours>   The number of working hours in a working day [default:8]
    --stemmed                 Enables porter stemming of the words.
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