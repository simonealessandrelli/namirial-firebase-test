using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Namirial.Firebase.Test.Utils {
    public class DataConverterHelper {

        private DateTime _dateTime;

        public DataConverterHelper(DateTime datetime) {
            _dateTime = datetime;
        }

        public int GetIntDate() {
            int value = _dateTime.Year * 10000 + _dateTime.Month * 100 + _dateTime.Day;
            return value;
        }

        public int GetUnixEpoch() {
            System.TimeSpan timeDifference = _dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return System.Convert.ToInt32(timeDifference.TotalSeconds);
        }
    }
}
