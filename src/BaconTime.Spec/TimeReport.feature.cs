﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.0.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace BaconTime.Spec
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("TimeReport")]
    public partial class TimeReportFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "TimeReport.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "TimeReport", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("log time for ticket")]
        [NUnit.Framework.TestCaseAttribute("--cmd log -t id -h 1 -m 10 -c \"Hi mom\"", "Hi mom", "70", "today", new string[0])]
        [NUnit.Framework.TestCaseAttribute("--cmd log -t id -m 10 -c \"work\" -d 2016-01-31", "work", "10", "2016-01-31", new string[0])]
        [NUnit.Framework.TestCaseAttribute("--cmd log -t id -h 1 -c \"work\"", "work", "60", "today", new string[0])]
        [NUnit.Framework.TestCaseAttribute("--cmd log -t id -c work -h 1 -d 2010-02-18", "work", "60", "2010-02-18", new string[0])]
        [NUnit.Framework.TestCaseAttribute("--cmd log -t id -h 10 -c \"work very hard\"", "work very hard", "600", "today", new string[0])]
        [NUnit.Framework.TestCaseAttribute("--cmd log -t id -m 1 -c  hi", "hi", "1", "today", new string[0])]
        public virtual void LogTimeForTicket(string command, string comment, string totalMinutes, string date, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("log time for ticket", exampleTags);
#line 3
this.ScenarioSetup(scenarioInfo);
#line 4
 testRunner.Given("I have a ticket", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 5
 testRunner.When(string.Format("I execute log {0}", command), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 6
 testRunner.Then(string.Format("{0}, {1} and the {2} is added.", totalMinutes, date, comment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("show time for ticket")]
        public virtual void ShowTimeForTicket()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("show time for ticket", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 17
 testRunner.Given("I have a ticket", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 18
 testRunner.And("I execute log --cmd log -t id -h 1 -m 10 -d 2016-01-31 -c \"Hi mom\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
 testRunner.And("I execute log --cmd log -t id -h 8 -m 30 -d 2015-01-31 -c \"Working Hard\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
 testRunner.When("I execute show --cmd show -t id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 21
 testRunner.Then("message is shown", "| user         | date       | hours   | message      |\r\n|--------------|---------" +
                    "---|---------|--------------|\r\n| Peter Jensen | 2016-01-31 | 1.2     | Hi mom   " +
                    "    |\r\n| Peter Jensen | 2015-01-31 | 8.5     | Working Hard |", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("show all time")]
        public virtual void ShowAllTime()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("show all time", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line 30
 testRunner.Given("I have a ticket", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 31
 testRunner.Given("I have another  ticket", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 32
 testRunner.And("I execute log --cmd log -t id -h 1 -m 10 -d 2016-01-31 -c \"Hi mom\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
 testRunner.And("I execute log --cmd log -t id -h 8 -m 30 -d 2015-01-31 -c \"Working Hard\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
 testRunner.And("I execute log --cmd log -t another-id -h 8 -m 30 -d 2015-01-30 -c \"Working Hard\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
 testRunner.When("I execute show --cmd show-all", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 36
 testRunner.Then("message is shown", @"| user         | id         | date       | hours   | message      |
|--------------|------------|------------|---------|--------------|
| Peter Jensen | id         | 2016-01-31 | 1.2     | Hi mom       |
| Peter Jensen | id         | 2015-01-31 | 8.5     | Working Hard |
| Peter Jensen | another-id | 2015-01-30 | 8.5     | Working Hard |", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
