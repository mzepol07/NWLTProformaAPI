using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class StructureTypesVM
    {
        public string StructureType { get; set; }
        public List<CalcParameterOutputModel> Calculations { get; set; }
    }
}
