using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class Report
    {
        public int Id { get; private set; }
        public string UserName { get; private set; }
        public string Comment { get; set; }
        public string Profit { get; private set; }

        public Report(int id, string userName, string comment, string profit)
        {
            Id = id;
            UserName = userName;
            Comment = comment;
            Profit = profit;
        }
    }
}
