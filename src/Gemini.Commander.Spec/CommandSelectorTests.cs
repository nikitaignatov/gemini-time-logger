using System;
using DocoptNet;
using FluentAssertions;
using Gemini.Commander.Commands;
using Gemini.Commander.Core;
using NUnit.Framework;

namespace Gemini.Commander.Spec
{
    [TestFixture]
    class CommandSelectorTests
    {
        [TestCase("log 1m30h 24242 hi mom", typeof(LogTimeCommand))]
        [TestCase("show hours", typeof(ShowHoursCommand))]
        public void should_select_correct_command(string text, Type type)
        {
            var command = CommandRunner.LoadCommand(new MainArgs(text.Split(' ')).Args, new[] { typeof(LogTimeCommand), typeof(ShowHoursCommand), });
            command.ShouldBeEquivalentTo(type);
        }

        [TestCase("log 1m30h 24242 hi mom", typeof(LogTimeCommand))]
        [TestCase("show hours", typeof(ShowHoursCommand))]
        public void should_throw_when_command_not_defined(string text, Type type)
        {
            Action result = () => CommandRunner.LoadCommand(new MainArgs(text.Split(' ')).Args, new[] { typeof(string) });
            result.ShouldThrow<Exception>();
        }

        [TestCase("dup", typeof(DupCommand))]
        public void should_throw_when_multiple_command_are_assigned_to_same_name(string text, Type type)
        {
            var d = new Docopt().Apply("Usage:command dup", text.Split(' '));
            Action result = () => CommandRunner.LoadCommand(d, new[] { typeof(DupCommand), typeof(Dup2Command), });
            result.ShouldThrow<Exception>();
        }

        [Command("dup")]
        public class DupCommand : ICommand { public void Execute(MainArgs args) => Console.WriteLine("dup"); }

        [Command("dup")]
        public class Dup2Command : ICommand { public void Execute(MainArgs args) => Console.WriteLine("dup2"); }
    }
}
