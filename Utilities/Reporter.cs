using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_BDD_Framwork.Utilities
{
    public static class Reporter
    {
        public static ExtentReports extentReport;
        public static ExtentReports extent;
        public static ExtentHtmlReporter htmlReporter;
        public static ExtentTest scenario;

        public static void ConfigureExtentReport(string reportName, string title, dynamic path)
        {
            htmlReporter = new ExtentHtmlReporter(path+"index.html");
            htmlReporter.Config.Theme = Theme.Dark;
            htmlReporter.Config.DocumentTitle = title;
            htmlReporter.Config.ReportName = reportName;
            extentReport = new ExtentReports();
            extentReport.AttachReporter(htmlReporter);
           // extentReport = extent;
        }

        public static ExtentTest CreateTest(string testTitle)
        {
            return extentReport.CreateTest(testTitle);
        }

        public static ExtentTest CreateScenario(ExtentTest feature, string scenarioName)
        {
            return feature.CreateNode(scenarioName);

        }

        public static ExtentTest CreateGivenStep(string status, ExtentTest scenario, string stepName, string errorMsg = null)
        {
            return status == "pass" ? scenario.CreateNode<Given>(stepName) : scenario.CreateNode<Given>(stepName).Fail(errorMsg);

        }

        public static ExtentTest CreateWhenStep(string status, ExtentTest scenario, string stepName, string errorMsg = null)
        {
            return status == "pass" ? scenario.CreateNode<When>(stepName) : scenario.CreateNode<When>(stepName).Fail(errorMsg);

        }

        public static ExtentTest CreateThenStep(string status, ExtentTest scenario, string stepName, string errorMsg = null)
        {
            return scenario.CreateNode<Then>(stepName);

        }

        public static ExtentTest CreateAndStep(string status, ExtentTest scenario, string stepName, string errorMsg = null)
        {
            return status == "pass" ? scenario.CreateNode<And>(stepName) : scenario.CreateNode<And>(stepName).Fail(errorMsg);

        }
        public static void AttachToReport(Status status, string msg)
        {
            scenario.Log(status, msg);
        }

        public static void FlushReport()
        {
            extentReport.Flush();
        }
    }
}
