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
        [NUnit.Framework.TestCaseAttribute("log 1h10m id \"Hi mom\"", "Hi mom", "70", "today", new string[0])]
        [NUnit.Framework.TestCaseAttribute("log 10h10m id Hi mom", "Hi mom", "610", "today", new string[0])]
        [NUnit.Framework.TestCaseAttribute("log 10m id work --when 2016-01-31", "work", "10", "2016-01-31", new string[0])]
        [NUnit.Framework.TestCaseAttribute("log 1h id \"work\"", "work", "60", "today", new string[0])]
        [NUnit.Framework.TestCaseAttribute("log 1h id work  --when 2010-02-18", "work", "60", "2010-02-18", new string[0])]
        [NUnit.Framework.TestCaseAttribute("log 10h id \"work very hard\"", "work very hard", "600", "today", new string[0])]
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
 testRunner.And("I execute log log 1h10m id --when 2016-01-31 Hi mom", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
 testRunner.And("I execute log log 8h30m id --when 2015-01-31 Working Hard", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
 testRunner.When("I execute show show logs ticket id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 21
 testRunner.Then("message is shown", "| user         | date       | hours   | message      |\r\n|--------------|---------" +
                    "---|---------|--------------|\r\n| Peter Jensen | 2016-01-31 | 1.2     | Hi mom   " +
                    "    |\r\n| Peter Jensen | 2015-01-31 | 8.5     | Working Hard |", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("show words for current user")]
        public virtual void ShowWordsForCurrentUser()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("show words for current user", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 29
 testRunner.Given("I have a ticket", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 30
 testRunner.And("I execute log log 1h10m id design of the new api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
 testRunner.And("I execute log log 4h id implementation of the api and unit tests", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
 testRunner.And("I execute log log 1h10m id testing", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
 testRunner.And("I execute log log 3h10m id refactoring", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
 testRunner.And("I execute log log 2h40m id deployment and testing", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
 testRunner.And("I execute log log 1h id fixing an isue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.And("I execute log log 8h30m id --when 2015-01-31 fixing an issue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
 testRunner.When("I execute show show words", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 38
 testRunner.Then("message is shown", @"| word                         | percent |
|------------------------------|---------|
| hi                           | 26.0    |
| mom                          | 24.0    |
| system.collections.arraylist | 20.0    |
| fixing                       | 12.0    |
| testing                      | 10.0    |
| api                          | 8.0     |
| tests                        | 8.0     |
| unit                         | 6.0     |
| issue                        | 4.0     |
| design                       | 4.0     |
| new                          | 4.0     |
| implementation               | 4.0     |
| refactoring                  | 4.0     |
| deployment                   | 4.0     |
| isue                         | 4.0     |
| working                      | 2.0     |
| hard                         | 2.0     |
| database                     | 2.0     |
| schema                       | 2.0     |
| writing                      | 2.0     |
| test                         | 2.0     |
| added                        | 2.0     |
| bug                          | 2.0     |
| moe                          | 2.0     |
| work                         | 2.0     |
| commited                     | 2.0     |", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("show all time")]
        public virtual void ShowAllTime()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("show all time", ((string[])(null)));
#line 70
this.ScenarioSetup(scenarioInfo);
#line 71
 testRunner.Given("I have a ticket", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 72
 testRunner.Given("I have another  ticket", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 73
 testRunner.And("I execute log log 1h10m id --when 2016-01-31 Hi mom", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
 testRunner.And("I execute log log 8h30m id --when 2015-01-31 Working Hard", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.And("I execute log log 8h30m another-id --when 2015-01-30 Working Hard", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
 testRunner.When("I execute show show time my", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 77
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
