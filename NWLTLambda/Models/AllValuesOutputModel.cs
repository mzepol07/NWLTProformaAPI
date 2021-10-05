using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class AllValuesOutputModel
    {
        public double PropertyID { get; set; }
        public string FormName { get; set; }
        public string CityName { get; set; }
        public string FormCreationDate { get; set; }
        public string ParameterID { get; set; }
        public string ParameterName { get; set; }
        public string Phase { get; set; }
        public string Stage { get; set; }
        public string Value { get; set; }
        public string mResponseMessage { get; set; }
    }
}
