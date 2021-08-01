using API_BDD_Framwork.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace API_BDD_Framwork.StepDefinitions
{
    [Binding]
    public class SharedSteps
    {
        private SharedData sharedData;
        private Type responseType;
        public SharedSteps(SharedData data)
        {
            sharedData = data;
        }

        [Given(@"The user wants to set suppress response code to (.*)")]
        public void GivenTheUserWantsToSetSuppressResponseCodeTo(string suppressValue)
        {
            sharedData.suppressCode = suppressValue;
        }

        [Then(@"the status code should be (.*)")]
        public void ThenTheStatusCodeShouldBe(int expectedStatusCode)
        {

            Assert.AreEqual(expectedStatusCode, (int)sharedData.statusCode, "The status code is not matching");
        }

        [Then(@"The response should be")]
        public void ThenTheResponseShouldBe(Table table)
        {
            responseType = sharedData.response.GetType();
            if(responseType.Equals(typeof(SatellitePosition)))
            {
                SatellitePosition position  = table.CreateInstance<SatellitePosition>();
                SatellitePosition returnedPosition = sharedData.response.First();
                position.Should().BeEquivalentTo(returnedPosition);
            }
            else if (responseType.Equals(typeof(Tles)))
            {
                Tles tle = table.CreateInstance<Tles>();
                Tles returnedTle = sharedData.response;
                tle.Should().BeEquivalentTo(returnedTle);
            }
        }

        [Then(@"the error message should be ""(.*)""")]
        public void ThenTheErrorMessageShouldBe(string expectedMessage)
        {
            responseType = sharedData.response.GetType();
            if (responseType.Equals(typeof(BadRequestRes)))
            {
                var actualErrorMsg = sharedData.response.error;
                Assert.AreEqual(expectedMessage, actualErrorMsg, "The error message is not corresponding the received one");
            }
            else
            {
                var actualErrorMsg = sharedData.response;
                StringAssert.Contains(expectedMessage, actualErrorMsg);
            }
        }
    }


}
