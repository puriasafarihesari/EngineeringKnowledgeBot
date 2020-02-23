using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot1
{
    public class Project
    {
        public string Typology { get; set; }

        public string Material { get; set; }

        public double minSpan { get; set; }

        public double maxSpan { get; set; }

        public string projectName { get; set; }

        public string executionMethod { get; set; }

        public Project(string typo, string mat, int mSpan, int mxSpan, string pName, string exMethod)
        {
            Typology = typo;
            Material = mat;
            minSpan = mSpan;
            maxSpan = mxSpan;
            projectName = pName;
            executionMethod = exMethod;
        }
    }
}
