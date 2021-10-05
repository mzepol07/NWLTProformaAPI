using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class SecretDBModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string engine { get; set; }
        public string host { get; set; }
        public uint port { get; set; }
        public string dbInstanceIdentifier { get; set; }
        public string dbname { get; set; }
    }
}
