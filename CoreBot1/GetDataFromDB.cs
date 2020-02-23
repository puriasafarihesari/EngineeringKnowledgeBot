using System;
using System.Reflection;
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
                if (p.Expertise.ToLower().Contains(skill))
                    person = person + p.Name + Environment.NewLine;
            }
            return person;
        }

        public static string FindWord(string word, string language)
        {
            string translatedWord = "";

            foreach (Language l in DataBase._language)
            {
                if (l.ENGLISH.ToLower() == word || l.ITALIANO.ToLower() == word || l.FRANCAIS.ToLower() == word || l.DEUTSCH.ToLower() == word)
                {
                    switch (language.ToUpper())
                    {
                        case "ENGLISG":
                            translatedWord = l.ENGLISH;
                            break;
                        case "FRENCH":
                            translatedWord = l.FRANCAIS;
                            break;
                        case "ITALIAN":
                            translatedWord = l.ITALIANO;
                            break;
                        case "GERMAN":
                            translatedWord = l.DEUTSCH;
                            break;
                        default:
                            translatedWord = "Unfortunately I don't know what that word means";
                            break;
                    }
                }
            }
            return translatedWord;
        }

        public static string FindProjcetByTypology(string typo)
        {
            string projString = "";
            foreach (Project p in DataBase._project)
            {
                if (p.Typology != null && p.Typology.ToLower().Contains(typo))
                    projString = projString + p.projectName + Environment.NewLine;
            }
            return projString;
        }
    }
}
