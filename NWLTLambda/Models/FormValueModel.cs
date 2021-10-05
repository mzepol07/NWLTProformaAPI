using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class FormValueModel
    {
        public int property_id { get; set; }
        public string form_name { get; set; }
        public string city_name { get; set; }
        public string form_creation_date { get; set; }
        public int parameter_id { get; set; }
        public string parameter_name { get; set; }
        public string value { get; set; }
        public string mResponseMessage { get; set; }
    }
}
