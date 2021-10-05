using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class CalcParamInputModel
    {
        public int FormId { get; set; }
        public string Structure { get; set; }
        public List<ParameterInputModel> parameters { get; set; }
    }
}
