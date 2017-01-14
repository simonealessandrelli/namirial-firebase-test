using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Namirial.Firebase.Test {
    public class NotifyTestParam {
        NotifyTestParam() { }
        public string Type { get; set; }
        public string Token { get; set; }
        public int Attempts { get; set; }
        public int Delay { get; set; }
    }
}
