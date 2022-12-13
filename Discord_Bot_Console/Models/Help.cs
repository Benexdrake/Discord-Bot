using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_Console.Models
{
    public class Help
    {
        public string CommandName { get; set; }
        public string[] Parameters { get; set; }
        public string Description { get; set; }
    }
}
