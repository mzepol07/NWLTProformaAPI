using System;
using System.Collections.Generic;
using System.Text;

namespace NWLTLambda.Models
{
    public class ParametersList
    {
        public string State { get; set; }
        public int City { get; set; }
        public string Address { get; set; }
        public int YearBuilt { get; set; }
        public double LotSize { get; set; }
        public string Zoning { get; set; }
        public string MHASuffix { get; set; }
        public double AccessedValue { get; set; }
        public double LotWidth { get; set; }
        public double LotLength { get; set; }
        //11
        public string HighEndArea { get; set; }
        public string MajorArterial { get; set; }
        public string CornerLot { get; set; }
        public string Alley { get; set; }
        public string GrowthArea { get; set; }
        public string ElevationChange { get; set; }
        public string SlopeDirection { get; set; }
        public string TreeCanopy { get; set; }
        public string ECAs { get; set; }
        public string WaterMainExtension { get; set; }
        //21
        public double WaterMainLength { get; set; }
        public string SewerMainExtension { get; set; }
        public double SewerMainLength { get; set; }
        public string AlleyReconstruction { get; set; }
        public double AlleyReconstructionLength { get; set; }
        public string UndergroundGarage { get; set; }
        public int NumberofSpots { get; set; }
        public string StructureType { get; set; }
        public string MHaArea { get; set; }
        public int NumberofHomes { get; set; }
        //31
        public double ListingPrice { get; set; }
        public double AADU { get; set; }
        public double AADUSize { get; set; }
        public double DADU { get; set; }
        public double DADUSize { get; set; }
        public double EstimatedValue { get; set; }
        public double SFEstimatedValue { get; set; }
        public double ADUEstimatedValue { get; set; }
        public double DADUEstimatedValue { get; set; }
        public double ConstructionCost { get; set; }
        // 41
        public double SalesCommission { get; set; }
        public double ExciseTax { get; set; }
        public double EscrowFees { get; set; }
        public double MarketingClearers { get; set; }
        public double Profit { get; set; }
        public double HomesLandValue { get; set; }
        public double PropertiesLandValue { get; set; }
        public double TotalFinancingFees { get; set; }
        public double FinancingFeesPerHome { get; set; }
        public double NetProfitPerHome { get; set; }        
        // 51
        public double TotalNetProfit { get; set; }        
        public double TotalAcquisition { get; set; }
        public double SoftCost { get; set; }
        public double HarCost { get; set; }
        public double Equity { get; set; }
        public double AcquisitionCarryingMonths { get; set; }
        public double InterestRateLandAcquisition { get; set; }
        public double TotalFinancingCostLandAcquisition { get; set; }
        public double InterestRateConstructionCost { get; set; }
        public double LoanFeesConstructionCost { get; set; }
        // 61
        public double ConstructionLoanMonths { get; set; }
        public double TotalFinancingCostConstruction { get; set; }
        public double AssignmentFees { get; set; }
        public double MonthToSellAferCompletion { get; set; }
        public double ExtraFinancingAfterCompletion { get; set; }
        public double TotalFinancingFees2 { get; set; }
    }
}
