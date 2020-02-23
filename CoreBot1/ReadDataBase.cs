using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace CoreBot1
{
    public class ReadDataBase
    {
        public static string GetCSV(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
        }

        public static void ReadData()
        {
            string url = "http://grot.rvba.fr/models/Dummydatabase.csv";
            string s = GetCSV(url);

            var splitString = s.Split('\n');

            foreach (string line in splitString)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var values = line.Split(';');
                if(values.Length > 0)
                {
                Person person = new Person(values[0], values[4].ToLower(), values[3], values[1], values[2]);
                DataBase._persons.Add(person);
                }
            }
        }
    }
}
