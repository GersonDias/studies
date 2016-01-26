using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSample.Model
{
    public class TFSUsers
    {
        public int Id { get; set; }
        
        public string UserAgent { get; set; }

        public string UserName { get; set; }

        public DateTime CommandDate { get; set; }

        public long Quantity { get; set; }

        public TFSUsers(string userName, string userAgent, DateTime commandDate, long quantity)
        {
            UserName = userName;
            UserAgent = userAgent;
            CommandDate = commandDate;
            Quantity = quantity;
        }

        public TFSUsers(){}
    }
}
