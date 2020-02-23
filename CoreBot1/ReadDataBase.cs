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
            string peopleUrl = "http://grot.rvba.fr/models/dummyDatabase.json";
            var peopleJson = StreamProject.GetCSV(peopleUrl);
            DataBase._persons = JsonConvert.DeserializeObject<List<Person>>(peopleJson);
            string jsonString = File.ReadAllText("wordDatabase.json");
            DataBase._language = JsonConvert.DeserializeObject<List<Language>>(jsonString);
        }
    }
}
