using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSample.Model
{
    public class tblCommand
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string UserAgent { get; set; }

        public DateTime StartTime { get; set; }

        public int Quantity { get; set; }
    }
}
