using System;
using System.Linq;
using FluentAssertions;
using Gemini.Commander.Core;
using Gemini.Commander.Core.Extensions;
using NUnit.Framework;

namespace Gemini.Commander.Spec
{
    [TestFixture]
    public class TextProcessingTests
    {
        [TestCase("tested tests", "username", new[] { 1, 1 })]
        [TestCase("tested", "username", new[] { 1 })]
        [TestCase("tested tested", "username", new[] { 2 })]
        [TestCase("tested tested tested", "username", new[] { 3 })]
        [TestCase("testing tests tested tested", "username", new[] { 1, 1, 2 })]
        public void should_count_term_frequency_in_the_given_document(string text, string username, int[] expected)
        {
            text.ToDocument(new MetaData { User = username }).Tokens.Values.ShouldAllBeEquivalentTo(expected);
        }

        [Test]
        public void should_generate_vocabulary_from_multiple_documents_and_exclude_stopwords()
        {
            var input = new[]
            {
                "testing tests works testing" ,
                "added tests to project",
                "tested scenarios"
            };
            var expected = new[] { "testing", "tests", "works", "added", "project", "tested", "scenarios" }.OrderBy(x => x);
            input.Select(x => x.ToDocument(new MetaData { User = "user" }))
                .ToVocabulary()
                .OrderBy(x => x)
                .ShouldBeEquivalentTo(expected);
        }
    }
}