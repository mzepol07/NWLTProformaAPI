using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class AllParametersOutputModel
    {
        public double ParameterId { get; set; }
        public string ParameterName { get; set; }
        public string HtmlHeader { get; set; }
        public string HtmlTag { get; set; }
        public string Phase { get; set; }
        public string Stage { get; set; }
        public int ParameterTypeId { get; set; }
        public double ParamOrder { get; set; }
        public string mResponseMessage { get; set; }
    }
}
