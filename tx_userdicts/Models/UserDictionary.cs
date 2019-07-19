using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tx_userdicts.Models
{
    public class UserDictionary
    {
        public string[] words { get; set; }
        public string language { get; set; }
        public string name { get; set; }
    }
}