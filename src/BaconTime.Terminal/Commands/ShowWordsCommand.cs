using System;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleTables.Core;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Entity;
using Iveonik.Stemmers;
using Format = ConsoleTables.Core.Format;

namespace BaconTime.Terminal.Commands
{
    [Command("show", "words")]
    public class ShowWordsCommand : ServiceManagerCommand
    {
        public ShowWordsCommand(ServiceManager svc) : base(svc) { }

        private static string Trim(string e) => e.ToLower().Trim(',', ';', ':', '.', '-', '+', '\r', '\n');

        private static Func<string, string> Stem(bool stemmed) => e => stemmed ? new EnglishStemmer().Stem(e) : e;

        private static bool Allowed(string e)
        {
            var stopwords = ", ; . : / - + * meeting marc mustafa abdeljalil jacob flemming anders mohamed a about above after again against all am an and any are aren't as at be because been before being below between both but by can't cannot could couldn't did didn't do does doesn't doing don't down during each few for from further had hadn't has hasn't have haven't having he he'd he'll he's her here here's hers herself him himself his how how's i i'd i'll i'm i've if in into is isn't it it's its itself let's me more most mustn't my myself no nor not of off on once only or other ought our ours 	ourselves out over own same shan't she she'd she'll she's should shouldn't so some such than that that's the their theirs them themselves then there there's these they they'd they'll they're they've this those through to too under until up very was wasn't we we'd we'll we're we've were weren't what what's when when's where where's which while who who's whom why why's with won't would wouldn't you you'd you'll you're you've your yours yourself yourselves af alle andet andre at begge da de den denne der deres det dette dig din dog du ej eller en end ene eneste enhver et fem fire flere fleste for fordi forrige 	fra få før god han hans har hendes her hun hvad hvem hver hvilken hvis hvor hvordan hvorfor hvornår i ikke ind ingen intet jeg jeres kan kom kommer lav lidt lille man 	mand mange med meget men mens mere mig ned ni nogen noget ny nyt nær næste næsten og op otte over på se seks ses som stor store syv ti til to tre ud var".Split(' ');
            return !stopwords.Contains(e) && !string.IsNullOrWhiteSpace(e) && e.Length > 1;
        }

        private static string Clean(string e) => Regex.Replace(e.ToLower(), @"\s+", " ");

        public override void Execute(MainArgs args)
        {
            var take = args.Options.Take;
            var user = Svc.Item.WhoAmI();
            var items = Svc.Item.GetFilteredItems(new IssuesFilter
            {
                IncludeClosed = true,
                TimeLoggedBy = user.Entity.Id + ""
            }).SelectMany(x => x.TimeEntries.Where(e => e.Entity.UserId == user.Entity.Id));

            var words = items.SelectMany(e =>
                e.Entity.Comment.Split(' ')
                    .Select(Clean)
                    .Select(Trim)
                    .Select(Stem(args.Options.Stemmed))
                    .Where(Allowed)
                )
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