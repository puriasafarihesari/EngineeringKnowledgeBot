using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot1
{
    public class StreamProject
    {
        public static void Stream(string project)
        {
            string route = "https://speckle.continuum.codes/api/streams/a3KF2UUr_";

            var test = new SpecklePlane(SpeckleObject.CreatePoint(0, 0, 0),
                     SpeckleObject.CreateVector(0, 0, 1),
                     SpeckleObject.CreateVector(1, 0, 0),
                     SpeckleObject.CreateVector(0, 1, 0));

            SpeckleInput input = new SpeckleInput();
            input.Objects.Add(test);
            
            string response = ApiRequest.ApiRequestWithAuth(RequestType.PUT, ResponseType.String, route, input);
            //dynamic response = ApiRequest.ApiRequest("POST", "json", ssoServer + "/api/users/authenticate/token", values);
        }

    }

    public class SpeckleInput
    {
        [JsonProperty("objects")]
        public List<SpeckleObject> Objects { get; set; }

        public SpeckleInput()
        {
            this.Objects = new List<SpeckleObject>();

        }
    }

    public class SpecklePlane : SpeckleObject
    {
        public SpecklePlane(SpeckleObject origin, SpeckleObject normal,
            SpeckleObject xdir, SpeckleObject ydir)
        {
            this.Type = "Plane";
            this.Normal = normal;
            this.XDir = xdir;
            this.YDir = ydir;
        }

        [JsonProperty("xdir")]
        public SpeckleObject XDir { get; set; }

        [JsonProperty("ydir")]
        public SpeckleObject YDir { get; set; }

        [JsonProperty("normal")]
        public SpeckleObject Normal { get; set; }

    }
    
    public class SpecleObjectWithValue : SpeckleObject
    {
        [JsonProperty("value")]
        public object Value { get; set; }
    }

    public class SpeckleObject
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        public static SpeckleObject CreateVector(double x, double y, double z)
        {
            return new SpecleObjectWithValue()
            {
                Type = "Vector",
                Value = new List<double>() { x, y, z }
            };
        }

        public static SpeckleObject CreatePoint(double x, double y, double z)
        {
            return new SpecleObjectWithValue()
            {
                Type = "Point",
                Value = new List<double>() { x, y, z }
            };
        }
        
    }
    

}
