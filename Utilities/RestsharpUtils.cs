using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace API_BDD_Framwork.Utilities
{
    public class RestsharpUtils
    {
        string BaseUrl = ConfigurationManager.AppSettings["ClientUrl"];
        RestClient client;
        RestRequest request;

        public RestClient SetApiUrl(string endpoint)
        {
            client = new RestClient(BaseUrl);
            return client;
        }

        public RestRequest CreateGetRequest(string endpoint, Dictionary<string, string> cookies = null,
            Dictionary<string, string> parameters = null, Dictionary<string, string> headers = null,
            Dictionary<string, string> urlSegments = null, string body = null)
        {
            request = new RestRequest(endpoint, Method.GET);

            if (null != body)
            {
                request = SetRequestBody(body, request);
            }

            if (null != cookies)
            {
                request = AddCookies(request, cookies);
            }

            if (null != parameters)
            {
                request = AddQueryParameters(request, parameters);
            }

            if (null != headers)
            {
                request = AddHeaders(request, headers);
            }

            if (null != urlSegments)
            {
                request = AddUrlSegments(request, urlSegments);
            }
            return request;
        }

        
        public RestResponse GetResponse(RestClient client , RestRequest request)
        {
            var response = client.Execute(request);
            return (RestResponse)response;
        }

        public T GetResponseAsJson<T>(RestResponse response) where T : new()
        {
            JsonDeserializer deserializer = new JsonDeserializer();
            var jsonResponse = deserializer.Deserialize<T>(response);
            return jsonResponse;
        }

        public string GetResponseAsText(RestResponse response)
        {
            return response.Content;
        }

        private RestRequest AddParameters(RestRequest request, Dictionary<string, string> parameters, ParameterType type)
        {
           if (null != parameters && 0 != parameters.Count)   parameters.Select(e => request.AddParameter(e.Key, e.Value, type));
           var urlhere = client.BuildUri(request);
           return request;
        }

        private RestRequest AddUrlSegments(RestRequest request, Dictionary<string, string> urlSegments)
        {
            foreach (var segment in urlSegments)
            {
                request.AddUrlSegment(segment.Key, segment.Value.ToString());
            }
            return request;
        }

        private RestRequest AddCookies(RestRequest request, Dictionary<string, string> cookies)
        {
            foreach (var cookie in cookies)
            {
                request.AddCookie(cookie.Key, cookie.Value);
            }
            return request;
        }

        private RestRequest SetRequestBody(string body, RestRequest request)
        {
            if (body.StartsWith("{")) request.AddJsonBody(body);
            if (body.StartsWith("<")) request.AddXmlBody(body);
            return request;
        }

        private RestRequest AddQueryParameters(RestRequest request, Dictionary<string, string> parameters)
        {
            foreach (var param in parameters)
            {
                request.AddQueryParameter(param.Key, param.Value);
            }
            return request;
        }

        private RestRequest AddHeaders(RestRequest request, Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                request.AddHeader(header.Key, header.Value.ToString());
            }
            return request;
        }

        private void AuthenticateRequest(IRestClient client, IRestRequest request)
        {
            /* TODO: for future impelementation*/


        }

    }
}
