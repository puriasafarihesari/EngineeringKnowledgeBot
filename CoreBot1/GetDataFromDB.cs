using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot1
{
    public class GetDataFromDB
    {
        public static string FindPersonWithSkill(string skill)
        {
            string person = "";
            foreach (Person p in DataBase._persons)
            {
                if (p.Expertise == skill)
                    person = person + p.Name + Environment.NewLine;
            }
            return person;
        }
    }
}
