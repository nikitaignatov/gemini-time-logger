using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemini.Commander.Core.Extensions
{
    public class Document
    {
        public Document()
        {
            TFIDF = new Dictionary<string, double>();
        }
        public MetaData MetaData { get; set; }
        public string Value { get; set; }
        public Dictionary<string, int> Tokens { get; set; }
        public Dictionary<string, double> TFIDF { get; set; }
    }

    public class TermStat
    {
        public double TFW { get; set; }
        public int TF { get; set; }
        public int DF { get; set; }
        public double IDF { get; set; }
    }

    public class MetaData
    {
        public MetaData()
        {
            Attributes = new Dictionary<string, string>();
            Metrics = new Dictionary<string, int>();
            MetricsNormalized = new Dictionary<string, double>();
        }
        public string User { get; set; }
        public string Project { get; set; }
        public string Ticket { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public Dictionary<string, int> Metrics { get; set; }
        public Dictionary<string, double> MetricsNormalized { get; set; }
    }

    public static class TextProcessing
    {
        public static Dictionary<string, int> Tokenize(this string data)
            => data
            .Split(' ', '\r', '\n', ',', '.')
            .Normalized()
            .GroupBy(Ext.Id)
            .ToDictionary(t => t.Key, t => t.Count());

        public static Document ToDocument(this string data, MetaData meta)
            => new Document
            {
                MetaData = meta,
                Value = data,
                Tokens = data.Tokenize()
            };

        public static string[] ToVocabulary(this IEnumerable<Document> documents)
            => documents.SelectMany(x => x.Tokens.Keys).Distinct().ToArray();

        public static Dictionary<string, TermStat> ToTermFrequency(this IEnumerable<Document> documents)
            => documents.SelectMany(x => x.Tokens.Select(t => new { t, x }))
                .GroupBy(x => x.t.Key)
                .ToDictionary(x => x.Key, x => new TermStat
                {
                    TFW = Math.Log(1 + x.Sum(t => t.t.Value)).Round(3),
                    TF = x.Sum(t => t.t.Value),
                });

        public static Dictionary<string, TermStat> ToDocumentFrequency(this string[] vocabulary, Document[] documents, Dictionary<string, TermStat> tf)
            => vocabulary.ToDictionary(x => x, x => new TermStat
            {
                DF = documents.Count(d => d.Tokens.ContainsKey(x)),
                IDF = Math.Log(documents.Count() / (double)documents.Count(d => d.Tokens.ContainsKey(x))),
                TF = tf[x].TF,
                TFW = tf[x].TFW,
            });

        public static Dictionary<string, TermStat> ToDocumentFrequency(this string[] vocabulary, Document[] documents)
            => vocabulary.ToDocumentFrequency(documents, documents.ToTermFrequency());
    }
}
