using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Dto;
using Flagger;
using Gemini.Commander.Commands.Flags;
using Gemini.Commander.Core;
using Gemini.Commander.Core.Extensions;
using Newtonsoft.Json;

namespace Gemini.Commander.Commands
{
    public class Profile
    {
        public IDictionary<string, Dictionary<string, decimal>> Words { get; set; }
        public IEnumerable<IFlag> Flags { get; set; }
    }

    public class ShowProfileQuery : ServiceManagerQuery<dynamic>
    {
        public ShowProfileQuery(ServiceManager svc) : base(svc) { }

        public override dynamic Execute(MainArgs args)
        {
            // var items = Svc.LogsByEveryone(args)
            var items = JsonConvert.DeserializeObject<IEnumerable<IssueTimeTrackingDto>>(File.ReadAllText("c:\\temp\\dump_times.json"))
                        .Where(x => x.Entity.EntryDate > DateTime.Today.AddDays(-300))
                        .GroupBy(x => x.Fullname)
                        .Select(x => new
                        {
                            id = x.Key,
                            doc = string.Join(" ", x.Select(m => m.Entity.Comment))
                        });

            var documents = items
                .Select(x => x.doc.ToDocument(new MetaData { User = x.id }))
                .ToArray();

            var vocab = documents.ToVocabulary();
            var df = vocab.ToDocumentFrequency(documents);

            var weighted = documents.Select(x => new
            {
                x.MetaData,
                doc = x.Tokens.ToDictionary(m => m.Key, m => Math.Log(1 + m.Value) * df[m.Key].IDF)
            })
            .Select(x => new { x.MetaData, x.doc, sumsq = x.doc.Values.SqrtSumSquares() })
            .Select(x => new
            {
                x.MetaData,
                x.doc,
                l2norm = x.doc.ToDictionary(d => d.Key, d => d.Value / x.sumsq)
            }).ToList();

            Console.SetBufferSize(100, 30000);

            foreach (var profile in Profiles())
            {
                var query = profile.Profile;
                var tokens = query.Tokenize();

                var result = weighted
                    .Select(x => new { profile.Description, score = tokens.Sum(t => x.doc.ContainsKey(t.Key) ? x.l2norm[t.Key] : 0), x.MetaData })
                    .OrderByDescending(x => x.score)
                    .GroupBy(x => x.Description)
                    .ToDictionary(x => x.Key, x => x.Select(m => new { m.MetaData.User, score = m.score.Round(3) }));

                Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            }
            return null;
        }

        public static IEnumerable<ProfileFlag> Profiles() => typeof(DevOpsFlag).Assembly.ExportedTypes
                  .Where(x => x.IsSubclassOf(typeof(ProfileFlag)))
                  .Select(Activator.CreateInstance)
                  .OfType<ProfileFlag>();
    }
}