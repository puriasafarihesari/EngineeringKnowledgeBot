using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoreBot1
{
    public class StreamProject
    {
        public static bool TryGetBestMatchingProject(string project, out SpecleMesh matchingMesh)
        {
            if(project != null)
            {
                project = project.ToLower();
            }

            matchingMesh = null;
            if (project == "bridge1")
            {
                return false;
            }
            else if (project == "bridge2")
            {
                matchingMesh = new SpecleMesh();
                matchingMesh.Vertices = new List<double>()
                {
                    0,0,10,
                    10,0,0,
                    10,10,0,
                    0,10,0
                };

                matchingMesh.Faces = new List<int>()
                {
                    1,0,1,2,3
                };
                return true;
            }
            else if (project == "bridge3")
            {
                return false;
            }
            else if (project == "bridge4")
            {
                matchingMesh = new SpecleMesh();
                matchingMesh.Vertices = new List<double>()
                {
                    0,0,10,
                    10,0,20,
                    20,10,0,
                    0,10,0
                };

                matchingMesh.Faces = new List<int>()
                {
                    1,0,1,2,3
                };
                return true;
            }
            return false;
        }

        public static string GetCSV(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();
            return results;
        }

        public static string GetParametricUrl(string levels, string curviness)
        {
            if (curviness == null)
            {
                curviness = "54";
            }
            else
            {
                if (curviness == "low")
                {
                    curviness = "4";
                }
                else if (curviness == "medium")
                {
                    curviness = "14";
                }
                else if (curviness == "high")
                {
                    curviness = "54";
                }
                else
                {
                    curviness = "4";
                }
            }
            if (levels == null)
            {
                levels = "5";
            }

            string compiledUrl = "http://grot.rvba.fr/models/" + levels + "-" + curviness + ".json";
            //string compiledUrl = "http://grot.rvba.fr/models/proj1.json";
            return compiledUrl;
        }

        public static void StreamBuilding(string levels, string curviness)
        {
            string route = "https://speckle.continuum.codes/api/streams/MfNWI67wx";

            string compiledUrl = GetParametricUrl(levels, curviness);
            var proj1Json = GetCSV(compiledUrl);
            string response = ApiRequest.ApiRequestWithAuth(RequestType.PUT, ResponseType.String, route, proj1Json, false);
            //return compiledUrl;
        }

        public static void Stream(string project)
        {
            string route = "https://speckle.continuum.codes/api/streams/MfNWI67wx";

            string url = "http://grot.rvba.fr/models/proj1.json";
            string peopleUrl = "http://grot.rvba.fr/models/dummyDatabase.json";
            var proj1Json = GetCSV(url);
            var peopleJson = GetCSV(peopleUrl);
            DataBase._persons = JsonConvert.DeserializeObject<List<Person>>(peopleJson);

            string response = ApiRequest.ApiRequestWithAuth(RequestType.PUT, ResponseType.String, route, proj1Json, false);



            SpecleMesh matchingMesh = null;
            if (TryGetBestMatchingProject(project, out matchingMesh))
            {
                SpeckleInput input = new SpeckleInput();
                input.Objects.Add(matchingMesh);
                //string response = ApiRequest.ApiRequestWithAuth(RequestType.PUT, ResponseType.String, route, input);
            }
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

    public class SpecleMesh : SpeckleObject
    {
        public SpecleMesh()
        {
            this.Type = "Mesh";
            this.Vertices = new List<double>();
            this.Faces = new List<int>();
        }

        [JsonProperty("vertices")]
        public List<double> Vertices { get; set; }

        [JsonProperty("faces")]
        public List<int> Faces { get; set; }

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
