using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class SFOutputModel
    {
        public double FarMax { get; set; }
        public double SFHome { get; set; }
        public double SFBuildingCost { get; set; }
        public double SFADUBuildingCost { get; set; }
        public double ConstructionCost { get; set; }

        public double SalesCommission { get; set; }
        public double ExciseTax { get; set; }
        public double Escrow { get; set; }
        public double MarketingClearners { get; set; }
        public double Profit { get; set; }
        public double HomeLandValue { get; set; }
        public double PropertyLandValue { get; set; }
        public double TotalFinancingFees { get; set; }
        public double NetProfitPerHome { get; set; }
        public double TotalNetProfit { get; set; }
        public string mResponseMessage { get; set; }
    }
}
