using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot1
{
    /// <summary>
    /// HTTP Request Types
    /// </summary>
    public enum RequestType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public enum ResponseType
    {
        String = 0,
        JSON = 1
    }

    public class ApiRequest
    {
        private static string key = "JWT eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJfaWQiOiI1ZGYzNzczOGY0YTU1MDA3OWUyY2Q4ZmEiLCJpYXQiOjE1NzYyMzY4NTYsImV4cCI6MTYzOTM1MjA1Nn0.julewKh3RbjF_wzSBSuMX7frGvaESXkJdHckx0ra6W0";

        public static dynamic ApiRequestWithAuth(RequestType requestType, ResponseType responseType, string route, object jsonObject)
        {
            string jsonArguments = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.Authorization] = key;
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                return StandardizeApiRequest(client, requestType, responseType, route, jsonArguments);
            }
        }

        public static dynamic StandardizeApiRequest(WebClient client, RequestType requestType, ResponseType responseType, string url, string jsonArguments)
        {
            var bytes = Encoding.Default.GetBytes(jsonArguments);
            var responseString = "";
            dynamic responseJson = null;

            try
            {
                if (requestType == RequestType.POST)
                {
                    var response = client.UploadData(url, "POST", bytes);
                    responseString = Encoding.Default.GetString(response);
                }
                else if (requestType == RequestType.GET)
                {
                    responseString = client.DownloadString(url);
                }
                else if (requestType == RequestType.PUT)
                {
                    var response = client.UploadData(url, "PUT", bytes);
                }

                responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseString);
            }
            catch (Exception e)
            {
                object a = e;

                // do nothing
            }

            if (responseType == ResponseType.String)
            {
                return responseString;
            }
            else
            {
                return responseJson;
            }
        }

    }
        
}
