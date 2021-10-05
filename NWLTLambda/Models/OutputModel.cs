using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class OutputModel
    {
        public int FormId { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public string InputUser { get; set; }
        public List<ParameterInputModel> FormValues { get; set; }
        public List<StructureTypesVM> StructureTypes { get; set; }
        public string MessageText { get; set; }
    }
}

