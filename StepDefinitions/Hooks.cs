using API_BDD_Framwork.Utilities;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace API_BDD_Framwork.StepDefinitions
{
    
    [Binding]
    public sealed class Hooks
    {
        private static ExtentTest featureName;
        private static ExtentTest scenario;


        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            string path1 = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "");
            string path = path1 + "Report\\";
            Reporter.ConfigureExtentReport("API_Report", "BDD API Tests", path);
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            featureName = Reporter.CreateTest(FeatureContext.Current.FeatureInfo.Title);
            
        }

        [BeforeScenario]
        public static void BeforeScenario()
        {
            scenario = Reporter.CreateScenario(featureName, ScenarioContext.Current.ScenarioInfo.Title);
        }

       
        [AfterStep]
        public static void AfterStep()
        {
            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            if (ScenarioContext.Current.TestError == null)
            {
                if (stepType == "Given")
                    Reporter.CreateGivenStep("pass", scenario, ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "When")
                    Reporter.CreateWhenStep("pass", scenario, ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "Then")
                    Reporter.CreateThenStep("pass", scenario, ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "And")
                    Reporter.CreateAndStep("pass", scenario, ScenarioStepContext.Current.StepInfo.Text);
            }
           else if (ScenarioContext.Current.TestError != null)
            {
                if (stepType == "Given")
                {
                    Reporter.CreateGivenStep("fail", scenario, ScenarioStepContext.Current.StepInfo.Text, ScenarioContext.Current.TestError.Message);
                }
                else if (stepType == "When")
                {
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                }
                else if (stepType == "Then")
                {
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                }
                else if (stepType == "And")
                {
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                }
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Reporter.FlushReport();
        }
    }
}
