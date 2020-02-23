using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace CoreBot1
{
    public class ReadDataBase
    {
        public static void ReadData()
        {
            string dummyDBJson = File.ReadAllText("dummyDatabase.json");
            DataBase._persons = JsonConvert.DeserializeObject<List<Person>>(dummyDBJson);
            string wordDBJson = File.ReadAllText("wordDatabase.json");
            DataBase._language = JsonConvert.DeserializeObject<List<Language>>(wordDBJson);
            string projectDBJson = File.ReadAllText("projetDatabase.json");
            DataBase._project = JsonConvert.DeserializeObject<List<Project>>(projectDBJson);
        }
    }
}
