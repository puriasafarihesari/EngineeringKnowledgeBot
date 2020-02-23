using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot1
{
    public class Language
    {
        public string ENGLISH { get; set; }

        public string DEUTSCH { get; set; }

        public string FRANCAIS { get; set; }

        public string ITALIANO { get; set; }

        public Language(string eng, string ger, string fre, string ita)
        {
            ENGLISH = eng;
            DEUTSCH = ger;
            FRANCAIS = fre;
            ITALIANO = ita;
        }
    }
}
