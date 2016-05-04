using System.Linq;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Gemini.Commander.Core;
using Gemini.Commander.Core.Extensions;
using Format = ConsoleTables.Core.Format;
using static Gemini.Commander.Core.Extensions.Ext;

namespace Gemini.Commander.Commands
{
    [Command("show", "words", "all")]
    public class ShowWordsCommand : ServiceManagerCommand
    {
        public ShowWordsCommand(ServiceManager svc) : base(svc) { }
        
        public override void Execute(MainArgs args)
        {
            var take = args.Options.Take;
            var user = Svc.Item.WhoAmI();
            var items = Svc.LogsByUser(user, args);

            var words = items.AllWords()
                .Select(Clean)
                .Select(Trim)
                .Select(Stem(args.Options.Stemmed))
                .Where(Allowed)
                .GroupBy(m => m)
                .OrderByDescending(m => m.Count())
                .Select(m => new { m.Key, pct = (m.Count() * 100m / items.Count()) })
                .ToList();

            var table = new ConsoleTable("word", "percent");

            words
                .Select(x => new object[]
                {
                    x.Key,x.pct.ToString("F1")
                })
                .Take(take)
                .ToList()
                .ForEach(x => table.AddRow(x));
            
            table.Write(Format.MarkDown);
        }
    }
}