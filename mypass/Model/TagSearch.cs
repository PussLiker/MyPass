using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mypass.Model
{
    public static class TagSearch
    {
        public static List<string> toNumberSort(List<Dictionary<string, string>> dictionaryList)
        {
            int k = 1;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> list = new List<string>();
            for (int i = 0; i < (int)dictionaryList.LongCount(); i++)
            {
                dictionary = dictionaryList.ElementAt(i);
                list.Add(dictionary.ElementAt(1).Value);
                k++;
            }
            return list;
        }
        public static List<string> toAZsort(List<Dictionary<string, string>> dictionaryList)
        {
            List<string> list = toNumberSort(dictionaryList);
            list = list.OrderBy(item => item, StringComparer.CurrentCulture).ToList();
            return list;
        }
        public static void init(string resourcepath)
        {
            TagsDB tagsDB = new TagsDB(resourcepath);
            //tagsDB.AddTag("social net");
            //tagsDB.AddTag("messenger");
            //tagsDB.AddTag("wikipedias");
            toAZsort(tagsDB.GetAllTags());
        }
    }
}
