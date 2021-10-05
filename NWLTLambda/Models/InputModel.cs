using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class InputModel
    {
        public string address { get; set; }
        public string username { get; set; }
        public string CityId { get; set; }
        public List<ParameterInputModel> parameters { get; set; }
    }
}
