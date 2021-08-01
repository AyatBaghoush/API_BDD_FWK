using API_BDD_Framwork.Controller;
using API_BDD_Framwork.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace API_BDD_Framwork.StepDefinitions
{
    [Binding]
    public class SatellitePositionsSteps
    {

        private readonly ApiController controller = new ApiController();
        private SatellitePositionReq satellitePositionReq;
        private readonly SharedData sharedData;
        private long expectedTimeStamp;
        private int rateLimit;
        private string suppressCode = "false";
        
        public SatellitePositionsSteps(SharedData data)
        {
            satellitePositionReq = new SatellitePositionReq();
            
            sharedData = data;
        }


        [When(@"Get Satellite API is called for the satellite id = (.*) and unit = (.*) and timestamps = (.*)")]
        public void WhenGetSatelliteAPIIsCalledForTheSatelliteIdAndUnitAndTimestamps(int id, string unit, string timestamps)
        {
            satellitePositionReq.Id = id;
            satellitePositionReq.Unit = unit;
            satellitePositionReq.TimeStamps = timestamps;
            satellitePositionReq.SuppressCode = sharedData.suppressCode;
            sharedData.response = controller.GetSatellitePositions(satellitePositionReq, out sharedData.statusCode, out rateLimit);
        }


        [When(@"Get Satellite API is called for the satellite id = (.*) at a past timestamp")]
        public void WhenGetSatelliteAPIIsCalledForTheSatelliteIdAtAPastTimestamp(int id)
        {
            DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
            expectedTimeStamp = now.ToUnixTimeSeconds() - 500000;
            satellitePositionReq.Id = id;
            satellitePositionReq.SuppressCode = suppressCode;
            satellitePositionReq.TimeStamps = expectedTimeStamp.ToString();
            satellitePositionReq.SuppressCode = suppressCode;
            sharedData.response = controller.GetSatellitePositions(satellitePositionReq, out sharedData.statusCode, out rateLimit);
        }


        [When(@"Get Satellite API is called for the satellite id = (.*) at a future timestamp")]
        public void WhenGetSatelliteAPIIsCalledForTheSatelliteIdAtAFutureTimestamp(int id)
        {
            DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
            expectedTimeStamp = now.ToUnixTimeSeconds() + 500000;
            satellitePositionReq.Id = id;
            satellitePositionReq.TimeStamps = expectedTimeStamp.ToString();
            satellitePositionReq.SuppressCode = suppressCode;
            sharedData.response = controller.GetSatellitePositions(satellitePositionReq, out sharedData.statusCode, out rateLimit);
        }
    

       
        [Then(@"the number of provided positions should be equivalent to requested timestamps (.*)")]
        public void ThenTheNumberOfProvidedPositionsShouldBeEquivalentToRequestedTimestamps(string timestamps)
        {
            int expectedNumberOfReqStamps = timestamps.Trim().Split(',').Count();
            List<SatellitePosition> res = sharedData.response;
            int actualNumOfReturnedPositions = res.Count();
            Assert.AreEqual(expectedNumberOfReqStamps, actualNumOfReturnedPositions, "The number of returned positions does not equal to the number of requested timestamps");
        }


        [Then(@"All the returned positions should have ""(.*)"" as a default unit")]
        public void ThenAllTheReturnedPositionsShouldHaveAsADefaultUnit(string expectedUnit)
        {
            VerifyResponseUnit(expectedUnit);
        }

        
        [Then(@"the response contains the position of the requested timestamp")]
        public void ThenTheResponseContainsThePositionOfTheRequestedTimestamp()
        {
            List<SatellitePosition> res = sharedData.response;
            Assert.AreEqual(1, res.Count(), "The response does not contain one position");
            var actualTimestamp = res.First().timestamp;
            Assert.AreEqual(expectedTimeStamp, actualTimestamp, "The returned position does not correspond to the requested timestamp");
        }


        [Then(@"The returned units matches with the requested (.*)")]
        public void ThenTheReturnedUnitsMatchesWithTheRequestedOne(string expectedUnit)
        {
            VerifyResponseUnit(expectedUnit);
        }

        [When(@"Get Satellite API is called for number of times that exceeds the rate limit")]
        public void WhenGetSatelliteAPIIsCalledForNumberOfTimesThatExceedsTheRateLimit()
        {
            satellitePositionReq.Id = 25544;
            satellitePositionReq.Unit = "miles";
            satellitePositionReq.TimeStamps = "1436029892";
            satellitePositionReq.SuppressCode = suppressCode;
            sharedData.response = controller.GetSatellitePositions(satellitePositionReq, out sharedData.statusCode, out rateLimit);
            for (int i=0; i<rateLimit; i++)
            {
                sharedData.response = controller.GetSatellitePositions(satellitePositionReq, out sharedData.statusCode, out rateLimit);
            }
            
        }




        private void VerifyResponseUnit(string expectedUnit)
        {
            List<SatellitePosition> res = sharedData.response;
            foreach (SatellitePosition position in res)
            {
                StringAssert.AreEqualIgnoringCase(expectedUnit, position.units, "The unit is not " + expectedUnit);
            }
        }

    }
}
