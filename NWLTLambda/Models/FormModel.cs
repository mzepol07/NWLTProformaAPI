using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class FormModel
    {
        public int city_id { get; set; }
        public int form_id { get; set; }
        public string form_name { get; set; }
        public string form_creator { get; set; }
        public string form_creation_date { get; set; }
        public string mResponseMessage { get; set; }
    }
}
