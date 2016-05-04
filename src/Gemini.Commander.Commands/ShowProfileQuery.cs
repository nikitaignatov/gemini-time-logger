using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Countersoft.Gemini.Api;
using Countersoft.Gemini.Commons.Dto;
using Flagger;
using Gemini.Commander.Commands.Flags;
using Gemini.Commander.Commands.Rules;
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

        private Dictionary<string, int> Tokenize(string data) => data
            .Split(' ', '\r', '\n')
            .Normalized()
            .GroupBy(Ext.Id)
            .ToDictionary(t => t.Key, t => t.Count());


        public override dynamic Execute(MainArgs args)
        {
            //var items = Svc.LogsByEveryone(args);
            var items = JsonConvert.DeserializeObject<List<IssueTimeTrackingDto>>(File.ReadAllText("C:\\temp\\dump_times.json"))
                                    .Where(x => x.Entity.EntryDate > DateTime.Today.AddDays(-200)).GroupBy(x => x.Fullname).Select(x => new { id = x.Key, doc = string.Join(" ", x.Select(m => m.Entity.Comment)) });
            var documents = items
                .Select(x => new { x.id, doc = Tokenize(x.doc) })
                .ToList();

            var vocab = documents
                .SelectMany(x => x.doc.Keys)
                .Distinct()
                .ToList();

            var tf = documents
                .SelectMany(x => x.doc)
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => new
                {
                    tf = Math.Log(1 + x.Sum(t => t.Value)).Round(3),
                    n = x.Sum(t => t.Value)
                });

            var df = vocab.ToDictionary(x => x, x => new
            {
                df = documents.Count(d => d.doc.ContainsKey(x)),
                idf = Math.Log(documents.Count() / (double)documents.Count(d => d.doc.ContainsKey(x)))
            });

            var weighted = documents.Select(x => new { x.id, doc = x.doc.ToDictionary(m => m.Key, m => Math.Log(1 + m.Value) * df[m.Key].idf) });

            Console.SetBufferSize(100, 30000);

            Console.WriteLine(JsonConvert.SerializeObject(tf.OrderByDescending(x => x.Value.n).Take(10).Select(x => new { x.Key, entries = x.Value }), Formatting.Indented));
            //Console.WriteLine(JsonConvert.SerializeObject(weighted.ToList().Take(5), Formatting.Indented));

            var profiles = typeof(DevOpsFlag).Assembly.ExportedTypes
                  .Where(x => x.IsSubclassOf(typeof(ProfileFlag)))
                  .Select(Activator.CreateInstance)
                  .OfType<ProfileFlag>()
                  .Select(profile => new { profile, tokens = Tokenize(profile.Profile) });

            foreach (var profile in profiles)
            {
                var query = profile.profile.Profile;// "testing merge deploy implement review investigate fix commit design spec";
                var tokens = Tokenize(query);

                var result = weighted
                    .Select(x => new { profile.profile.Description, score = tokens.Sum(t => x.doc.ContainsKey(t.Key) ? x.doc[t.Key] : 0), x.id })
                    .OrderByDescending(x => x.score)
                    .Where(x=>x.score>1)
                    .GroupBy(x => x.Description)
                    .ToDictionary(x => x.Key, x => x.Select(m => new { m.id, score= m.score.Round() }));

                Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            }
            return null;
        }
    }
}