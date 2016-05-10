using System;
using FluentAssertions;
using Gemini.Commander.Core;
using NUnit.Framework;

namespace Gemini.Commander.Spec.Tests
{
    [TestFixture]
    class OptionParserTests
    {
        [TestCase("create ticket 12 1 testing --parent=1", 1)]
        [TestCase("create ticket 12 1 testing --parent=200", 200)]
        [TestCase("create ticket 12 1 testing", null)]
        public void should_parse_parent_option(string text, int? expected)
        {
            new MainArgs(text.Split(' ')).Options.Parent.ShouldBeEquivalentTo(expected);
        }

        [TestCase("log 1h 10 testing --when 2016-04-12", 2016, 04, 12)]
        [TestCase("log 1h 10 --when 2016-03-31 testing ", 2016, 3, 31)]
        public void should_parse_when_option(string text, int year, int month, int day)
        {
            new MainArgs(text.Split(' ')).Options.When.ShouldBeEquivalentTo(new DateTime(year, month, day));
        }

        [TestCase("log 1h 10 testing")]
        public void should_parse_when_option_default(string text)
        {
            new MainArgs(text.Split(' ')).Options.When.Date.ShouldBeEquivalentTo(DateTime.Today.Date);
        }

        [TestCase("show hours all --working-hours 10", 10)]
        [TestCase("show hours all --working-hours 0", 0)]
        [TestCase("show hours all", 8)]
        public void should_parse_working_hours_option(string text, int expected)
        {
            new MainArgs(text.Split(' ')).Options.WorkingHours.ShouldBeEquivalentTo(expected);
        }

        [TestCase("show words all --stemmed", true)]
        [TestCase("show words all", false)]
        public void should_parse_stemmed_option(string text, bool expected)
        {
            new MainArgs(text.Split(' ')).Options.Stemmed.ShouldBeEquivalentTo(expected);
        }
    }
}