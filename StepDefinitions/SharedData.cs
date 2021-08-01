using System.Net;

namespace API_BDD_Framwork.StepDefinitions
{
    public class SharedData
    {
        public HttpStatusCode statusCode;
        public dynamic response;
        public string suppressCode;
        public SharedData()
        {
            
        }
    }
}
