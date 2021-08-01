using API_BDD_Framwork.Constants;
using API_BDD_Framwork.Model;
using API_BDD_Framwork.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace API_BDD_Framwork.Controller
{
    public class ApiController
    {
        RestsharpUtils restsharpUtils;
        Dictionary<string, string> pathParams;
        Dictionary<string, string> queryParams;
        public ApiController()
        {
            restsharpUtils = new RestsharpUtils();
            
        }
        public dynamic GetSatellitePositions(SatellitePositionReq satellitePositionReq, out HttpStatusCode statusCode, out int rateLimit)
        {
            pathParams = new Dictionary<string, string>();
            queryParams = new Dictionary<string, string>();
            var client = restsharpUtils.SetApiUrl(EndPoints.GetSatellitePosition);

            pathParams.Add("id", satellitePositionReq.Id.ToString());
            if (!string.IsNullOrEmpty(satellitePositionReq.Unit)) queryParams.Add("units", satellitePositionReq.Unit);
            if (!string.IsNullOrEmpty(satellitePositionReq.TimeStamps)) queryParams.Add("timestamps", satellitePositionReq.TimeStamps);
            if ("true" == satellitePositionReq.SuppressCode) queryParams.Add("suppress_response_codes", satellitePositionReq.SuppressCode);

            var request = restsharpUtils.CreateGetRequest(EndPoints.GetSatellitePosition, parameters: queryParams, urlSegments: pathParams);
            var response = restsharpUtils.GetResponse(client, request);
            rateLimit = int.Parse(response.Headers.Where(x => x.Name == "X-Rate-Limit-Limit").Select(x => x.Value).FirstOrDefault().ToString());
            statusCode = response.StatusCode;
            if (statusCode == HttpStatusCode.OK && "true" != satellitePositionReq.SuppressCode)
            {
                return restsharpUtils.GetResponseAsJson<List<SatellitePosition>>(response);
            }
            else
            {
                return restsharpUtils.GetResponseAsJson<BadRequestRes>(response);
            }
            
        }

        public dynamic GetSatelliteTles(int id , string suppressCode, out HttpStatusCode statusCode, out int rateLimit, string format = ResponseFormat.NONE)
        {
            pathParams = new Dictionary<string, string>();
            queryParams = new Dictionary<string, string>();
            var client = restsharpUtils.SetApiUrl(EndPoints.GetSatelliteTles);
            pathParams.Add("id", id.ToString());
            if (!string.IsNullOrEmpty(format)) queryParams.Add("format", format);
            if ("true" == suppressCode) queryParams.Add("suppress_response_codes", suppressCode);

            var request = restsharpUtils.CreateGetRequest(EndPoints.GetSatelliteTles, parameters: queryParams, urlSegments: pathParams);
            var response = restsharpUtils.GetResponse(client, request);
            rateLimit = int.Parse(response.Headers.Where(x => x.Name == "X-Rate-Limit-Limit").Select(x => x.Value).FirstOrDefault().ToString());

            statusCode = response.StatusCode;
            if (format.ToUpper() == ResponseFormat.TEXT)
            {
                return restsharpUtils.GetResponseAsText(response);
            }
            else if (statusCode == HttpStatusCode.OK && "true" != suppressCode)
            {
                return restsharpUtils.GetResponseAsJson<Tles>(response);
            }
            else
            {
                return restsharpUtils.GetResponseAsJson<BadRequestRes>(response);
            }
                         
        }

       
    }
}
