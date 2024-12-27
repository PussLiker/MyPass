using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mypass.Model
{
    public class History
    {
        private static List<string> HistoryList = new List<string>();
        public static void addToHistory(string anyevent)
        {
            DateTime now = DateTime.Now;
            anyevent = "["+ now.GetDateTimeFormats()[31] + "] " + anyevent;
            HistoryList.Add(anyevent);
        }
        public static List<string> getHistory()
        {
            return HistoryList;
        }
    }
    
}
