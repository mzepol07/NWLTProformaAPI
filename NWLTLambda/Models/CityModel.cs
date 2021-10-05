using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class CityModel
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string StateAbbrev { get; set; }
        public string Region { get; set; }
        public string TimeZone { get; set; }
        public string mResponseMessage { get; set; }
    }
}
