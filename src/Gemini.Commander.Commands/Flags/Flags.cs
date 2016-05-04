using System.Collections.Generic;
using Flagger;

namespace Gemini.Commander.Commands.Flags
{
    public abstract class ProfileFlag : Flag
    {
        public abstract string Profile { get; }
    }

    public class HotFixerFlag : ProfileFlag
    {
        public override string Description { get; } = "Hotfix";
        public override FlagCategory Category { get; } = FlagCategory.Danger;
        public override int Points { get; } = 10;
        public override string Profile { get; } = "hot hotfix fix release deploy";
    }

    public class QuickfixFlag : ProfileFlag
    {
        public override string Description { get; } = "Quickfix";
        public override FlagCategory Category { get; } = FlagCategory.Warning;
        public override int Points { get; } = 10;
        public override string Profile { get; } = "quick quickfix fix seems works";
    }

    public class ManagerFlag : ProfileFlag
    {
        public override string Description { get; } = "Manager";
        public override FlagCategory Category { get; } = FlagCategory.Info;
        public override int Points { get; } = 10;
        public override string Profile { get; } = "misq meeting talk discuss plan requirment";
    }

    public class TesterFlag : ProfileFlag
    {
        public override string Description { get; } = "Tester";
        public override FlagCategory Category { get; } = FlagCategory.Info;
        public override int Points { get; } = 10;
        public override string Profile { get; } = "tested testing check fix works";
    }

    public class AumtatedTestsFlag : ProfileFlag
    {
        public override string Description { get; } = "Automated tests";
        public override FlagCategory Category { get; } = FlagCategory.Info;
        public override int Points { get; } = 10;
        public override string Profile { get; } = "test tests cucumber spec specflow nunit junit unit tdd run execute";
    }

    public class DevOpsFlag : ProfileFlag
    {
        public override string Description { get; } = "DevOps";
        public override FlagCategory Category { get; } = FlagCategory.Info;
        public override int Points { get; } = 10;
        public override string Profile { get; } = "deployed released implemented merge merged review investigate logs";
    }

    public class DeveloperFlag : ProfileFlag
    {
        public override string Description { get; } = "Developer";
        public override FlagCategory Category { get; } = FlagCategory.Info;
        public override int Points { get; } = 10;
        public override string Profile { get; } = "implementation implement implemented tests test review develop development change refactor commit fix";
    }

    public class ProgrammerFlag : ProfileFlag
    {
        public override string Description { get; } = "Programmer";
        public override FlagCategory Category { get; } = FlagCategory.Info;
        public override int Points { get; } = 10;
        public override string Profile { get; } = "commit implement build script function object reference db jenkins";
    }

    public class Flags : Flag
    {
        public override string Description { get; } = "Work life balance";
        public override FlagCategory Category { get; } = FlagCategory.Info;
        public override int Points { get; } = 10;
    }

    public class LongHoursFlag : Flag
    {
        public override string Description { get; } = "Long hours";
        public override FlagCategory Category { get; } = FlagCategory.Warning;
        public override int Points { get; } = 10;
    }
}
