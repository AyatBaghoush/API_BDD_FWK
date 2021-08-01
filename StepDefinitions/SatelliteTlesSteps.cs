using API_BDD_Framwork.Constants;
using API_BDD_Framwork.Controller;
using API_BDD_Framwork.Model;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace API_BDD_Framwork.StepDefinitions
{
    [Binding]
    public class SatelliteTlesSteps
    {
        private readonly ApiController controller = new ApiController();
        private readonly SharedData sharedData;
        private int rateLimit;
        public SatelliteTlesSteps(SharedData data)
        {
            sharedData = data;
        }

        [When(@"the satellite tles is requested for satellite id= (.*) with a (.*) response")]
        public void WhenTheSatelliteTlesIsRequestedForSatelliteIdWithATextResponse(int id, string format)
        {
            sharedData.response = controller.GetSatelliteTles(id, sharedData.suppressCode, out sharedData.statusCode, out rateLimit, format);
        }

        [Then(@"the (.*) response should contain (.*)")]
        public void ThenTheResponseShouldContain(string format, string value)
        {
            if(ResponseFormat.TEXT == format.ToUpper())
            {
                string res = sharedData.response;
                StringAssert.Contains(value, res, "The text is not contained inside the API response!");
            }
            else
            {
                Tles tles = sharedData.response;
                Assert.AreEqual(value, tles.header, "The header does not = " + value);
            }
        }

        [When(@"Get Satellite tles API is called for number of times that exceeds the rate limit")]
        public void WhenGetSatelliteTlesAPIIsCalledForNumberOfTimesThatExceedsTheRateLimit()
        {
            var id = 25544;

            sharedData.response = controller.GetSatelliteTles(id: id, sharedData.suppressCode, statusCode: out sharedData.statusCode, rateLimit: out rateLimit);
            for (int i = 0; i < rateLimit; i++)
            {
                sharedData.response = controller.GetSatelliteTles(id, sharedData.suppressCode, out sharedData.statusCode, out rateLimit);
            }
        }

    }
}
