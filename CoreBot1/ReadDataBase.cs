using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot1
{
    public class ReadDataBase
    {
        public static void ReadData()
        {
            var reader = new StreamReader("Dummydatabase.csv");
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                Person person = new Person(values[0], values[4].ToLower(), values[3], values[1], values[2]);
                DataBase._persons.Add(person);
                
            }
        }
    }
}
