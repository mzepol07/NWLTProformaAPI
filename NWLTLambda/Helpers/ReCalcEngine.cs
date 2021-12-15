using NWLTLambda.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;


// TODO: Set FAR variables to be changed by the front-end
// TODO: add structure type to recalc engine

namespace NWLTLambda
{
    public class ReCalcEngine
    {
        public double ExciseOutput(double SalesPrice)
        {
            double result = 0;
            if (SalesPrice <= 500000)
            {
                result = 0.011 * SalesPrice;
            }
            else if (SalesPrice > 500000 && SalesPrice <= 1500000)
            {
                result = (500000 * 0.011) + ((SalesPrice - 500000) * 0.0128);
            }
            else if (SalesPrice > 1500000 && SalesPrice <= 3000000)
            {
                result = (500000 * 0.011) + (1000000 * 0.0128) + ((SalesPrice - 1500000) * 0.0275);
            }
            else if (SalesPrice > 3000000)
            {
                result = (500000 * 0.011) + (1000000 * 0.0128) + (1500000 * 0.0275) + ((SalesPrice - 3000000) * 0.03);
            }
            return result;
        }
        public StructureTypesVM ReCalcProforma(ParametersList mParams)
        {
            {
                StructureTypesVM mOutModel = new StructureTypesVM();
                mySqlAccess _sqlaccess = new mySqlAccess();

                const double mFactor1 = 1.07;
                const double mFactor2 = 0.06;
                const double mFactor3 = 0.0175;
                const double mFactor4 = 0.0075;
                const double mFactor5 = 0.005;
                const double mFactor6 = 0.25;
                if (mParams.Aadu == 0)
                {
                    mParams.Aadu = 1;
                    mParams.Aadu_size = 1000;
                }
                if (mParams.Aadu_size == 0)
                {
                    mParams.Aadu_size = 1000;
                }
                if (mParams.Dadu == 0)
                {
                    mParams.Dadu = 1;
                    mParams.Dadu_size = 1000;
                }
                if (mParams.Dadu_size == 0)
                {
                    mParams.Dadu_size = 1000;
                }


                List<CalcParameterOutputModel> mCalcList = new List<CalcParameterOutputModel>();
                CalcParameterOutputModel mSingleCalculation = new CalcParameterOutputModel();
                StructureTypesVM mCalcOutput = new StructureTypesVM();
                mCalcOutput.StructureType = mParams.StructureType;

                //  TODO --- You will have to replace  mParams.StructureType with mStructureType  to run the next 3 calculations
                decimal BuildCost = _sqlaccess.GetBaseCost(mParams.CityId, mParams.HighEndArea, mParams.StructureType);
                decimal mFAR = _sqlaccess.GetFAR(mParams.CityId, mParams.Zoning, mParams.StructureType, mParams.MhaSuffix, mParams.GrowthArea);
                decimal mHaFee = _sqlaccess.GetMHAFee(mParams.CityId, mParams.Mhaarea, mParams.MhaSuffix);
                decimal mDig = _sqlaccess.GetDensityLimit(mParams.CityId, mParams.Zoning, mParams.StructureType, mParams.MhaSuffix, mParams.CornerLot);

                decimal FarMax = Convert.ToDecimal(mParams.LotSize) * mFAR;
                // decimal SFHome = Convert.ToDouble(FarMax) / mParams.Number_of_homes * Convert.ToDecimal(mFactor1);

                if (mFAR != 0)
                {
                    double CostPerGrade = (mParams.CostPerGrade == 0 ? 1 : mParams.CostPerGrade);
                    double WaterLineCostPerSqft = (mParams.WaterLineCostPerSqft == 0 ? 1000 : mParams.WaterLineCostPerSqft);
                    double SewerCostPerSqft = (mParams.SewerCostPerSqft == 0 ? 1000 : mParams.SewerCostPerSqft);
                    double StormDrainCostPerSqft = (mParams.StormDrainCostPerSqft == 0 ? 1000 : mParams.StormDrainCostPerSqft);
                    double AlleyCostPerSqft = (mParams.AlleyCostPerSqft == 0 ? 500 : mParams.AlleyCostPerSqft);
                    double UndergroundParkingCostPerStall = (mParams.UndergroundParkingCostPerStall == 0 ? 50000 : mParams.UndergroundParkingCostPerStall);
                    double mSellableSqftFactor = (mParams.SellableSqftFactor == 0 ? 1.07 : mParams.SellableSqftFactor);
                    double mTownhomeBaseCostPerSqft = (mParams.TownhomeBaseCostPerSqft == 0 ? 250 : mParams.TownhomeBaseCostPerSqft);
                    double mSfBaseCostPerSqft = (mParams.SfBaseCostPerSqft == 0 ? 240 : mParams.SfBaseCostPerSqft);
                    double mApartmentBaseCostPerSqft = (mParams.ApartmentBaseCostPerSqft == 0 ? 275 : mParams.ApartmentBaseCostPerSqft);
                    double mRowhouseBaseCostPerSqft = (mParams.RowhouseBaseCostPerSqft == 0 ? 250 : mParams.RowhouseBaseCostPerSqft);
                    double mCondoBaseCostPerSqft = (mParams.CondoBaseCostPerSqft == 0 ? 275 : mParams.CondoBaseCostPerSqft);
                    double mCottageBaseCostPerSqft = (mParams.CottageBaseCostPerSqft == 0 ? 250 : mParams.CottageBaseCostPerSqft);
                    double mSfEstimatedValue = (mParams.SfEstimatedValue == 0 ? 2000000 : mParams.SfEstimatedValue);
                    double mDesiredProfitPercent = (mParams.DesiredProfitPercent == 0 ? 0.2 : mParams.DesiredProfitPercent);
                    double mEquity = (mParams.Equity == 0 ? 200000 : mParams.Equity);
                    double mAcquisitionCarryingMonths = (mParams.AcquisitionCarryingMonths == 0 ? 12 : mParams.AcquisitionCarryingMonths);
                    double mInterestRateLandAcquisition = (mParams.InterestRateLandAcquisition == 0 ? 0.06 : mParams.InterestRateLandAcquisition);
                    double mLoanFeesLandAcquisitionLoan = (mParams.LoanFeesLandAcquisitionLoan == 0 ? 0.01 : mParams.LoanFeesLandAcquisitionLoan);
                    double mInterestRateConstructionLoan = (mParams.InterestRateConstructionLoan == 0 ? 0.06 : mParams.InterestRateConstructionLoan);
                    double mLoanFeesConstructionLoan = (mParams.LoanFeesConstructionLoan == 0 ? 0.01 : mParams.LoanFeesConstructionLoan);
                    double mMonthsToSellAfterCompletion = (mParams.MonthsToSellAfterCompletion == 0 ? 3 : mParams.MonthsToSellAfterCompletion);
                    double mConstructionLoanMonths = (mParams.ConstructionLoanMonths == 0 ? 12 : mParams.ConstructionLoanMonths);
                    double mHomeEstimatedValue = (mParams.HomeEstimatedValue == 0 ? 1000000 : mParams.HomeEstimatedValue);
                    double mUnitEstimatedValue = (mParams.UnitEstimatedValue == 0 ? 500000 : mParams.UnitEstimatedValue);
                    double mInefficientSpacePercentage = (mParams.InefficientSpacePercentage == 0 ? 0.15 : mParams.InefficientSpacePercentage);
                    double mApartmentCapRate = (mParams.ApartmentCapRate == 0 ? 0.03 : mParams.ApartmentCapRate);
                    double mMonthlyRentPerUnit = (mParams.MonthlyRentPerUnit == 0 ? 2000 : mParams.MonthlyRentPerUnit);
                    double mVacancyRate = (mParams.VacancyRate == 0 ? 0.05 : mParams.VacancyRate);
                    double mOperatingExpensesPerFoot = (mParams.OperatingExpensesPerFt == 0 ? 10 : mParams.OperatingExpensesPerFt);
                    double mYearlyApartmentTaxes = (mParams.YearlyApartmentTaxes == 0 ? 50000 : mParams.YearlyApartmentTaxes);

                    //-FAR(Get from Database) 85
                    CalcParameterOutputModel mSingleCalculation85 = new CalcParameterOutputModel();
                    mSingleCalculation85.parameterid = "85";
                    mSingleCalculation85.parametername = "Floor Area Ratio";
                    double TempFloorToAreaRatio = Convert.ToDouble(mFAR);
                    double FloorToAreaRatio = (mParams.FloorAreaRatio != TempFloorToAreaRatio && mParams.FloorAreaRatio != 0 ? mParams.FloorAreaRatio : TempFloorToAreaRatio);
                    mSingleCalculation85.value = FloorToAreaRatio.ToString();
                    mCalcList.Add(mSingleCalculation85);

                    //- Property’s MAX FAR(FAR X Lot Size) 86
                    CalcParameterOutputModel mSingleCalculation86 = new CalcParameterOutputModel();
                    mSingleCalculation86.parameterid = "86";
                    mSingleCalculation86.parametername = "FAR Max";
                    double TempFARMax = Convert.ToDouble(FarMax);
                    double FARMax = (mParams.FarMax != TempFARMax && mParams.FarMax != 0 ? mParams.FarMax : TempFARMax);
                    mSingleCalculation86.value = FARMax.ToString();
                    mCalcList.Add(mSingleCalculation86);

                    //- Number of single-family homes(Lot Size / Minimum Lot size) rounded down to the nearest whole number 30
                    CalcParameterOutputModel mSingleCalculation30 = new CalcParameterOutputModel();
                    mSingleCalculation30.parameterid = "30";
                    mSingleCalculation30.parametername = "Number of Homes";
                    double TempNumberOfHomes = Math.Floor(mParams.LotSize / Convert.ToDouble(mDig));
                    double NumberOfHomes = (mParams.Number_of_homes != TempNumberOfHomes && mParams.Number_of_homes != 0 ? mParams.Number_of_homes : TempNumberOfHomes);
                    mSingleCalculation30.value = NumberOfHomes.ToString();
                    mCalcList.Add(mSingleCalculation30);

                    //- ECA cost(Pulled form Database)
                    CalcParameterOutputModel mSingleCalculation87 = new CalcParameterOutputModel();
                    mSingleCalculation87.parameterid = "87";
                    mSingleCalculation87.parametername = "ECA Cost";
                    double TempEcaCost = 0;
                    double EcaCost = (mParams.EcaCost != TempEcaCost && mParams.EcaCost != 0 ? mParams.EcaCost : TempEcaCost);
                    mSingleCalculation87.value = EcaCost.ToString();
                    mCalcList.Add(mSingleCalculation87);

                    // Elevation Grade Cost (Lot Size X Elevation Change /2 X Cost per sqft of dirt removed/Number of Homes) * Cost Per Grade
                    CalcParameterOutputModel mSingleCalculation88 = new CalcParameterOutputModel();
                    mSingleCalculation88.parameterid = "88";
                    mSingleCalculation88.parametername = "Elevation Grade Cost";
                    double TempElevationGradeCost = mParams.LotSize * mParams.ElevationChange / 2 * CostPerGrade / NumberOfHomes;
                    double ElevationGradeCost = (mParams.ElevationGradeCost != TempElevationGradeCost && mParams.ElevationGradeCost != 0 ? mParams.ElevationGradeCost : TempElevationGradeCost);
                    mSingleCalculation88.value = ElevationGradeCost.ToString();
                    mCalcList.Add(mSingleCalculation88);

                    //- Water Line Cost (Water Main Extension Length X Price of Watermain Extension per foot/Number of Homes)
                    CalcParameterOutputModel mSingleCalculation89 = new CalcParameterOutputModel();
                    mSingleCalculation89.parameterid = "89";
                    mSingleCalculation89.parametername = "Water Line Cost";
                    double TempWaterLineCost = mParams.WaterMainLength * WaterLineCostPerSqft / NumberOfHomes;
                    double WaterLineCost = (mParams.WaterLineCost != TempWaterLineCost && mParams.WaterLineCost != 0 ? mParams.WaterLineCost : TempWaterLineCost);
                    mSingleCalculation89.value = WaterLineCost.ToString();
                    mCalcList.Add(mSingleCalculation89);

                    //- Sewer Cost (Sewer Main Extension Length X Price of Sewer Cost per Sqft / Number of Homes)
                    CalcParameterOutputModel mSingleCalculation91 = new CalcParameterOutputModel();
                    mSingleCalculation91.parameterid = "91";
                    mSingleCalculation91.parametername = "Sewer Cost";
                    double TempSewerCost = mParams.SewerMainLength * SewerCostPerSqft / NumberOfHomes;
                    double SewerCost = (mParams.SewerCost != TempSewerCost && mParams.SewerCost != 0 ? mParams.SewerCost : TempSewerCost);
                    mSingleCalculation91.value = SewerCost.ToString();
                    mCalcList.Add(mSingleCalculation91);

                    //- Storm Drain Cost (Storm Main Extension Length X Price of Storm Drain Cost per sqft /Number of Homes)
                    CalcParameterOutputModel mSingleCalculation105 = new CalcParameterOutputModel();
                    mSingleCalculation105.parameterid = "105";
                    mSingleCalculation105.parametername = "Storm Drain Cost";
                    double TempStormDrainCost = mParams.DrainageDistance * StormDrainCostPerSqft / NumberOfHomes;
                    double StormDrainCost = (mParams.StormDrainCost != TempStormDrainCost && mParams.StormDrainCost != 0 ? mParams.StormDrainCost : TempStormDrainCost);
                    mSingleCalculation105.value = StormDrainCost.ToString();
                    mCalcList.Add(mSingleCalculation105);

                    //- Alley Cost (Alley Reconstruction Length X Alley Cost per foot / Number of Homes)
                    CalcParameterOutputModel mSingleCalculation90 = new CalcParameterOutputModel();
                    mSingleCalculation90.parameterid = "90";
                    mSingleCalculation90.parametername = "Alley Cost";
                    double TempAlleyCost = mParams.AlleyConstructionLength * AlleyCostPerSqft / NumberOfHomes;
                    double AlleyCost = (mParams.AlleyCost != TempAlleyCost && mParams.AlleyCost != 0 ? mParams.AlleyCost : TempAlleyCost);
                    mSingleCalculation90.value = AlleyCost.ToString();
                    mCalcList.Add(mSingleCalculation90);

                    //- Underground Parking Cost (Number of Underground Spots X Underground Parking Cost / Number of Homes)
                    CalcParameterOutputModel mSingleCalculation106 = new CalcParameterOutputModel();
                    mSingleCalculation106.parameterid = "106";
                    mSingleCalculation106.parametername = "Underground Parking Cost";
                    double TempUndergroundParkingCost = UndergroundParkingCostPerStall * mParams.NumberOfSpots / NumberOfHomes;
                    double UndergroundParkingCost = (mParams.UndergroundParkingBaseCost != TempUndergroundParkingCost && mParams.UndergroundParkingBaseCost != 0 ? mParams.UndergroundParkingBaseCost : TempUndergroundParkingCost);
                    mSingleCalculation106.value = UndergroundParkingCost.ToString();
                    mCalcList.Add(mSingleCalculation106);

                    if (mParams.StructureType == "Single Family")
                    {
                        // Number of Dadu’s(Equaled to the number of single - family homes) rounded down to the nearest whole 32
                        CalcParameterOutputModel mSingleCalculation34 = new CalcParameterOutputModel();
                        mSingleCalculation34.parameterid = "34";
                        mSingleCalculation34.parametername = "Dadu";
                        double TempDADUNumber = Math.Floor(mParams.LotSize / Convert.ToDouble(mDig));
                        double DADUNumber = (mParams.Dadu != TempDADUNumber && mParams.Dadu != 0 ? mParams.Dadu : TempDADUNumber);
                        mSingleCalculation34.value = DADUNumber.ToString();
                        mCalcList.Add(mSingleCalculation34);

                        // Number of AADU’s(Equaled to the number of single - family homes) rounded down to the nearest whole 34
                        CalcParameterOutputModel mSingleCalculation32 = new CalcParameterOutputModel();
                        mSingleCalculation32.parameterid = "32";
                        mSingleCalculation32.parametername = "Aadu";
                        double TempAADUNumber = Math.Floor(mParams.LotSize / Convert.ToDouble(mDig));
                        double AADUNumber = (mParams.Aadu != TempAADUNumber && mParams.Aadu != 0 ? mParams.Aadu : TempAADUNumber);
                        mSingleCalculation32.value = AADUNumber.ToString();
                        mCalcList.Add(mSingleCalculation32);

                        // Sellable Sqft.Factor(1.07) Default 96
                        CalcParameterOutputModel mSingleCalculation96 = new CalcParameterOutputModel();
                        mSingleCalculation96.parameterid = "96";
                        mSingleCalculation96.parametername = "Sellable Sqft Factor";
                        double TempSellableSqftFactor = mSellableSqftFactor;
                        double SellableSqftFactor = (mParams.SellableSqftFactor != TempSellableSqftFactor && mParams.SellableSqftFactor != 0 ? mParams.SellableSqftFactor : TempSellableSqftFactor);
                        mSingleCalculation96.value = SellableSqftFactor.ToString();
                        mCalcList.Add(mSingleCalculation96);

                        //- Average Single Family Sellable sqft. (MAX FAR X Sellable Sqft. Factor / Number of single-family homes) 67
                        CalcParameterOutputModel mSingleCalculation67 = new CalcParameterOutputModel();
                        mSingleCalculation67.parameterid = "67";
                        mSingleCalculation67.parametername = "Sellable Square Footage";
                        double TempSellableSquareFootage = Convert.ToDouble(FarMax) / NumberOfHomes * SellableSqftFactor;
                        double SellableSquareFootage = (mParams.SellableSqFootage != TempSellableSquareFootage && mParams.SellableSqFootage != 0 ? mParams.SellableSqFootage : TempSellableSquareFootage);
                        mSingleCalculation67.value = SellableSquareFootage.ToString();
                        mCalcList.Add(mSingleCalculation67);

                        //-Average DADU Sellable sqft. (Default to 1, 100sqft)
                        CalcParameterOutputModel mSingleCalculation35 = new CalcParameterOutputModel();
                        mSingleCalculation35.parameterid = "35";
                        mSingleCalculation35.parametername = "DADU Size";
                        double DaduSize = 1000;
                        mSingleCalculation67.value = DaduSize.ToString();
                        mCalcList.Add(mSingleCalculation35);

                        //- Average AADU Sellable sqft. (Default to 1, 100sqft)
                        CalcParameterOutputModel mSingleCalculation33 = new CalcParameterOutputModel();
                        mSingleCalculation33.parameterid = "33";
                        mSingleCalculation33.parametername = "AADU Size";
                        double AaduSize = 1000;
                        mSingleCalculation33.value = AaduSize.ToString();
                        mCalcList.Add(mSingleCalculation33);

                        //2nd Step(Find out the construction Costs)
                        //-Single Family Base Cost Per Sqft. (Default to $240 per foot)
                        CalcParameterOutputModel mSingleCalculation109 = new CalcParameterOutputModel();
                        mSingleCalculation109.parameterid = "109";
                        mSingleCalculation109.parametername = "SF Base Cost Per Sqft";
                        double TempSfBaseCostPerSqft = mSfBaseCostPerSqft;
                        double SfBaseCostPerSqft = (mParams.SfBaseCostPerSqft != TempSfBaseCostPerSqft && mParams.SfBaseCostPerSqft != 0 ? mParams.SfBaseCostPerSqft : TempSfBaseCostPerSqft);
                        mSingleCalculation109.value = SfBaseCostPerSqft.ToString();
                        mCalcList.Add(mSingleCalculation109);

                        //- ADU’s cost per sqft. (Default to $250)
                        CalcParameterOutputModel mSingleCalculation115 = new CalcParameterOutputModel();
                        mSingleCalculation115.parameterid = "115";
                        mSingleCalculation115.parametername = "ADU Base Cost Per Sqft";
                        double AduCostPerSqft = 250;
                        mSingleCalculation115.value = AduCostPerSqft.ToString();
                        mCalcList.Add(mSingleCalculation115);

                        //SF Base Cost (SF Base Cost Per Sqft. X Sellable square Footage.)
                        CalcParameterOutputModel mSingleCalculation97 = new CalcParameterOutputModel();
                        mSingleCalculation97.parameterid = "97";
                        mSingleCalculation97.parametername = "SF Base Cost";
                        double TempSfBaseCost = SfBaseCostPerSqft * SellableSquareFootage;
                        double SfBaseCost = (mParams.SfBaseCost != TempSfBaseCost && mParams.SfBaseCost != 0 ? mParams.SfBaseCost : TempSfBaseCost);
                        mSingleCalculation97.value = SfBaseCost.ToString();
                        mCalcList.Add(mSingleCalculation97);

                        //-AADU Base Cost (ADU’s cost per sqft. X AADU Size.)
                        CalcParameterOutputModel mSingleCalculation104 = new CalcParameterOutputModel();
                        mSingleCalculation104.parameterid = "104";
                        mSingleCalculation104.parametername = "AADU Base Cost";
                        double TempAaduBaseCost = AduCostPerSqft * AaduSize;
                        double AaduBaseCost = (mParams.AADUBaseCost != TempAaduBaseCost && mParams.AADUBaseCost != 0 ? mParams.AADUBaseCost : TempAaduBaseCost);
                        mSingleCalculation104.value = AaduBaseCost.ToString();
                        mCalcList.Add(mSingleCalculation104);

                        //-DADU Base Cost (ADU’s cost per sqft. X DADU Size.)
                        CalcParameterOutputModel mSingleCalculation124 = new CalcParameterOutputModel();
                        mSingleCalculation124.parameterid = "124";
                        mSingleCalculation124.parametername = "DADU Base Cost";
                        double TempDaduBaseCost = AduCostPerSqft * DaduSize;
                        double DaduBaseCost = (mParams.DaduBaseCost != TempDaduBaseCost && mParams.DaduBaseCost != 0 ? mParams.DaduBaseCost : TempDaduBaseCost);
                        mSingleCalculation124.value = DaduBaseCost.ToString();
                        mCalcList.Add(mSingleCalculation124);

                        //Total Building Cost Per Lot(Sum of ALL RED TEXT)
                        CalcParameterOutputModel mSingleCalculation107 = new CalcParameterOutputModel();
                        mSingleCalculation107.parameterid = "107";
                        mSingleCalculation107.parametername = "Total Building Cost Per Lot";
                        double TempTotalBuildingCostPerLot = SfBaseCost + AaduBaseCost + DaduBaseCost + EcaCost + ElevationGradeCost + WaterLineCost + SewerCost + StormDrainCost + AlleyCost + UndergroundParkingCost;
                        double TotalBuildingCostPerLot = (mParams.TotalBuildingCostPerLot != TempTotalBuildingCostPerLot && mParams.TotalBuildingCostPerLot != 0 ? mParams.TotalBuildingCostPerLot : TempTotalBuildingCostPerLot);
                        mSingleCalculation107.value = TotalBuildingCostPerLot.ToString();
                        mCalcList.Add(mSingleCalculation107);

                        //Total Project Cost(Total Building Cost Per Lot X Number of Single Family homes)
                        CalcParameterOutputModel mSingleCalculation108 = new CalcParameterOutputModel();
                        mSingleCalculation108.parameterid = "108";
                        mSingleCalculation108.parametername = "Total Project Cost";
                        double TempTotalProjectCost = TotalBuildingCostPerLot * NumberOfHomes;
                        double TotalProjectCost = (mParams.TotalProjectCost != TempTotalProjectCost && mParams.TotalProjectCost != 0 ? mParams.DaduBaseCost : TempTotalProjectCost);
                        mSingleCalculation108.value = TotalProjectCost.ToString();
                        mCalcList.Add(mSingleCalculation108);

                        //3rd Step(Sales Cost Breakdown)
                        //SF Estimated Value (Manually Imported)
                        CalcParameterOutputModel mSingleCalculation37 = new CalcParameterOutputModel();
                        mSingleCalculation37.parameterid = "37";
                        mSingleCalculation37.parametername = "SF Estimated Value";
                        double TempSFEstimatedValue = mSfEstimatedValue;
                        double SFEstimatedValue = (mParams.SfEstimatedValue != TempDaduBaseCost && mParams.SfEstimatedValue != 0 ? mParams.SfEstimatedValue : TempSFEstimatedValue);
                        mSingleCalculation37.value = SFEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation37);

                        // AADU Estimated Value (Manually Imported)
                        CalcParameterOutputModel mSingleCalculation38 = new CalcParameterOutputModel();
                        mSingleCalculation38.parameterid = "38";
                        mSingleCalculation38.parametername = "ADU Estimated Value";
                        double TempADUEstimatedValue = mParams.Aadu * mParams.Aadu_size * 250;
                        double ADUEstimatedValue = (mParams.AADUEstimatedValue != TempADUEstimatedValue && mParams.AADUEstimatedValue != 0 ? mParams.AADUEstimatedValue : TempADUEstimatedValue);
                        mSingleCalculation38.value = ADUEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation38);

                        // DADU Estimated Value (Manually Imported)
                        CalcParameterOutputModel mSingleCalculation39 = new CalcParameterOutputModel();
                        mSingleCalculation39.parameterid = "39";
                        mSingleCalculation39.parametername = "DADU Estimated Value";
                        double TempDADUEstimatedValue = mParams.Dadu * mParams.Dadu_size * 250;
                        double DADUEstimatedValue = (mParams.DaduEstimatedValue != TempDADUEstimatedValue && mParams.DaduEstimatedValue != 0 ? mParams.DaduEstimatedValue : TempDADUEstimatedValue);
                        mSingleCalculation39.value = DADUEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation39);

                        // Lot Estimated Value (Sum of Green above)
                        CalcParameterOutputModel mSingleCalculation145 = new CalcParameterOutputModel();
                        mSingleCalculation145.parameterid = "145";
                        mSingleCalculation145.parametername = "Lot Estimated Value";
                        double TempLotEstimatedValue = SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue;
                        double LotEstimatedValue = (mParams.LotEstimatedValue != TempLotEstimatedValue && mParams.LotEstimatedValue != 0 ? mParams.LotEstimatedValue : TempLotEstimatedValue);
                        mSingleCalculation145.value = LotEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation145);

                        // Sales Commission (Sales Commission Table X Lot Estimated Value)
                        // TODO: load sales commission table
                        CalcParameterOutputModel mSingleCalculation41 = new CalcParameterOutputModel();
                        mSingleCalculation41.parameterid = "41";
                        mSingleCalculation41.parametername = "Sales Commission";
                        double TempSalesCommission = 0.06 * LotEstimatedValue;
                        double SalesCommission = (mParams.SalesCommission != TempSalesCommission && mParams.SalesCommission != 0 ? mParams.SalesCommission : TempSalesCommission);
                        mSingleCalculation41.value = SalesCommission.ToString();
                        mCalcList.Add(mSingleCalculation41);

                        // Excise Tax (Excise Tax Table X Lot Estimated Value)
                        CalcParameterOutputModel mSingleCalculation42 = new CalcParameterOutputModel();
                        mSingleCalculation42.parameterid = "42";
                        mSingleCalculation42.parametername = "Excise Tax";
                        double TempExciseTax = ExciseOutput(LotEstimatedValue);
                        double ExciseTax = (mParams.ExciseTax != TempExciseTax && mParams.ExciseTax != 0 ? mParams.ExciseTax : TempExciseTax);
                        mSingleCalculation42.value = ExciseTax.ToString();
                        mCalcList.Add(mSingleCalculation42);

                        // Escrow Fees / Insurance (Escrow and Title Fee table X Lot Estimated Value)
                        CalcParameterOutputModel mSingleCalculation43 = new CalcParameterOutputModel();
                        mSingleCalculation43.parameterid = "43";
                        mSingleCalculation43.parametername = "Escrow Fees/Insurance";
                        double TempEscrowFees = (SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue) * mFactor5;
                        double EscrowFees = (mParams.EscrowFees != TempEscrowFees && mParams.EscrowFees != 0 ? mParams.EscrowFees : TempEscrowFees);
                        mSingleCalculation43.value = EscrowFees.ToString();
                        mCalcList.Add(mSingleCalculation43);

                        // Marketing / Staging (Marketing and Staging table X Lot Estimated Value)
                        CalcParameterOutputModel mSingleCalculation44 = new CalcParameterOutputModel();
                        mSingleCalculation44.parameterid = "44";
                        mSingleCalculation44.parametername = "Marketing/Cleaners";
                        double TempMarketing = (SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue) * mFactor5;
                        double Marketing = (mParams.MarketingAndStaging != TempMarketing && mParams.MarketingAndStaging != 0 ? mParams.MarketingAndStaging : TempMarketing);
                        mSingleCalculation44.value = Marketing.ToString();
                        mCalcList.Add(mSingleCalculation44);

                        //- Total Sales Cost per Single Per Lot(Sum of all blue)
                        CalcParameterOutputModel mSingleCalculation134 = new CalcParameterOutputModel();
                        mSingleCalculation134.parameterid = "134";
                        mSingleCalculation134.parametername = "Total Sales Cost Per Lot";
                        double TempTotalSalesCostPerLot = SalesCommission + ExciseTax + EscrowFees + Marketing;
                        double TotalSalesCostPerLot = (mParams.TotalSalesCostPerLot != TempTotalSalesCostPerLot && mParams.TotalSalesCostPerLot != 0 ? mParams.TotalSalesCostPerLot : TempTotalSalesCostPerLot);
                        mSingleCalculation104.value = TotalSalesCostPerLot.ToString();
                        mCalcList.Add(mSingleCalculation134);

                        //-Total Sales Cost per Project(Total Sales Cost per Single Per Lot X Number of Single-Family Homes)
                        CalcParameterOutputModel mSingleCalculation140 = new CalcParameterOutputModel();
                        mSingleCalculation140.parameterid = "140";
                        mSingleCalculation140.parametername = "Total Sales Cost Per Project";
                        double TempTotalSalesCostPerProject = TotalSalesCostPerLot * NumberOfHomes;
                        double TotalSalesCostPerProject = (mParams.TotalSalesCostPerProject != TempTotalSalesCostPerProject && mParams.TotalSalesCostPerProject != 0 ? mParams.TotalSalesCostPerProject : TempTotalSalesCostPerProject);
                        mSingleCalculation140.value = TotalSalesCostPerProject.ToString();
                        mCalcList.Add(mSingleCalculation140);

                        //4th Step(Land Analysis)
                        // -Lot Estimated Value
                        CalcParameterOutputModel mSingleCalculation147 = new CalcParameterOutputModel();
                        mSingleCalculation147.parameterid = "147";
                        mSingleCalculation147.parametername = "Lot Estimated Value";
                        double TempLotEstimatedValueLA = LotEstimatedValue;
                        double LotEstimatedValueLA = (mParams.LotEstimatedValue != TempLotEstimatedValueLA && mParams.LotEstimatedValue != 0 ? mParams.LotEstimatedValue : TempLotEstimatedValueLA);
                        mSingleCalculation147.value = LotEstimatedValueLA.ToString();
                        mCalcList.Add(mSingleCalculation147);

                        // -Desired Profit Percent
                        CalcParameterOutputModel mSingleCalculation181 = new CalcParameterOutputModel();
                        mSingleCalculation181.parameterid = "181";
                        mSingleCalculation181.parametername = "Desired Profit Percent";
                        double TempDesiredProfitPercentLA = mDesiredProfitPercent;
                        double DesiredProfitPercentLA = (mParams.DesiredProfitPercent != TempDesiredProfitPercentLA && mParams.DesiredProfitPercent != 0 ? mParams.DesiredProfitPercent : TempDesiredProfitPercentLA);
                        mSingleCalculation181.value = DesiredProfitPercentLA.ToString();
                        mCalcList.Add(mSingleCalculation181);

                        //-Total Building Cost Per Lot
                        CalcParameterOutputModel mSingleCalculation170 = new CalcParameterOutputModel();
                        mSingleCalculation170.parameterid = "170";
                        mSingleCalculation170.parametername = "Total Building Cost Per Lot";
                        double TempTotalBuildingCostPerLotLA = TotalBuildingCostPerLot;
                        double TotalBuildingCostPerLotLA = (mParams.TotalBuildingCostPerLot != TempTotalBuildingCostPerLotLA && mParams.TotalBuildingCostPerLot != 0 ? mParams.TotalBuildingCostPerLot : TempTotalBuildingCostPerLotLA);
                        mSingleCalculation170.value = TotalBuildingCostPerLotLA.ToString();
                        mCalcList.Add(mSingleCalculation170);

                        // -Total Sales Cost Per Lot
                        CalcParameterOutputModel mSingleCalculation172 = new CalcParameterOutputModel();
                        mSingleCalculation172.parameterid = "172";
                        mSingleCalculation172.parametername = "Total Sales Cost Per Lot";
                        double TempTotalSalesCostPerLotLA = TotalSalesCostPerLot;
                        double TotalSalesCostPerLotLA = (mParams.TotalSalesCostPerLot != TempTotalSalesCostPerLotLA && mParams.TotalSalesCostPerLot != 0 ? mParams.TotalSalesCostPerLot : TempTotalSalesCostPerLotLA);
                        mSingleCalculation172.value = TotalSalesCostPerLotLA.ToString();
                        mCalcList.Add(mSingleCalculation172);

                        // -Desired Profit before financing (Desired profit % X Backend Sales Price)
                        CalcParameterOutputModel mSingleCalculation45 = new CalcParameterOutputModel();
                        mSingleCalculation45.parameterid = "45";
                        mSingleCalculation45.parametername = "Desired Profit Before Financing";
                        double TempDesiredProfitBeforeFinancingLA = DesiredProfitPercentLA * LotEstimatedValueLA;
                        double DesiredProfitBeforeFinancingLA = (mParams.DesiredProfitBeforeFinancing != TempDesiredProfitBeforeFinancingLA && mParams.DesiredProfitBeforeFinancing != 0 ? mParams.DesiredProfitBeforeFinancing : TempDesiredProfitBeforeFinancingLA);
                        mSingleCalculation45.value = DesiredProfitBeforeFinancingLA.ToString();
                        mCalcList.Add(mSingleCalculation45);

                        // -Value of Land Per Lot (Sum of Step 4 above this line (Red is Negative Green is Positive Number)
                        CalcParameterOutputModel mSingleCalculation148 = new CalcParameterOutputModel();
                        mSingleCalculation148.parameterid = "148";
                        mSingleCalculation148.parametername = "Value of Land Per Lot";
                        double TempValueofLandPerLotLA = LotEstimatedValue - (TotalBuildingCostPerLot + TotalSalesCostPerLot + DesiredProfitBeforeFinancingLA);
                        double ValueofLandPerLotLA = (mParams.ValueOfLandPerLot != TempValueofLandPerLotLA && mParams.ValueOfLandPerLot != 0 ? mParams.ValueOfLandPerLot : TempValueofLandPerLotLA);
                        mSingleCalculation148.value = ValueofLandPerLotLA.ToString();
                        mCalcList.Add(mSingleCalculation148);

                        //-Total Land Value (Value of Land Per Lot X Number of Single Family Homes)
                        CalcParameterOutputModel mSingleCalculation47 = new CalcParameterOutputModel();
                        mSingleCalculation47.parameterid = "47";
                        mSingleCalculation47.parametername = "Total Land Value";
                        double TempTotalLandValue = ValueofLandPerLotLA * NumberOfHomes;
                        double TotalLandValue = (mParams.TotalLandValue != TempTotalLandValue && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValue);
                        mSingleCalculation47.value = TotalLandValue.ToString();
                        mCalcList.Add(mSingleCalculation47);

                        // Step 5 Proforma & Financing

                        // Total Project Cost* Step 2  
                        CalcParameterOutputModel mSingleCalculation178 = new CalcParameterOutputModel();
                        mSingleCalculation178.parameterid = "178";
                        mSingleCalculation178.parametername = "Total Project Cost";
                        double TempTotalProjectCostFI = TotalProjectCost;
                        double TotalProjectCostFI = (mParams.TotalProjectCost != TempTotalProjectCostFI && mParams.TotalProjectCost != 0 ? mParams.TotalProjectCost : TempTotalProjectCostFI);
                        mSingleCalculation178.value = TotalProjectCost.ToString();
                        mCalcList.Add(mSingleCalculation178);

                        // Total Land Value* Step 4
                        CalcParameterOutputModel mSingleCalculation179 = new CalcParameterOutputModel();
                        mSingleCalculation179.parameterid = "179";
                        mSingleCalculation179.parametername = "Total Land Value";
                        double TempTotalLandValueFI = TotalLandValue;
                        double TotalLandValueFI = (mParams.TotalLandValue != TempTotalLandValueFI && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValueFI);
                        mSingleCalculation179.value = TotalLandValueFI.ToString();
                        mCalcList.Add(mSingleCalculation179);

                        // Equity(Manual Input)
                        CalcParameterOutputModel mSingleCalculation55 = new CalcParameterOutputModel();
                        mSingleCalculation55.parameterid = "55";
                        mSingleCalculation55.parametername = "Equity";
                        double TempEquityFI = mEquity;
                        double EquityFI = (mParams.Equity != TempEquityFI && mParams.Equity != 0 ? mParams.Equity : TempEquityFI);
                        mSingleCalculation55.value = EquityFI.ToString();
                        mCalcList.Add(mSingleCalculation55);

                        // Acquisition Carrying Months(Manual Input)
                        CalcParameterOutputModel mSingleCalculation56 = new CalcParameterOutputModel();
                        mSingleCalculation56.parameterid = "56";
                        mSingleCalculation56.parametername = "Acquisition Carrying Months";
                        double TempAcquisitionCarryingMonthsFI = mAcquisitionCarryingMonths;
                        double AcquisitionCarryingMonthsFI = (mParams.AcquisitionCarryingMonths != TempAcquisitionCarryingMonthsFI && mParams.AcquisitionCarryingMonths != 0 ? mParams.AcquisitionCarryingMonths : TempAcquisitionCarryingMonthsFI);
                        mSingleCalculation56.value = AcquisitionCarryingMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation56);

                        // Interest Rate Land Acquisition(Manual Input)
                        CalcParameterOutputModel mSingleCalculation57 = new CalcParameterOutputModel();
                        mSingleCalculation57.parameterid = "57";
                        mSingleCalculation57.parametername = "Interest Rate Land Acquisition";
                        double TempInterestRateLandAcquisitionFI = mInterestRateLandAcquisition;
                        double InterestRateLandAcquisitionFI = (mParams.InterestRateLandAcquisition != TempInterestRateLandAcquisitionFI && mParams.InterestRateLandAcquisition != 0 ? mParams.InterestRateLandAcquisition : TempInterestRateLandAcquisitionFI);
                        mSingleCalculation57.value = InterestRateLandAcquisitionFI.ToString();
                        mCalcList.Add(mSingleCalculation57);

                        // Loan Fees Land Acquisition Loan(Manual Input)
                        CalcParameterOutputModel mSingleCalculation81 = new CalcParameterOutputModel();
                        mSingleCalculation81.parameterid = "81";
                        mSingleCalculation81.parametername = "Loan Fees Land Acquisition Loan";
                        double TempLoanFeesLandAcquisitionLoanFI = mLoanFeesLandAcquisitionLoan;
                        double LoanFeesLandAcquisitionLoanFI = (mParams.LoanFeesLandAcquisitionLoan != TempLoanFeesLandAcquisitionLoanFI && mParams.LoanFeesLandAcquisitionLoan != 0 ? mParams.LoanFeesLandAcquisitionLoan : TempLoanFeesLandAcquisitionLoanFI);
                        mSingleCalculation81.value = LoanFeesLandAcquisitionLoanFI.ToString();
                        mCalcList.Add(mSingleCalculation81);

                        // Total Financing Cost Land Acquisition((Land Value – Equity) * (Interest Rate Land Acquisition * (Acquisition Carrying Months/ 12) +(Land Value – Equity)*(Loan Fees Land Acquisition Loan)
                        CalcParameterOutputModel mSingleCalculation58 = new CalcParameterOutputModel();
                        mSingleCalculation58.parameterid = "58";
                        mSingleCalculation58.parametername = "Total Financing Cost Land Acquisition";
                        double TempTotalFinancingCostLandAcquisitionFI = (TotalLandValueFI - EquityFI) * (InterestRateLandAcquisitionFI * (AcquisitionCarryingMonthsFI / 12) + (TotalLandValueFI - EquityFI) * (LoanFeesLandAcquisitionLoanFI));
                        double TotalFinancingCostLandAcquisitionFI = (mParams.TotalFinancingCostLandAcquisition != TempTotalFinancingCostLandAcquisitionFI && mParams.TotalFinancingCostLandAcquisition != 0 ? mParams.TotalFinancingCostLandAcquisition : TempTotalFinancingCostLandAcquisitionFI);
                        mSingleCalculation58.value = TotalFinancingCostLandAcquisitionFI.ToString();
                        mCalcList.Add(mSingleCalculation58);

                        // Interest Rate Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation59 = new CalcParameterOutputModel();
                        mSingleCalculation59.parameterid = "59";
                        mSingleCalculation59.parametername = "Interest Rate Construction Loan";
                        double TempInterestRateConstructionLoan = mInterestRateConstructionLoan;
                        double InterestRateConstructionLoan = (mParams.InterestRateConstructionLoan != TempInterestRateConstructionLoan && mParams.InterestRateConstructionLoan != 0 ? mParams.InterestRateConstructionLoan : TempInterestRateConstructionLoan);
                        mSingleCalculation59.value = InterestRateConstructionLoan.ToString();
                        mCalcList.Add(mSingleCalculation59);

                        // Loan Fees Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation60 = new CalcParameterOutputModel();
                        mSingleCalculation60.parameterid = "60";
                        mSingleCalculation60.parametername = "Loan Fees Construction Loan";
                        double TempLoanFeesConstructionLoanPI = mLoanFeesConstructionLoan;
                        double LoanFeesConstructionLoanPI = (mParams.LoanFeesConstructionLoan != TempLoanFeesConstructionLoanPI && mParams.LoanFeesConstructionLoan != 0 ? mParams.LoanFeesConstructionLoan : TempLoanFeesConstructionLoanPI);
                        mSingleCalculation60.value = LoanFeesConstructionLoanPI.ToString();
                        mCalcList.Add(mSingleCalculation60);

                        // Construction Loan Months(Manual Default)
                        CalcParameterOutputModel mSingleCalculation61 = new CalcParameterOutputModel();
                        mSingleCalculation61.parameterid = "61";
                        mSingleCalculation61.parametername = "Construction Loan Months";
                        double TempConstructionLoanMonthsFI = mConstructionLoanMonths;
                        double ConstructionLoanMonthsFI = (mParams.ConstructionLoanMonths != TempConstructionLoanMonthsFI && mParams.ConstructionLoanMonths != 0 ? mParams.ConstructionLoanMonths : TempConstructionLoanMonthsFI);
                        mSingleCalculation61.value = ConstructionLoanMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation61);

                        // Total Financing Construction Cost(((Land Value -Equity) +(Total Project Cost/ 2) *((Interest Rate Construction Loan”) *(Construction Loan Months/ 12)) +((Land Value – Equity) *(Loan Fees Construction Loan”)))
                        CalcParameterOutputModel mSingleCalculation62 = new CalcParameterOutputModel();
                        mSingleCalculation62.parameterid = "62";
                        mSingleCalculation62.parametername = "Total Financing Cost Construction";
                        double TempTotalFinancingCostConstructionFI = ((TotalLandValueFI - EquityFI) + (TotalProjectCostFI / 2) * ((InterestRateConstructionLoan) * (ConstructionLoanMonthsFI / 12)) + ((TotalLandValueFI - EquityFI) * (LoanFeesConstructionLoanPI)));
                        double TotalFinancingCostConstructionFI = (mParams.TotalFinancingCostConstruction != TempTotalFinancingCostConstructionFI && mParams.TotalFinancingCostConstruction != 0 ? mParams.TotalFinancingCostConstruction : TempTotalFinancingCostConstructionFI);
                        mSingleCalculation62.value = TotalFinancingCostConstructionFI.ToString();
                        mCalcList.Add(mSingleCalculation62);

                        // Months to Sell after completion(Manual Default)
                        CalcParameterOutputModel mSingleCalculation64 = new CalcParameterOutputModel();
                        mSingleCalculation64.parameterid = "64";
                        mSingleCalculation64.parametername = "Months to Sell After completion";
                        double TempMonthstoSellAfterCompletionFI = mMonthsToSellAfterCompletion;
                        double MonthstoSellAfterCompletionFI = (mParams.MonthsToSellAfterCompletion != TempMonthstoSellAfterCompletionFI && mParams.MonthsToSellAfterCompletion != 0 ? mParams.MonthsToSellAfterCompletion : TempMonthstoSellAfterCompletionFI);
                        mSingleCalculation64.value = MonthstoSellAfterCompletionFI.ToString();
                        mCalcList.Add(mSingleCalculation64);

                        // Extra Financing After Completion(Land Value +Total Project Cost – Contributed Equity) *(Interest Rate Construction Loan * (Months to Sell after completion Months / 12)  
                        CalcParameterOutputModel mSingleCalculation65 = new CalcParameterOutputModel();
                        mSingleCalculation65.parameterid = "65";
                        mSingleCalculation65.parametername = "Extra Financing After Completion";
                        double TempExtraFinancingAfterCompletionFI = (TotalLandValueFI + TotalProjectCostFI - EquityFI) * (InterestRateConstructionLoan * (MonthstoSellAfterCompletionFI / 12));
                        double ExtraFinancingAfterCompletionFI = (mParams.ExtraFinancingAfterCompletion != TempExtraFinancingAfterCompletionFI && mParams.ExtraFinancingAfterCompletion != 0 ? mParams.ExtraFinancingAfterCompletion : TempExtraFinancingAfterCompletionFI);
                        mSingleCalculation65.value = ExtraFinancingAfterCompletionFI.ToString();
                        mCalcList.Add(mSingleCalculation65);

                        // Total Financing Fees(Sum of Green Text)
                        CalcParameterOutputModel mSingleCalculation66 = new CalcParameterOutputModel();
                        mSingleCalculation66.parameterid = "66";
                        mSingleCalculation66.parametername = "Total Financing Fees";
                        double TempTotalFinancingFeesFI = TotalFinancingCostLandAcquisitionFI + TotalFinancingCostConstructionFI + ExtraFinancingAfterCompletionFI;
                        double TotalFinancingFeesFI = (mParams.TotalFinancingFees != TempTotalFinancingFeesFI && mParams.TotalFinancingFees != 0 ? mParams.TotalFinancingFees : TempTotalFinancingFeesFI);
                        mSingleCalculation66.value = TotalFinancingFeesFI.ToString();
                        mCalcList.Add(mSingleCalculation66);


                        // Step 6 Proformas and Financing

                        // -Lot Estimated Value 
                        CalcParameterOutputModel mSingleCalculation146 = new CalcParameterOutputModel();
                        mSingleCalculation146.parameterid = "146";
                        mSingleCalculation146.parametername = "Lot Estimated Value";
                        double TempLotEstimatedValuePB = LotEstimatedValue;
                        double LotEstimatedValuePB = (mParams.LotEstimatedValue != TempLotEstimatedValuePB && mParams.LotEstimatedValue != 0 ? mParams.LotEstimatedValue : TempLotEstimatedValuePB);
                        mSingleCalculation146.value = LotEstimatedValuePB.ToString();
                        mCalcList.Add(mSingleCalculation146);

                        // -Total Building Cost Per Lot
                        CalcParameterOutputModel mSingleCalculation161 = new CalcParameterOutputModel();
                        mSingleCalculation161.parameterid = "161";
                        mSingleCalculation161.parametername = "Total Building Cost Per Lot";
                        double TempTotalBuildingCostPerLotPB = TotalBuildingCostPerLot;
                        double TotalBuildingCostPerLotPB = (mParams.TotalBuildingCostPerLot != TempTotalBuildingCostPerLotPB && mParams.TotalBuildingCostPerLot != 0 ? mParams.TotalBuildingCostPerLot : TempTotalBuildingCostPerLotPB);
                        mSingleCalculation161.value = TotalBuildingCostPerLotPB.ToString();
                        mCalcList.Add(mSingleCalculation161);

                        // -Total Sales Cost Per Lot 
                        CalcParameterOutputModel mSingleCalculation135 = new CalcParameterOutputModel();
                        mSingleCalculation135.parameterid = "135";
                        mSingleCalculation135.parametername = "Total Sales Cost Per Lot";
                        double TempTotalSalesCostPerLotPB = LotEstimatedValue;
                        double TotalSalesCostPerLotPB = (mParams.TotalSalesCostPerLot != TempTotalSalesCostPerLotPB && mParams.TotalSalesCostPerLot != 0 ? mParams.TotalSalesCostPerLot : TempTotalSalesCostPerLotPB);
                        mSingleCalculation135.value = LotEstimatedValuePB.ToString();
                        mCalcList.Add(mSingleCalculation135);

                        // -Value of Land Per Lot 
                        CalcParameterOutputModel mSingleCalculation149 = new CalcParameterOutputModel();
                        mSingleCalculation149.parameterid = "149";
                        mSingleCalculation149.parametername = "Value of Land Per Lot";
                        double TempValueofLandPerLotPB = ValueofLandPerLotLA;
                        double ValueofLandPerLotPB = (mParams.ValueOfLandPerLot != TempValueofLandPerLotPB && mParams.ValueOfLandPerLot != 0 ? mParams.ValueOfLandPerLot : TempValueofLandPerLotPB);
                        mSingleCalculation149.value = LotEstimatedValuePB.ToString();
                        mCalcList.Add(mSingleCalculation149);

                        // -Profit per lot before Financing (Sum of Step 5 above this line (Red is Negative Green is Positive Number for per lot)
                        CalcParameterOutputModel mSingleCalculation153 = new CalcParameterOutputModel();
                        mSingleCalculation153.parameterid = "153";
                        mSingleCalculation153.parametername = "Profit Per Home before Financing ";
                        double TempProfitPerLotBeforeFinancing = LotEstimatedValue - (TotalBuildingCostPerLotPB + TotalSalesCostPerLotPB + ValueofLandPerLotPB);
                        double ProfitPerLotBeforeFinancing = (mParams.ProfitPerLotBeforeFinancing != TempProfitPerLotBeforeFinancing && mParams.ProfitPerLotBeforeFinancing != 0 ? mParams.ProfitPerLotBeforeFinancing : TempProfitPerLotBeforeFinancing);
                        mSingleCalculation153.value = ProfitPerLotBeforeFinancing.ToString();
                        mCalcList.Add(mSingleCalculation153);

                        // -Financing Costs(Total financing Cost / Number of Single Family Homes)
                        CalcParameterOutputModel mSingleCalculation162 = new CalcParameterOutputModel();
                        mSingleCalculation162.parameterid = "162";
                        mSingleCalculation162.parametername = "Total Financing Cost";
                        double TempTotalFinancingCostPF = TotalFinancingFeesFI / NumberOfHomes;
                        double TotalFinancingCostPF = (mParams.TotalFinancingFees != TempTotalFinancingCostPF && mParams.TotalFinancingFees != 0 ? mParams.TotalFinancingFees : TempTotalFinancingCostPF);
                        mSingleCalculation162.value = TotalFinancingCostPF.ToString();
                        mCalcList.Add(mSingleCalculation162);

                        // -Net Profit Per Lot (Profit per lot before Financing - Financing Costs)
                        CalcParameterOutputModel mSingleCalculation156 = new CalcParameterOutputModel();
                        mSingleCalculation156.parameterid = "156";
                        mSingleCalculation156.parametername = "Net Profit Per Lot";
                        double TempNetProfitPerLotPF = TotalFinancingFeesFI / NumberOfHomes;
                        double NetProfitPerLotPF = (mParams.NetProfitPerLot != TempNetProfitPerLotPF && mParams.NetProfitPerLot != 0 ? mParams.NetProfitPerLot : TempNetProfitPerLotPF);
                        mSingleCalculation156.value = NetProfitPerLotPF.ToString();
                        mCalcList.Add(mSingleCalculation156);

                        // Performa Per Project
                        // -Total Backend Sales Price(Backend Sales Price X Number of Single Family Homes)
                        CalcParameterOutputModel mSingleCalculation166 = new CalcParameterOutputModel();
                        mSingleCalculation166.parameterid = "166";
                        mSingleCalculation166.parametername = "Total Backend Sales Price";
                        double TempTotalBackendSalesPricePF = LotEstimatedValueLA / NumberOfHomes;
                        double TotalBackendSalesPricePF = (mParams.HomeEstimatedValue != TempTotalBackendSalesPricePF && mParams.HomeEstimatedValue != 0 ? mParams.HomeEstimatedValue : TempTotalBackendSalesPricePF);
                        mSingleCalculation166.value = TotalBackendSalesPricePF.ToString();
                        mCalcList.Add(mSingleCalculation166);

                        // - Total Project Cost *See step 2
                        CalcParameterOutputModel mSingleCalculation182 = new CalcParameterOutputModel();
                        mSingleCalculation182.parameterid = "182";
                        mSingleCalculation182.parametername = "Total Project Cost";
                        double TempTotalProjectCostPF = TotalProjectCost;
                        double TotalProjectCostPF = (mParams.TotalProjectCost != TempTotalProjectCostPF && mParams.TotalProjectCost != 0 ? mParams.TotalProjectCost : TempTotalProjectCostPF);
                        mSingleCalculation182.value = TotalBackendSalesPricePF.ToString();
                        mCalcList.Add(mSingleCalculation182);

                        // - Total Sales Cost Per Project *See step 3
                        CalcParameterOutputModel mSingleCalculation141 = new CalcParameterOutputModel();
                        mSingleCalculation141.parameterid = "141";
                        mSingleCalculation141.parametername = "Total Sales Cost Per Project";
                        double TempTotalSalesCostPerProjectPF = TotalSalesCostPerProject;
                        double TotalSalesCostPerProjectPF = (mParams.TotalSalesCostPerProject != TempTotalSalesCostPerProjectPF && mParams.TotalSalesCostPerProject != 0 ? mParams.TotalSalesCostPerProject : TempTotalSalesCostPerProjectPF);
                        mSingleCalculation141.value = TotalSalesCostPerProjectPF.ToString();
                        mCalcList.Add(mSingleCalculation141);

                        // - Total Land Value *See step 4
                        CalcParameterOutputModel mSingleCalculation123 = new CalcParameterOutputModel();
                        mSingleCalculation123.parameterid = "123";
                        mSingleCalculation123.parametername = "Total Land Value";
                        double TempTotalLandValuePF = TotalLandValue;
                        double TotalLandValuePF = (mParams.TotalLandValue != TempTotalLandValuePF && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValuePF);
                        mSingleCalculation123.value = TotalLandValuePF.ToString();
                        mCalcList.Add(mSingleCalculation123);

                        // - Profit For Project before financing(Sum of Step 5 above this line(Red is Negative Green is Positive Number for project)
                        CalcParameterOutputModel mSingleCalculation155 = new CalcParameterOutputModel();
                        mSingleCalculation155.parameterid = "155";
                        mSingleCalculation155.parametername = "Profit For Project before Financing";
                        double TempProfitForProjectbeforeFinancingPF = TotalBackendSalesPricePF - (TotalProjectCostPF + TotalSalesCostPerProjectPF + TotalLandValuePF);
                        double ProfitForProjectbeforeFinancingPF = (mParams.ProfitForProjectBeforeFinancing != TempProfitForProjectbeforeFinancingPF && mParams.ProfitForProjectBeforeFinancing != 0 ? mParams.ProfitForProjectBeforeFinancing : TempProfitForProjectbeforeFinancingPF);
                        mSingleCalculation155.value = ProfitForProjectbeforeFinancingPF.ToString();
                        mCalcList.Add(mSingleCalculation155);

                        // -Total Financing Costs*See Step 6
                        // CalcParameterOutputModel mSingleCalculation162 = new CalcParameterOutputModel();
                        // mSingleCalculation162.parameterid = "162";
                        // mSingleCalculation162.parametername = "Total Financing Cost";
                        // double TotalFinancingCostPF = TotalBackendSalesPricePF - (TotalProjectCostPF + TotalSalesCostPerProjectPF + TotalLandValuePF);
                        // mSingleCalculation162.value = ProfitForProjectbeforeFinancingPF.ToString();
                        // mCalcList.Add(mSingleCalculation162);


                        // - Net Profit For Project(Profit For Project before financing - Total Financing Costs)
                        CalcParameterOutputModel mSingleCalculation159 = new CalcParameterOutputModel();
                        mSingleCalculation159.parameterid = "159";
                        mSingleCalculation159.parametername = "Net Profit For Project";
                        double TempNetProfitForProjectPF = ProfitForProjectbeforeFinancingPF - TotalFinancingCostPF;
                        double NetProfitForProjectPF = (mParams.NetProfitForProject != TempNetProfitForProjectPF && mParams.NetProfitForProject != 0 ? mParams.NetProfitForProject : TempNetProfitForProjectPF);
                        mSingleCalculation159.value = NetProfitForProjectPF.ToString();
                        mCalcList.Add(mSingleCalculation159);

                    }

                    else if (mParams.StructureType == "Townhome" || mParams.StructureType == "Rowhouse" || mParams.StructureType == "Cottage")
                    {
                        // Sellable Sqft.Factor(1.07) Default 96
                        CalcParameterOutputModel mSingleCalculation96 = new CalcParameterOutputModel();
                        mSingleCalculation96.parameterid = "96";
                        mSingleCalculation96.parametername = "Sellable Sqft Factor";
                        double TempSellableSqftFactor = mSellableSqftFactor;
                        double SellableSqftFactor = (mParams.SellableSqftFactor != TempSellableSqftFactor && mParams.SellableSqftFactor != 0 ? mParams.SellableSqftFactor : TempSellableSqftFactor);
                        mSingleCalculation96.value = SellableSqftFactor.ToString();
                        mCalcList.Add(mSingleCalculation96);

                        //- Sellable Square Footage. (MAX FAR X Sellable Sqft. Factor / Number of Homes)
                        CalcParameterOutputModel mSingleCalculation67 = new CalcParameterOutputModel();
                        mSingleCalculation67.parameterid = "67";
                        mSingleCalculation67.parametername = "Sellable Square Footage";
                        double TempSellableSquareFootage = Convert.ToDouble(FarMax) / NumberOfHomes * SellableSqftFactor;
                        double SellableSquareFootage = (mParams.SellableSqFootage != TempSellableSquareFootage && mParams.SellableSqFootage != 0 ? mParams.SellableSqFootage : TempSellableSquareFootage);
                        mSingleCalculation67.value = SellableSquareFootage.ToString();
                        mCalcList.Add(mSingleCalculation67);

                        //2nd Step(Find out the Construction Costs Breakdown)
                        //-Townhome, Rowhouse or Cottage Base Cost Per Sqft. (Default)
                        double BaseCost = 0;
                        if (mParams.StructureType == "Townhome")
                        {
                            CalcParameterOutputModel mSingleCalculation110 = new CalcParameterOutputModel();
                            mSingleCalculation110.parameterid = "110";
                            mSingleCalculation110.parametername = "Townhome Base Cost Per Sqft";
                            double TempTownhomeBaseCostPerSqft = mTownhomeBaseCostPerSqft;
                            double TownhomeBaseCostPerSqft = (mParams.TownhomeBaseCostPerSqft != TempTownhomeBaseCostPerSqft && mParams.TownhomeBaseCostPerSqft != 0 ? mParams.TownhomeBaseCostPerSqft : TempTownhomeBaseCostPerSqft);
                            mSingleCalculation110.value = TownhomeBaseCostPerSqft.ToString();
                            mCalcList.Add(mSingleCalculation110);

                            CalcParameterOutputModel mSingleCalculation99 = new CalcParameterOutputModel();
                            mSingleCalculation99.parameterid = "99";
                            mSingleCalculation99.parametername = "Townhome Base Cost";
                            double TempTownhomeBaseCost = TownhomeBaseCostPerSqft * SellableSquareFootage;
                            double TownhomeBaseCost = (mParams.TownhomeBaseCost != TempTownhomeBaseCost && mParams.TownhomeBaseCost != 0 ? mParams.Equity : TempTownhomeBaseCost);
                            mSingleCalculation99.value = TownhomeBaseCost.ToString();
                            mCalcList.Add(mSingleCalculation99);
                            BaseCost = TownhomeBaseCost;
                        }
                        else if (mParams.StructureType == "Rowhouse")
                        {
                            CalcParameterOutputModel mSingleCalculation112 = new CalcParameterOutputModel();
                            mSingleCalculation112.parameterid = "112";
                            mSingleCalculation112.parametername = "Rowhouse Base Cost Per Sqft";
                            double TempRowhouseBaseCostPerSqft = mRowhouseBaseCostPerSqft;
                            double RowhouseBaseCostPerSqft = (mParams.RowhouseBaseCostPerSqft != TempRowhouseBaseCostPerSqft && mParams.RowhouseBaseCostPerSqft != 0 ? mParams.RowhouseBaseCostPerSqft : TempRowhouseBaseCostPerSqft);
                            mSingleCalculation112.value = RowhouseBaseCostPerSqft.ToString();
                            mCalcList.Add(mSingleCalculation112);

                            CalcParameterOutputModel mSingleCalculation100 = new CalcParameterOutputModel();
                            mSingleCalculation100.parameterid = "100";
                            mSingleCalculation100.parametername = "Rowhouse Base Cost";
                            double TempRowhouseBaseCost = RowhouseBaseCostPerSqft * SellableSquareFootage;
                            double RowhouseBaseCost = (mParams.RowhouseBaseCost != TempRowhouseBaseCost && mParams.RowhouseBaseCost != 0 ? mParams.RowhouseBaseCost : TempRowhouseBaseCost);
                            mSingleCalculation100.value = RowhouseBaseCost.ToString();
                            mCalcList.Add(mSingleCalculation100);
                            BaseCost = RowhouseBaseCost;
                        }
                        else if (mParams.StructureType == "Cottage")
                        {
                            CalcParameterOutputModel mSingleCalculation111 = new CalcParameterOutputModel();
                            mSingleCalculation111.parameterid = "111";
                            mSingleCalculation111.parametername = "Rowhouse Base Cost Per Sqft";
                            double TempCottageBaseCostPerSqft = mCottageBaseCostPerSqft;
                            double CottageBaseCostPerSqft = (mParams.CottageBaseCostPerSqft != TempCottageBaseCostPerSqft && mParams.CottageBaseCostPerSqft != 0 ? mParams.CottageBaseCostPerSqft : TempCottageBaseCostPerSqft);
                            mSingleCalculation111.value = CottageBaseCostPerSqft.ToString();
                            mCalcList.Add(mSingleCalculation111);

                            CalcParameterOutputModel mSingleCalculation101 = new CalcParameterOutputModel();
                            mSingleCalculation101.parameterid = "101";
                            mSingleCalculation101.parametername = "Cottage Base Cost";
                            double TempCottageBaseCost = CottageBaseCostPerSqft * SellableSquareFootage;
                            double CottageBaseCost = (mParams.CottageBaseCost != TempCottageBaseCost && mParams.CottageBaseCost != 0 ? mParams.CottageBaseCost : TempCottageBaseCost);
                            mSingleCalculation101.value = CottageBaseCost.ToString();
                            mCalcList.Add(mSingleCalculation101);
                            BaseCost = CottageBaseCost;
                        }

                        // -MHA Cost (MHA Fee Table Price X MAX FAR/Number of Units)
                        CalcParameterOutputModel mSingleCalculation121 = new CalcParameterOutputModel();
                        mSingleCalculation121.parameterid = "121";
                        mSingleCalculation121.parametername = "MHA Cost";
                        double TempMhaCost = Convert.ToDouble(mHaFee) * Convert.ToDouble(FarMax) / NumberOfHomes;
                        double MhaCost = (mParams.MhaCost != TempMhaCost && mParams.MhaCost != 0 ? mParams.MhaCost : TempMhaCost);
                        mSingleCalculation121.value = MhaCost.ToString();
                        mCalcList.Add(mSingleCalculation121);

                        //Total Building Cost Per Home(Sum of ALL RED TEXT)
                        CalcParameterOutputModel mSingleCalculation128 = new CalcParameterOutputModel();
                        mSingleCalculation128.parameterid = "128";
                        mSingleCalculation128.parametername = "Total Building Cost Per Home";
                        double TempTotalBuildingCostPerHomeCB = BaseCost + EcaCost + ElevationGradeCost + WaterLineCost + SewerCost + StormDrainCost + AlleyCost + UndergroundParkingCost + MhaCost;
                        double TotalBuildingCostPerHomeCB = (mParams.TotalBuildingCostPerHome != TempTotalBuildingCostPerHomeCB && mParams.TotalBuildingCostPerHome != 0 ? mParams.TotalBuildingCostPerHome : TempTotalBuildingCostPerHomeCB);
                        mSingleCalculation128.value = TotalBuildingCostPerHomeCB.ToString();
                        mCalcList.Add(mSingleCalculation128);

                        //Total Project Cost(Total Building Cost Per Lot X Number of Homes)
                        CalcParameterOutputModel mSingleCalculation108 = new CalcParameterOutputModel();
                        mSingleCalculation108.parameterid = "108";
                        mSingleCalculation108.parametername = "Total Project Cost";
                        double TempTotalProjectCostCB = TotalBuildingCostPerHomeCB * NumberOfHomes;
                        double TotalProjectCostCB = (mParams.TotalProjectCost != TempTotalProjectCostCB && mParams.TotalProjectCost != 0 ? mParams.TotalProjectCost : TempTotalProjectCostCB);
                        mSingleCalculation108.value = TotalProjectCostCB.ToString();
                        mCalcList.Add(mSingleCalculation108);

                        //3rd Step(Sales Cost Breakdown)
                        //-Home Estimated Value
                        CalcParameterOutputModel mSingleCalculation36 = new CalcParameterOutputModel();
                        mSingleCalculation36.parameterid = "36";
                        mSingleCalculation36.parametername = "Home Estimated Value";
                        double TempHomeEstimatedValueSCB = mHomeEstimatedValue;
                        double HomeEstimatedValueSCB = (mParams.HomeEstimatedValue != TempHomeEstimatedValueSCB && mParams.HomeEstimatedValue != 0 ? mParams.HomeEstimatedValue : TempHomeEstimatedValueSCB);
                        mSingleCalculation36.value = HomeEstimatedValueSCB.ToString();
                        mCalcList.Add(mSingleCalculation36);

                        //-Sales Commission(Sales Commission Table X Home Estimated Value)
                        CalcParameterOutputModel mSingleCalculation41 = new CalcParameterOutputModel();
                        mSingleCalculation41.parameterid = "41";
                        mSingleCalculation41.parametername = "Sales Commission";
                        double TempSalesCommissionSCB = mFactor2 * HomeEstimatedValueSCB;
                        double SalesCommissionSCB = (mParams.SalesCommission != TempSalesCommissionSCB && mParams.SalesCommission != 0 ? mParams.SalesCommission : TempSalesCommissionSCB);
                        mSingleCalculation41.value = SalesCommissionSCB.ToString();
                        mCalcList.Add(mSingleCalculation41);

                        //- Excise Tax(Excise Tax Table X Home Estimated Value)
                        CalcParameterOutputModel mSingleCalculation42 = new CalcParameterOutputModel();
                        mSingleCalculation42.parameterid = "42";
                        mSingleCalculation42.parametername = "Excise Tax";
                        double TempExciseTaxSCB = ExciseOutput(HomeEstimatedValueSCB);
                        double ExciseTaxSCB = (mParams.ExciseTax != TempExciseTaxSCB && mParams.ExciseTax != 0 ? mParams.ExciseTax : TempExciseTaxSCB);
                        mSingleCalculation42.value = ExciseTaxSCB.ToString();
                        mCalcList.Add(mSingleCalculation42);

                        //- Escrow Fees / Insurance(Escrow and Title Fee table X Home Estimated Value)
                        CalcParameterOutputModel mSingleCalculation43 = new CalcParameterOutputModel();
                        mSingleCalculation43.parameterid = "43";
                        mSingleCalculation43.parametername = "Escrow Fees";
                        double TempEscrowFeesSCB = mFactor4 * HomeEstimatedValueSCB;
                        double EscrowFeesSCB = (mParams.EscrowFees != TempEscrowFeesSCB && mParams.EscrowFees != 0 ? mParams.EscrowFees : TempEscrowFeesSCB);
                        mSingleCalculation43.value = EscrowFeesSCB.ToString();
                        mCalcList.Add(mSingleCalculation43);

                        //- Marketing / Staging(Marketing and Staging table X Home Estimated Value)
                        CalcParameterOutputModel mSingleCalculation44 = new CalcParameterOutputModel();
                        mSingleCalculation44.parameterid = "44";
                        mSingleCalculation44.parametername = "Marketing and Staging";
                        double TempMarketingandStagingSCB = mFactor5 * HomeEstimatedValueSCB;
                        double MarketingandStagingSCB = (mParams.MarketingAndStaging != TempMarketingandStagingSCB && mParams.MarketingAndStaging != 0 ? mParams.MarketingAndStaging : TempMarketingandStagingSCB);
                        mSingleCalculation44.value = MarketingandStagingSCB.ToString();
                        mCalcList.Add(mSingleCalculation44);

                        //- Total Sales Cost per Home(Sum of all blue)
                        CalcParameterOutputModel mSingleCalculation136 = new CalcParameterOutputModel();
                        mSingleCalculation136.parameterid = "136";
                        mSingleCalculation136.parametername = "Total Sales Cost Per Home";
                        double TempTotalSalesCostPerHomeSCB = SalesCommissionSCB + ExciseTaxSCB + EscrowFeesSCB + MarketingandStagingSCB;
                        double TotalSalesCostPerHomeSCB = (mParams.TotalSalesCostPerHome != TempTotalSalesCostPerHomeSCB && mParams.TotalSalesCostPerHome != 0 ? mParams.TotalSalesCostPerHome : TempTotalSalesCostPerHomeSCB);
                        mSingleCalculation136.value = TotalSalesCostPerHomeSCB.ToString();
                        mCalcList.Add(mSingleCalculation136);

                        //-Total Sales Cost per Project(Total Sales Cost per Home X Number of Townhomes)
                        CalcParameterOutputModel mSingleCalculation140 = new CalcParameterOutputModel();
                        mSingleCalculation140.parameterid = "140";
                        mSingleCalculation140.parametername = "Total Sales Cost Per Project";
                        double TempTotalSalesCostPerProjectSCB = TotalBuildingCostPerHomeCB * NumberOfHomes;
                        double TotalSalesCostPerProjectSCB = (mParams.TotalSalesCostPerProject != TempTotalSalesCostPerProjectSCB && mParams.TotalSalesCostPerProject != 0 ? mParams.TotalSalesCostPerProject : TempTotalSalesCostPerProjectSCB);
                        mSingleCalculation140.value = TotalSalesCostPerProjectSCB.ToString();
                        mCalcList.Add(mSingleCalculation140);

                        //4th Step(Land Analysis)
                        // Home Estimated Value
                        CalcParameterOutputModel mSingleCalculation168 = new CalcParameterOutputModel();
                        mSingleCalculation168.parameterid = "168";
                        mSingleCalculation168.parametername = "Home Estimated Value";
                        double TempHomeEstimatedValueLA = HomeEstimatedValueSCB;
                        double HomeEstimatedValueLA = (mParams.HomeEstimatedValue != TempHomeEstimatedValueLA && mParams.HomeEstimatedValue != 0 ? mParams.HomeEstimatedValue : TempHomeEstimatedValueLA);
                        mSingleCalculation168.value = HomeEstimatedValueLA.ToString();
                        mCalcList.Add(mSingleCalculation168);

                        //-Total Building Cost Per Home
                        CalcParameterOutputModel mSingleCalculation169 = new CalcParameterOutputModel();
                        mSingleCalculation169.parameterid = "169";
                        mSingleCalculation169.parametername = "Total Building Cost Per Home";
                        double TempTotalBuildingCostPerHomeLA = TotalSalesCostPerHomeSCB;
                        double TotalBuildingCostPerHomeLA = (mParams.TotalBuildingCostPerHome != TempTotalBuildingCostPerHomeLA && mParams.TotalBuildingCostPerHome != 0 ? mParams.TotalBuildingCostPerHome : TempTotalBuildingCostPerHomeLA);
                        mSingleCalculation169.value = TotalBuildingCostPerHomeLA.ToString();
                        mCalcList.Add(mSingleCalculation169);

                        //-Total Sales Cost Per Home
                        CalcParameterOutputModel mSingleCalculation173 = new CalcParameterOutputModel();
                        mSingleCalculation173.parameterid = "173";
                        mSingleCalculation173.parametername = "Total Sales Cost Per Home";
                        double TempTotalSalesCostPerHomeLA = TotalSalesCostPerHomeSCB;
                        double TotalSalesCostPerHomeLA = (mParams.TotalSalesCostPerHome != TempTotalSalesCostPerHomeLA && mParams.TotalSalesCostPerHome != 0 ? mParams.TotalSalesCostPerHome : TempTotalSalesCostPerHomeLA);
                        mSingleCalculation173.value = TotalSalesCostPerHomeLA.ToString();
                        mCalcList.Add(mSingleCalculation173);

                        // -Desired Profit Percent
                        CalcParameterOutputModel mSingleCalculation181 = new CalcParameterOutputModel();
                        mSingleCalculation181.parameterid = "181";
                        mSingleCalculation181.parametername = "Desired Profit Percent";
                        double TempDesiredProfitPercentLA = mDesiredProfitPercent;
                        double DesiredProfitPercentLA = (mParams.DesiredProfitPercent != TempDesiredProfitPercentLA && mParams.DesiredProfitPercent != 0 ? mParams.DesiredProfitPercent : TempDesiredProfitPercentLA);
                        mSingleCalculation181.value = DesiredProfitPercentLA.ToString();
                        mCalcList.Add(mSingleCalculation181);

                        //-Desired Profit before financing(Desired profit % X Lot Estimated Value)
                        CalcParameterOutputModel mSingleCalculation45 = new CalcParameterOutputModel();
                        mSingleCalculation45.parameterid = "45";
                        mSingleCalculation45.parametername = "Desired Profit Before Financing";
                        double TempDesiredProfitBeforeFinancingLA = DesiredProfitPercentLA * HomeEstimatedValueLA;
                        double DesiredProfitBeforeFinancingLA = (mParams.DesiredProfitBeforeFinancing != TempDesiredProfitBeforeFinancingLA && mParams.DesiredProfitBeforeFinancing != 0 ? mParams.DesiredProfitBeforeFinancing : TempDesiredProfitBeforeFinancingLA);
                        mSingleCalculation45.value = DesiredProfitBeforeFinancingLA.ToString();
                        mCalcList.Add(mSingleCalculation45);

                        //- Value of Land Per Home(Sum of Step 4 above this line(Red is Negative Green is Positive Number)
                        CalcParameterOutputModel mSingleCalculation46 = new CalcParameterOutputModel();
                        mSingleCalculation46.parameterid = "46";
                        mSingleCalculation46.parametername = "Value of Land Per Home";
                        double TempValueofLandPerHomeLA = HomeEstimatedValueLA - (TotalBuildingCostPerHomeLA + TotalSalesCostPerHomeLA + DesiredProfitPercentLA);
                        double ValueofLandPerHomeLA = (mParams.ValueofLandPerHome != TempValueofLandPerHomeLA && mParams.ValueofLandPerHome != 0 ? mParams.ValueofLandPerHome : TempValueofLandPerHomeLA);
                        mSingleCalculation46.value = ValueofLandPerHomeLA.ToString();
                        mCalcList.Add(mSingleCalculation46);

                        //- Total Land Value(Value of Land Per Lot X Number of Townhomes Homes)
                        CalcParameterOutputModel mSingleCalculation47 = new CalcParameterOutputModel();
                        mSingleCalculation47.parameterid = "47";
                        mSingleCalculation47.parametername = "Total Land Value";
                        double TempTotalLandValueLA = ValueofLandPerHomeLA * NumberOfHomes;
                        double TotalLandValueLA = (mParams.TotalLandValue != TempTotalLandValueLA && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValueLA);
                        mSingleCalculation47.value = TotalLandValueLA.ToString();
                        mCalcList.Add(mSingleCalculation47);

                        // Step 5 Financing
                        //Total Project Cost* Step 2
                        CalcParameterOutputModel mSingleCalculation178 = new CalcParameterOutputModel();
                        mSingleCalculation178.parameterid = "178";
                        mSingleCalculation178.parametername = "Total Project Cost";
                        double TempTotalProjectCostFI = TotalProjectCostCB;
                        double TotalProjectCostFI = (mParams.TotalProjectCost != TempTotalProjectCostFI && mParams.TotalProjectCost != 0 ? mParams.TotalProjectCost : TempTotalProjectCostFI);
                        mSingleCalculation178.value = TotalLandValueLA.ToString();
                        mCalcList.Add(mSingleCalculation178);

                        //Total Land Value* Step 4
                        CalcParameterOutputModel mSingleCalculation179 = new CalcParameterOutputModel();
                        mSingleCalculation179.parameterid = "179";
                        mSingleCalculation179.parametername = "Total Land Value";
                        double TempTotalLandValueFI = TotalLandValueLA;
                        double TotalLandValueFI = (mParams.TotalLandValue != TempTotalLandValueFI && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValueFI);
                        mSingleCalculation179.value = TotalLandValueFI.ToString();
                        mCalcList.Add(mSingleCalculation179);

                        //Equity(Manual Input)
                        CalcParameterOutputModel mSingleCalculation55 = new CalcParameterOutputModel();
                        mSingleCalculation55.parameterid = "55";
                        mSingleCalculation55.parametername = "Equity";
                        double TempEquityFI = mEquity;
                        double EquityFI = (mParams.Equity != TempEquityFI && mParams.Equity != 0 ? mParams.Equity : TempEquityFI);
                        mSingleCalculation55.value = EquityFI.ToString();
                        mCalcList.Add(mSingleCalculation55);

                        //Acquisition Carrying Months(Manual Input)
                        CalcParameterOutputModel mSingleCalculation56 = new CalcParameterOutputModel();
                        mSingleCalculation56.parameterid = "56";
                        mSingleCalculation56.parametername = "Acquisition Carrying Months";
                        double TempAcquisitionCarryingMonthsFI = mAcquisitionCarryingMonths;
                        double AcquisitionCarryingMonthsFI = (mParams.AcquisitionCarryingMonths != TempAcquisitionCarryingMonthsFI && mParams.AcquisitionCarryingMonths != 0 ? mParams.AcquisitionCarryingMonths : TempAcquisitionCarryingMonthsFI);
                        mSingleCalculation56.value = AcquisitionCarryingMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation56);

                        //Interest Rate Land Acquisition(Manual Input)
                        CalcParameterOutputModel mSingleCalculation57 = new CalcParameterOutputModel();
                        mSingleCalculation57.parameterid = "57";
                        mSingleCalculation57.parametername = "Interest Rate Land Acquisition";
                        double TempInterestRateLandAcquisitionFI = mInterestRateLandAcquisition;
                        double InterestRateLandAcquisitionFI = (mParams.InterestRateLandAcquisition != TempInterestRateLandAcquisitionFI && mParams.InterestRateLandAcquisition != 0 ? mParams.InterestRateLandAcquisition : TempInterestRateLandAcquisitionFI);
                        mSingleCalculation57.value = InterestRateLandAcquisitionFI.ToString();
                        mCalcList.Add(mSingleCalculation57);

                        //Loan Fees Land Acquisition Loan(Manual Input)
                        CalcParameterOutputModel mSingleCalculation81 = new CalcParameterOutputModel();
                        mSingleCalculation81.parameterid = "81";
                        mSingleCalculation81.parametername = "Loan Fees Land Acquisition Loan";
                        double TempLoanFeesLandAcquisitionLoanFI = mLoanFeesLandAcquisitionLoan;
                        double LoanFeesLandAcquisitionLoanFI = (mParams.LoanFeesLandAcquisitionLoan != TempLoanFeesLandAcquisitionLoanFI && mParams.LoanFeesLandAcquisitionLoan != 0 ? mParams.LoanFeesLandAcquisitionLoan : TempLoanFeesLandAcquisitionLoanFI);
                        mSingleCalculation81.value = LoanFeesLandAcquisitionLoanFI.ToString();
                        mCalcList.Add(mSingleCalculation81);

                        //Total Financing Cost Land Acquisition((Land Value – Equity) * (Interest Rate Land Acquisition * (Acquisition Carrying Months/ 12) +(Land Value – Equity)*(Loan Fees Land Acquisition Loan)
                        CalcParameterOutputModel mSingleCalculation58 = new CalcParameterOutputModel();
                        mSingleCalculation58.parameterid = "58";
                        mSingleCalculation58.parametername = "Total Financing Cost Land Acquisition";
                        double TempTotalFinancingCostLandAcquisitionFI = ((TotalLandValueFI - EquityFI) * InterestRateLandAcquisitionFI * (AcquisitionCarryingMonthsFI / 12) + (TotalLandValueFI - EquityFI) * (LoanFeesLandAcquisitionLoanFI));
                        double TotalFinancingCostLandAcquisitionFI = (mParams.TotalFinancingCostLandAcquisition != TempTotalFinancingCostLandAcquisitionFI && mParams.TotalFinancingCostLandAcquisition != 0 ? mParams.TotalFinancingCostLandAcquisition : TempTotalFinancingCostLandAcquisitionFI);
                        mSingleCalculation58.value = TotalFinancingCostLandAcquisitionFI.ToString();
                        mCalcList.Add(mSingleCalculation58);

                        //Interest Rate Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation59 = new CalcParameterOutputModel();
                        mSingleCalculation59.parameterid = "59";
                        mSingleCalculation59.parametername = "Interest Rate Construction Loan";
                        double TempInterestRateConstructionLoanFI = mInterestRateConstructionLoan;
                        double InterestRateConstructionLoanFI = (mParams.InterestRateConstructionLoan != TempInterestRateConstructionLoanFI && mParams.InterestRateConstructionLoan != 0 ? mParams.InterestRateConstructionLoan : TempInterestRateConstructionLoanFI);
                        mSingleCalculation59.value = InterestRateConstructionLoanFI.ToString();
                        mCalcList.Add(mSingleCalculation59);

                        //Loan Fees Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation60 = new CalcParameterOutputModel();
                        mSingleCalculation60.parameterid = "60";
                        mSingleCalculation60.parametername = "Loan Fees Construction Loan";
                        double TempLoanFeesConstructionLoanFI = mLoanFeesConstructionLoan;
                        double LoanFeesConstructionLoanFI = (mParams.LoanFeesConstructionLoan != TempLoanFeesConstructionLoanFI && mParams.LoanFeesConstructionLoan != 0 ? mParams.LoanFeesConstructionLoan : TempLoanFeesConstructionLoanFI);
                        mSingleCalculation60.value = LoanFeesConstructionLoanFI.ToString();
                        mCalcList.Add(mSingleCalculation60);

                        //Construction Loan Months(Manual Default)
                        CalcParameterOutputModel mSingleCalculation61 = new CalcParameterOutputModel();
                        mSingleCalculation61.parameterid = "61";
                        mSingleCalculation61.parametername = "Construction Loan Months";
                        double TempConstructionLoanMonthsFI = mConstructionLoanMonths;
                        double ConstructionLoanMonthsFI = (mParams.ConstructionLoanMonths != TempConstructionLoanMonthsFI && mParams.ConstructionLoanMonths != 0 ? mParams.ConstructionLoanMonths : TempConstructionLoanMonthsFI);
                        mSingleCalculation61.value = ConstructionLoanMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation61);

                        //Total Financing Construction Cost(((Land Value -Equity) +(Total Project Cost/ 2) *((Interest Rate Construction Loan”) *(Construction Loan Months/ 12)) +((Land Value – Equity) *(Loan Fees Construction Loan”)))
                        CalcParameterOutputModel mSingleCalculation62 = new CalcParameterOutputModel();
                        mSingleCalculation62.parameterid = "62";
                        mSingleCalculation62.parametername = "Total Financing Cost Construction";
                        double TempTotalFinancingCostConstructionFI = ((TotalLandValueFI - EquityFI) + (TotalProjectCostFI / 2) * ((InterestRateConstructionLoanFI) * (ConstructionLoanMonthsFI / 12)) + ((TotalLandValueFI - EquityFI) * (LoanFeesConstructionLoanFI)));
                        double TotalFinancingCostConstructionFI = (mParams.TotalFinancingCostConstruction != TempTotalFinancingCostConstructionFI && mParams.TotalFinancingCostConstruction != 0 ? mParams.TotalFinancingCostConstruction : TempTotalFinancingCostConstructionFI);
                        mSingleCalculation62.value = TotalFinancingCostConstructionFI.ToString();
                        mCalcList.Add(mSingleCalculation62);

                        //Months to Sell after completion(Manual Default)
                        CalcParameterOutputModel mSingleCalculation64 = new CalcParameterOutputModel();
                        mSingleCalculation64.parameterid = "64";
                        mSingleCalculation64.parametername = "Months to Sell After completion";
                        double TempMonthsToSellAftercompletionFI = ((TotalLandValueFI - EquityFI) + (TotalProjectCostFI / 2) * ((InterestRateConstructionLoanFI) * (ConstructionLoanMonthsFI / 12)) + ((TotalLandValueFI - EquityFI) * (LoanFeesConstructionLoanFI)));
                        double MonthsToSellAftercompletionFI = (mParams.MonthsToSellAfterCompletion != TempMonthsToSellAftercompletionFI && mParams.MonthsToSellAfterCompletion != 0 ? mParams.MonthsToSellAfterCompletion : TempMonthsToSellAftercompletionFI);
                        mSingleCalculation64.value = MonthsToSellAftercompletionFI.ToString();
                        mCalcList.Add(mSingleCalculation64);

                        //Extra Financing After Completion(Land Value +Total Project Cost – Contributed Equity) *(Interest Rate Construction Loan * (Months to Sell after completion Months / 12)  
                        CalcParameterOutputModel mSingleCalculation65 = new CalcParameterOutputModel();
                        mSingleCalculation65.parameterid = "65";
                        mSingleCalculation65.parametername = "Extra Financing After Completion";
                        double TempExtraFinancingAfterCompletionFI = (TotalLandValueFI + TotalProjectCostFI - EquityFI) * (InterestRateConstructionLoanFI * (MonthsToSellAftercompletionFI / 12));
                        double ExtraFinancingAfterCompletionFI = (mParams.ExtraFinancingAfterCompletion != TempExtraFinancingAfterCompletionFI && mParams.ExtraFinancingAfterCompletion != 0 ? mParams.ExtraFinancingAfterCompletion : TempExtraFinancingAfterCompletionFI);
                        mSingleCalculation65.value = ExtraFinancingAfterCompletionFI.ToString();
                        mCalcList.Add(mSingleCalculation65);

                        //Total Financing Fees(Sum of Green Text)
                        CalcParameterOutputModel mSingleCalculation66 = new CalcParameterOutputModel();
                        mSingleCalculation66.parameterid = "66";
                        mSingleCalculation66.parametername = "Total Financing Fees";
                        double TempTotalFinancingFeesFI = TotalFinancingCostLandAcquisitionFI + TotalFinancingCostConstructionFI + ExtraFinancingAfterCompletionFI;
                        double TotalFinancingFeesFI = (mParams.TotalFinancingFees != TempTotalFinancingFeesFI && mParams.TotalFinancingFees != 0 ? mParams.TotalFinancingFees : TempTotalFinancingFeesFI);
                        mSingleCalculation66.value = TotalFinancingFeesFI.ToString();
                        mCalcList.Add(mSingleCalculation66);

                        //Step 6(Proformas & Financing)
                        //PEFORMA PER HOME

                        //-Home Estimated Value
                        CalcParameterOutputModel mSingleCalculation167 = new CalcParameterOutputModel();
                        mSingleCalculation167.parameterid = "167";
                        mSingleCalculation167.parametername = "Home Estimated Value";
                        double TempHomeEstimatedValuePF = HomeEstimatedValueSCB;
                        double HomeEstimatedValuePF = (mParams.HomeEstimatedValue != TempHomeEstimatedValuePF && mParams.HomeEstimatedValue != 0 ? mParams.HomeEstimatedValue : TempHomeEstimatedValuePF);
                        mSingleCalculation167.value = HomeEstimatedValuePF.ToString();
                        mCalcList.Add(mSingleCalculation167);

                        // -Total Building Cost Per Home
                        CalcParameterOutputModel mSingleCalculation127 = new CalcParameterOutputModel();
                        mSingleCalculation127.parameterid = "127";
                        mSingleCalculation127.parametername = "Total Building Cost Per Home";
                        double TempTotalBuildingCostPerHomePF = TotalBuildingCostPerHomeCB;
                        double TotalBuildingCostPerHomePF = (mParams.TotalBuildingCostPerHome != TempTotalBuildingCostPerHomePF && mParams.TotalBuildingCostPerHome != 0 ? mParams.TotalBuildingCostPerHome : TempTotalBuildingCostPerHomePF);
                        mSingleCalculation127.value = TotalBuildingCostPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation127);

                        // -Total Sales Cost Per Home
                        CalcParameterOutputModel mSingleCalculation137 = new CalcParameterOutputModel();
                        mSingleCalculation137.parameterid = "137";
                        mSingleCalculation137.parametername = "Total Sales Cost Per Home";
                        double TempTotalSalesCostPerHomePF = TotalSalesCostPerHomeSCB;
                        double TotalSalesCostPerHomePF = (mParams.TotalSalesCostPerHome != TempTotalSalesCostPerHomePF && mParams.TotalSalesCostPerHome != 0 ? mParams.TotalSalesCostPerHome : TempTotalSalesCostPerHomePF);
                        mSingleCalculation137.value = TotalSalesCostPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation137);

                        // -Value of Land Per Home
                        CalcParameterOutputModel mSingleCalculation122 = new CalcParameterOutputModel();
                        mSingleCalculation122.parameterid = "122";
                        mSingleCalculation122.parametername = "Value of Land Per Home";
                        double TempValueofLandPerHomePF = ValueofLandPerHomeLA;
                        double ValueofLandPerHomePF = (mParams.ValueofLandPerHome != TempValueofLandPerHomePF && mParams.ValueofLandPerHome != 0 ? mParams.ValueofLandPerHome : TempValueofLandPerHomePF);
                        mSingleCalculation122.value = ValueofLandPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation122);

                        // -Profit per home before Financing (Sum of Step 5 above this line (Red is Negative Green is Positive Number for per home)
                        CalcParameterOutputModel mSingleCalculation50 = new CalcParameterOutputModel();
                        mSingleCalculation50.parameterid = "50";
                        mSingleCalculation50.parametername = "Profit per home before Financing";
                        double TempProfitPerHomeBeforeFinancingPF = HomeEstimatedValuePF - (TotalBuildingCostPerHomePF + TotalSalesCostPerHomePF + ValueofLandPerHomePF);
                        double ProfitPerHomeBeforeFinancingPF = (mParams.ProfitPerHomeBeforeFinancing != TempProfitPerHomeBeforeFinancingPF && mParams.ProfitPerHomeBeforeFinancing != 0 ? mParams.ProfitPerHomeBeforeFinancing : TempProfitPerHomeBeforeFinancingPF);
                        mSingleCalculation50.value = ValueofLandPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation50);

                        // -Total Financing Costs Per Home (Total financing Cost/Number of Townhomes Homes)
                        CalcParameterOutputModel mSingleCalculation164 = new CalcParameterOutputModel();
                        mSingleCalculation164.parameterid = "164";
                        mSingleCalculation164.parametername = "Total Financing Cost Per Home";
                        double TempTotalFinancingCostPerHomePF = TotalFinancingFeesFI / NumberOfHomes;
                        double TotalFinancingCostPerHomePF = (mParams.TotalFinancingCostPerHome != TempTotalFinancingCostPerHomePF && mParams.TotalFinancingCostPerHome != 0 ? mParams.TotalFinancingCostPerHome : TempTotalFinancingCostPerHomePF);
                        mSingleCalculation164.value = TotalFinancingCostPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation164);

                        // -Net Profit Per Home (Profit per home before Financing - Financing Costs)
                        CalcParameterOutputModel mSingleCalculation157 = new CalcParameterOutputModel();
                        mSingleCalculation157.parameterid = "157";
                        mSingleCalculation157.parametername = "Net Profit Per Home";
                        double TempNetProfitPerHomePF = ProfitPerHomeBeforeFinancingPF - TotalFinancingCostPerHomePF;
                        double NetProfitPerHomePF = (mParams.NetProfitPerHome != TempNetProfitPerHomePF && mParams.NetProfitPerHome != 0 ? mParams.NetProfitPerHome : TempNetProfitPerHomePF);
                        mSingleCalculation157.value = NetProfitPerHomePF.ToString();
                        mCalcList.Add(mSingleCalculation157);

                        // PROFORMA PER PROJECT

                        //- Backend Sales Price
                        CalcParameterOutputModel mSingleCalculation166 = new CalcParameterOutputModel();
                        mSingleCalculation166.parameterid = "166";
                        mSingleCalculation166.parametername = "Total Backend Sales Price";
                        double TempTotalBackendSalesPricePF = HomeEstimatedValuePF * NumberOfHomes;
                        double TotalBackendSalesPricePF = (mParams.BackendSalesPrice != TempTotalBackendSalesPricePF && mParams.BackendSalesPrice != 0 ? mParams.BackendSalesPrice : TempTotalBackendSalesPricePF);
                        mSingleCalculation166.value = TotalBackendSalesPricePF.ToString();
                        mCalcList.Add(mSingleCalculation166);

                        // -Total Project Cost
                        CalcParameterOutputModel mSingleCalculation182 = new CalcParameterOutputModel();
                        mSingleCalculation182.parameterid = "182";
                        mSingleCalculation182.parametername = "Total Project Cost";
                        double TempTotalProjectCostPF = TotalProjectCostFI;
                        double TotalProjectCostPF = (mParams.TotalProjectCost != TempTotalProjectCostPF && mParams.TotalProjectCost != 0 ? mParams.TotalProjectCost : TempTotalProjectCostPF);
                        mSingleCalculation182.value = TotalProjectCostPF.ToString();
                        mCalcList.Add(mSingleCalculation182);

                        // -Total Sales Cost Per Project
                        CalcParameterOutputModel mSingleCalculation141 = new CalcParameterOutputModel();
                        mSingleCalculation141.parameterid = "141";
                        mSingleCalculation141.parametername = "Total Sales Cost Per Project";
                        double TempTotalSalesCostPerProjectPF = TotalSalesCostPerProjectSCB;
                        double TotalSalesCostPerProjectPF = (mParams.TotalSalesCostPerProject != TempTotalSalesCostPerProjectPF && mParams.TotalSalesCostPerProject != 0 ? mParams.TotalSalesCostPerProject : TempTotalSalesCostPerProjectPF);
                        mSingleCalculation141.value = TotalSalesCostPerProjectPF.ToString();
                        mCalcList.Add(mSingleCalculation141);

                        // -Total Land Value *See step 4
                        CalcParameterOutputModel mSingleCalculation123 = new CalcParameterOutputModel();
                        mSingleCalculation123.parameterid = "123";
                        mSingleCalculation123.parametername = "Total Land Value";
                        double TempTotalLandValuePF = TotalLandValueFI;
                        double TotalLandValuePF = (mParams.TotalLandValue != TempTotalLandValuePF && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValuePF);
                        mSingleCalculation123.value = TotalLandValuePF.ToString();
                        mCalcList.Add(mSingleCalculation123);

                        //-Profit For Project before Financing(Sum of Step 5 above this line(Red is Negative Green is Positive Number for per home)
                        // TODO: is this correct?
                        CalcParameterOutputModel mSingleCalculation155 = new CalcParameterOutputModel();
                        mSingleCalculation155.parameterid = "155";
                        mSingleCalculation155.parametername = "Profit For Project before Financing";
                        double TempProfitForProjectBeforeFinancingPFP = TotalBackendSalesPricePF - (TotalProjectCostPF + TotalSalesCostPerProjectPF + TotalLandValuePF);
                        double ProfitForProjectBeforeFinancingPFP = (mParams.ProfitForProjectBeforeFinancing != TempProfitForProjectBeforeFinancingPFP && mParams.ProfitForProjectBeforeFinancing != 0 ? mParams.ProfitForProjectBeforeFinancing : TempProfitForProjectBeforeFinancingPFP);
                        mSingleCalculation155.value = ProfitForProjectBeforeFinancingPFP.ToString();
                        mCalcList.Add(mSingleCalculation155);

                        // -Total Financing Costs* See Step 6
                        CalcParameterOutputModel mSingleCalculation162 = new CalcParameterOutputModel();
                        mSingleCalculation162.parameterid = "162";
                        mSingleCalculation162.parametername = "Total Financing Cost";
                        double TempTotalFinancingCostPFP = TotalFinancingFeesFI;
                        double TotalFinancingCostPFP = (mParams.TotalFinancingFees != TempTotalFinancingCostPFP && mParams.TotalFinancingFees != 0 ? mParams.TotalFinancingFees : TempTotalFinancingCostPFP);
                        mSingleCalculation162.value = TotalFinancingCostPFP.ToString();
                        mCalcList.Add(mSingleCalculation162);

                        // -Net Profit For Project (Profit For Project before financing - Total Financing Costs)
                        CalcParameterOutputModel mSingleCalculation159 = new CalcParameterOutputModel();
                        mSingleCalculation159.parameterid = "159";
                        mSingleCalculation159.parametername = "Net Profit For Project";
                        double TempNetProfitForProjectPFP = ProfitForProjectBeforeFinancingPFP - TotalFinancingCostPFP;
                        double NetProfitForProjectPFP = (mParams.NetProfitForProject != TempNetProfitForProjectPFP && mParams.NetProfitForProject != 0 ? mParams.NetProfitForProject : TempNetProfitForProjectPFP);
                        mSingleCalculation159.value = NetProfitForProjectPFP.ToString();
                        mCalcList.Add(mSingleCalculation159);
                    }

                    else if (mParams.StructureType == "Condo")
                    {
                        // - Livable Sqft (MAX FAR) – ((Inefficiency Space Percentage X MAX FAR))
                        CalcParameterOutputModel mSingleCalculation71 = new CalcParameterOutputModel();
                        mSingleCalculation71.parameterid = "71";
                        mSingleCalculation71.parametername = "Livable Sqft";
                        double TempLivableSqft = mInefficientSpacePercentage * Convert.ToDouble(FarMax);
                        double LivableSqft = (mParams.LivableSqft != TempLivableSqft && mParams.LivableSqft != 0 ? mParams.LivableSqft : TempLivableSqft);
                        mSingleCalculation71.value = LivableSqft.ToString();
                        mCalcList.Add(mSingleCalculation71);

                        // -Number of Units (Lot Size/Minimum Lot size) rounded down to the nearest whole number
                        CalcParameterOutputModel mSingleCalculation80 = new CalcParameterOutputModel();
                        mSingleCalculation80.parameterid = "80";
                        mSingleCalculation80.parametername = "Number Of Units";
                        double TempNumberofUnits = mParams.LotSize / Convert.ToDouble(mDig);
                        double NumberofUnits = (mParams.NumberOfUnits != TempNumberofUnits && mParams.NumberOfUnits != 0 ? mParams.NumberOfUnits : TempNumberofUnits);
                        mSingleCalculation80.value = NumberofUnits.ToString();
                        mCalcList.Add(mSingleCalculation80);

                        // -Sellable square footage. (Livable Sqft. /Number of Units)
                        CalcParameterOutputModel mSingleCalculation67 = new CalcParameterOutputModel();
                        mSingleCalculation67.parameterid = "67";
                        mSingleCalculation67.parametername = "Sellable Square Footage";
                        double TempSellableSquareFootage = LivableSqft / NumberofUnits;
                        double SellableSquareFootage = (mParams.SellableSqFootage != TempSellableSquareFootage && mParams.SellableSqFootage != 0 ? mParams.SellableSqFootage : TempSellableSquareFootage);
                        mSingleCalculation67.value = SellableSquareFootage.ToString();
                        mCalcList.Add(mSingleCalculation67);


                        // Step 2 (Find out the Construction Costs Breakdown)

                        // - Condo Base Cost Per Sqft. (Default)
                        CalcParameterOutputModel mSingleCalculation114 = new CalcParameterOutputModel();
                        mSingleCalculation114.parameterid = "114";
                        mSingleCalculation114.parametername = "Condo Base Cost Per Sqft";
                        double TempCondoBaseCostPerSqft = mCondoBaseCostPerSqft;
                        double CondoBaseCostPerSqft = (mParams.CondoBaseCostPerSqft != TempCondoBaseCostPerSqft && mParams.CondoBaseCostPerSqft != 0 ? mParams.CondoBaseCostPerSqft : TempCondoBaseCostPerSqft);
                        mSingleCalculation114.value = CondoBaseCostPerSqft.ToString();
                        mCalcList.Add(mSingleCalculation114);

                        // - Condo Base Cost 
                        CalcParameterOutputModel mSingleCalculation103 = new CalcParameterOutputModel();
                        mSingleCalculation103.parameterid = "103";
                        mSingleCalculation103.parametername = "Condo Base Cost";
                        double TempCondoBaseCost = CondoBaseCostPerSqft;
                        double CondoBaseCost = (mParams.CondoBaseCost != TempCondoBaseCost && mParams.CondoBaseCost != 0 ? mParams.CondoBaseCost : TempCondoBaseCost);
                        mSingleCalculation103.value = CondoBaseCost.ToString();
                        mCalcList.Add(mSingleCalculation103);

                        // -MHA Cost (MHA Fee Table Price X MAX FAR/ Number of Units)
                        CalcParameterOutputModel mSingleCalculation121 = new CalcParameterOutputModel();
                        mSingleCalculation121.parameterid = "121";
                        mSingleCalculation121.parametername = "MHA Cost";
                        double TempMhaCost = Convert.ToDouble(mHaFee) * Convert.ToDouble(FarMax) / NumberofUnits;
                        double MhaCost = (mParams.MhaCost != TempMhaCost && mParams.MhaCost != 0 ? mParams.MhaCost : TempMhaCost);
                        mSingleCalculation121.value = MhaCost.ToString();
                        mCalcList.Add(mSingleCalculation121);

                        // Total Building Cost Per Unit(Sum of ALL RED TEXT)
                        CalcParameterOutputModel mSingleCalculation126 = new CalcParameterOutputModel();
                        mSingleCalculation126.parameterid = "126";
                        mSingleCalculation126.parametername = "Total Building Cost Per Unit";
                        double TempTotalBuildingCostPerUnitCB = EcaCost + ElevationGradeCost + WaterLineCost + SewerCost + StormDrainCost + AlleyCost + UndergroundParkingCost + MhaCost;
                        double TotalBuildingCostPerUnitCB = (mParams.TotalBuildingCostPerUnit != TempTotalBuildingCostPerUnitCB && mParams.TotalBuildingCostPerUnit != 0 ? mParams.TotalBuildingCostPerUnit : TempTotalBuildingCostPerUnitCB);
                        mSingleCalculation126.value = TotalBuildingCostPerUnitCB.ToString();
                        mCalcList.Add(mSingleCalculation126);

                        // Total Project Cost(Total Building Cost Per Unit X Number of Units)
                        CalcParameterOutputModel mSingleCalculation108 = new CalcParameterOutputModel();
                        mSingleCalculation108.parameterid = "108";
                        mSingleCalculation108.parametername = "Total Project Cost";
                        double TempTotalProjectCostCB = TotalBuildingCostPerUnitCB * NumberofUnits;
                        double TotalProjectCostCB = (mParams.TotalProjectCost != TempTotalProjectCostCB && mParams.TotalProjectCost != 0 ? mParams.TotalProjectCost : TempTotalProjectCostCB);
                        mSingleCalculation108.value = TotalProjectCostCB.ToString();
                        mCalcList.Add(mSingleCalculation108);

                        // Step 3 Sales Cost Breakdown

                        // Unit Estimated Value
                        CalcParameterOutputModel mSingleCalculation142 = new CalcParameterOutputModel();
                        mSingleCalculation142.parameterid = "142";
                        mSingleCalculation142.parametername = "Unit Estimated Value";
                        double TempUnitEstimatedValueSCB = mParams.UnitEstimatedValue;
                        double UnitEstimatedValueSCB = (mParams.UnitEstimatedValue != TempUnitEstimatedValueSCB && mParams.UnitEstimatedValue != 0 ? mParams.UnitEstimatedValue : TempUnitEstimatedValueSCB);
                        mSingleCalculation142.value = UnitEstimatedValueSCB.ToString();
                        mCalcList.Add(mSingleCalculation142);

                        //-Sales Commission(Sales Commission Table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation41 = new CalcParameterOutputModel();
                        mSingleCalculation41.parameterid = "41";
                        mSingleCalculation41.parametername = "Sales Commission";
                        double TempSalesCommissionSCB = mFactor2 * UnitEstimatedValueSCB;
                        double SalesCommissionSCB = (mParams.SalesCommission != TempSalesCommissionSCB && mParams.SalesCommission != 0 ? mParams.SalesCommission : TempSalesCommissionSCB);
                        mSingleCalculation41.value = SalesCommissionSCB.ToString();
                        mCalcList.Add(mSingleCalculation41);

                        //- Excise Tax(Excise Tax Table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation42 = new CalcParameterOutputModel();
                        mSingleCalculation42.parameterid = "42";
                        mSingleCalculation42.parametername = "Excise Tax";
                        double TempExciseTaxSCB = ExciseOutput(UnitEstimatedValueSCB);
                        double ExciseTaxSCB = (mParams.ExciseTax != TempExciseTaxSCB && mParams.ExciseTax != 0 ? mParams.ExciseTax : TempExciseTaxSCB);
                        mSingleCalculation42.value = ExciseTaxSCB.ToString();
                        mCalcList.Add(mSingleCalculation42);

                        //- Escrow Fees / Insurance(Escrow and Title Fee table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation43 = new CalcParameterOutputModel();
                        mSingleCalculation43.parameterid = "43";
                        mSingleCalculation43.parametername = "Escrow Fees";
                        double TempEscrowFeesSCB = mFactor4 * UnitEstimatedValueSCB;
                        double EscrowFeesSCB = (mParams.EscrowFees != TempEscrowFeesSCB && mParams.EscrowFees != 0 ? mParams.EscrowFees : TempEscrowFeesSCB);
                        mSingleCalculation43.value = EscrowFeesSCB.ToString();
                        mCalcList.Add(mSingleCalculation43);

                        //- Marketing / Staging(Marketing and Staging table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation44 = new CalcParameterOutputModel();
                        mSingleCalculation44.parameterid = "44";
                        mSingleCalculation44.parametername = "Marketing and Staging";
                        double TempMarketingandStagingSCB = mFactor5 * UnitEstimatedValueSCB;
                        double MarketingandStagingSCB = (mParams.MarketingAndStaging != TempMarketingandStagingSCB && mParams.MarketingAndStaging != 0 ? mParams.MarketingAndStaging : TempMarketingandStagingSCB);
                        mSingleCalculation44.value = MarketingandStagingSCB.ToString();
                        mCalcList.Add(mSingleCalculation44);

                        //- OCIP Policy(3.5 % of Total Project Cost *)
                        CalcParameterOutputModel mSingleCalculation130 = new CalcParameterOutputModel();
                        mSingleCalculation130.parameterid = "130";
                        mSingleCalculation130.parametername = "OCIP Policy";
                        double TempOCIPPolicySCB = 0.035 * TotalProjectCostCB;
                        double OCIPPolicySCB = (mParams.OcipPolicy != TempOCIPPolicySCB && mParams.OcipPolicy != 0 ? mParams.OcipPolicy : TempOCIPPolicySCB);
                        mSingleCalculation130.value = OCIPPolicySCB.ToString();
                        mCalcList.Add(mSingleCalculation130);

                        //- Total Sales Cost per Unit(Sum of all blue)
                        CalcParameterOutputModel mSingleCalculation138 = new CalcParameterOutputModel();
                        mSingleCalculation138.parameterid = "138";
                        mSingleCalculation138.parametername = "Total Sales Cost Per Unit";
                        double TempTotalSalesCostPerUnitSCB = SalesCommissionSCB + ExciseTaxSCB + EscrowFeesSCB + MarketingandStagingSCB + OCIPPolicySCB;
                        double TotalSalesCostPerUnitSCB = (mParams.TotalSalesCostPerUnit != TempTotalSalesCostPerUnitSCB && mParams.TotalSalesCostPerUnit != 0 ? mParams.TotalSalesCostPerUnit : TempTotalSalesCostPerUnitSCB);
                        mSingleCalculation138.value = TotalSalesCostPerUnitSCB.ToString();
                        mCalcList.Add(mSingleCalculation138);

                        //-Total Sales Cost per Project(Total Sales Cost per Unit X Number of Units)
                        CalcParameterOutputModel mSingleCalculation140 = new CalcParameterOutputModel();
                        mSingleCalculation140.parameterid = "140";
                        mSingleCalculation140.parametername = "Total Sales Cost Per Project";
                        double TempTotalSalesCostPerProjectSCB = TotalSalesCostPerUnitSCB * NumberofUnits;
                        double TotalSalesCostPerProjectSCB = (mParams.TotalSalesCostPerProject != TempTotalSalesCostPerProjectSCB && mParams.TotalSalesCostPerProject != 0 ? mParams.TotalSalesCostPerProject : TempTotalSalesCostPerProjectSCB);
                        mSingleCalculation140.value = TotalSalesCostPerUnitSCB.ToString();
                        mCalcList.Add(mSingleCalculation140);

                        // Land Analysis
                        //-Unit Estimated Value 144 "Unit Estimated Value"
                        CalcParameterOutputModel mSingleCalculation144 = new CalcParameterOutputModel();
                        mSingleCalculation144.parameterid = "144";
                        mSingleCalculation144.parametername = "Unit Estimated Value";
                        double TempUnitEstimatedValueLA = mUnitEstimatedValue;
                        double UnitEstimatedValueLA = (mParams.UnitEstimatedValue != TempUnitEstimatedValueLA && mParams.UnitEstimatedValue != 0 ? mParams.UnitEstimatedValue : TempUnitEstimatedValueLA);
                        mSingleCalculation144.value = UnitEstimatedValueLA.ToString();
                        mCalcList.Add(mSingleCalculation144);

                        //-Total Building Cost Per Unit 176 "Total Building Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation176 = new CalcParameterOutputModel();
                        mSingleCalculation176.parameterid = "176";
                        mSingleCalculation176.parametername = "Total Building Cost Per Unit";
                        double TempTotalBuildingCostPerUnitLA = TotalBuildingCostPerUnitCB;
                        double TotalBuildingCostPerUnitLA = (mParams.TotalBuildingCostPerUnit != TempTotalBuildingCostPerUnitLA && mParams.TotalBuildingCostPerUnit != 0 ? mParams.TotalBuildingCostPerUnit : TempTotalBuildingCostPerUnitLA);
                        mSingleCalculation176.value = TotalBuildingCostPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation176);

                        //-Total Sales Cost Per Unit 174 "Total Sales Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation174 = new CalcParameterOutputModel();
                        mSingleCalculation174.parameterid = "174";
                        mSingleCalculation174.parametername = "Total Sales Cost Per Unit";
                        double TempTotalSalesCostPerUnitLA = TotalBuildingCostPerUnitCB;
                        double TotalSalesCostPerUnitLA = (mParams.TotalSalesCostPerUnit != TempTotalSalesCostPerUnitLA && mParams.TotalSalesCostPerUnit != 0 ? mParams.TotalSalesCostPerUnit : TempTotalSalesCostPerUnitLA);
                        mSingleCalculation174.value = TotalSalesCostPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation174);

                        // -Desired Profit Percent
                        CalcParameterOutputModel mSingleCalculation181 = new CalcParameterOutputModel();
                        mSingleCalculation181.parameterid = "181";
                        mSingleCalculation181.parametername = "Desired Profit Percent";
                        double TempDesiredProfitPercentLA = mDesiredProfitPercent;
                        double DesiredProfitPercentLA = (mParams.DesiredProfitPercent != TempDesiredProfitPercentLA && mParams.DesiredProfitPercent != 0 ? mParams.DesiredProfitPercent : TempDesiredProfitPercentLA);
                        mSingleCalculation181.value = DesiredProfitPercentLA.ToString();
                        mCalcList.Add(mSingleCalculation181);

                        //-Desired Profit before financing(Desired profit % X Backend Sales Price) 45 "Desired Profit Before Financing"
                        CalcParameterOutputModel mSingleCalculation45 = new CalcParameterOutputModel();
                        mSingleCalculation45.parameterid = "45";
                        mSingleCalculation45.parametername = "Desired Profit Before Financing";
                        double TempDesiredProfitBeforeFinancingLA = DesiredProfitPercentLA * UnitEstimatedValueLA;
                        double DesiredProfitBeforeFinancingLA = (mParams.DesiredProfitBeforeFinancing != TempDesiredProfitBeforeFinancingLA && mParams.DesiredProfitBeforeFinancing != 0 ? mParams.DesiredProfitBeforeFinancing : TempDesiredProfitBeforeFinancingLA);
                        mSingleCalculation45.value = DesiredProfitBeforeFinancingLA.ToString();
                        mCalcList.Add(mSingleCalculation45);

                        //- Value of Land Per Unit(Sum of Step 4 above this line(Red is Negative Green is Positive Number) 150 "Value of Land Per Unit"
                        CalcParameterOutputModel mSingleCalculation150 = new CalcParameterOutputModel();
                        mSingleCalculation150.parameterid = "150";
                        mSingleCalculation150.parametername = "Value of Land Per Unit";
                        double TempValueofLandPerUnitLA = UnitEstimatedValueLA - (TotalBuildingCostPerUnitLA + TotalSalesCostPerUnitLA + DesiredProfitBeforeFinancingLA);
                        double ValueofLandPerUnitLA = (mParams.ValueOfLandPerUnit != TempValueofLandPerUnitLA && mParams.ValueOfLandPerUnit != 0 ? mParams.ValueOfLandPerUnit : TempValueofLandPerUnitLA);
                        mSingleCalculation150.value = ValueofLandPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation150);

                        //- Total Land Value(Value of Land Per Unit X Number of Units) 47 "Total Land Value"
                        CalcParameterOutputModel mSingleCalculation47 = new CalcParameterOutputModel();
                        mSingleCalculation47.parameterid = "47";
                        mSingleCalculation47.parametername = "Total Land Value";
                        double TempTotalLandValueLA = ValueofLandPerUnitLA * NumberofUnits;
                        double TotalLandValueLA = (mParams.TotalLandValue != TempTotalLandValueLA && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValueLA);
                        mSingleCalculation47.value = TotalLandValueLA.ToString();
                        mCalcList.Add(mSingleCalculation47);

                        // Step 5 Financing
                        // PERFORMA PER PROJECT
                        // Total Project Cost* Step 2  
                        CalcParameterOutputModel mSingleCalculation178 = new CalcParameterOutputModel();
                        mSingleCalculation178.parameterid = "178";
                        mSingleCalculation178.parametername = "Total Project Cost";
                        double TempTotalProjectCostFI = TotalProjectCostCB;
                        double TotalProjectCostFI = (mParams.TotalProjectCost != TempTotalProjectCostFI && mParams.TotalProjectCost != 0 ? mParams.TotalProjectCost : TempTotalProjectCostFI);
                        mSingleCalculation178.value = TotalProjectCostFI.ToString();
                        mCalcList.Add(mSingleCalculation178);

                        // Total Land Value* Step 4
                        CalcParameterOutputModel mSingleCalculation179 = new CalcParameterOutputModel();
                        mSingleCalculation179.parameterid = "179";
                        mSingleCalculation179.parametername = "Total Land Value";
                        double TempTotalLandValuePF = TotalLandValueLA;
                        double TotalLandValuePF = (mParams.TotalLandValue != TempTotalLandValuePF && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValuePF);
                        mSingleCalculation179.value = TotalLandValuePF.ToString();
                        mCalcList.Add(mSingleCalculation179);

                        // Equity(Manual Input)
                        CalcParameterOutputModel mSingleCalculation55 = new CalcParameterOutputModel();
                        mSingleCalculation55.parameterid = "55";
                        mSingleCalculation55.parametername = "Equity";
                        double TempEquityPF = mEquity;
                        double EquityPF = (mParams.Equity != TempEquityPF && mParams.Equity != 0 ? mParams.Equity : TempEquityPF);
                        mSingleCalculation55.value = EquityPF.ToString();
                        mCalcList.Add(mSingleCalculation55);

                        // Acquisition Carrying Months(Manual Input)
                        CalcParameterOutputModel mSingleCalculation56 = new CalcParameterOutputModel();
                        mSingleCalculation56.parameterid = "56";
                        mSingleCalculation56.parametername = "Acquisition Carrying Months";
                        double TempAcquisitionCarryingMonthsPF = mAcquisitionCarryingMonths;
                        double AcquisitionCarryingMonthsPF = (mParams.AcquisitionCarryingMonths != TempAcquisitionCarryingMonthsPF && mParams.AcquisitionCarryingMonths != 0 ? mParams.AcquisitionCarryingMonths : TempAcquisitionCarryingMonthsPF);
                        mSingleCalculation56.value = AcquisitionCarryingMonthsPF.ToString();
                        mCalcList.Add(mSingleCalculation56);

                        // Interest Rate Land Acquisition(Manual Input)
                        CalcParameterOutputModel mSingleCalculation57 = new CalcParameterOutputModel();
                        mSingleCalculation57.parameterid = "57";
                        mSingleCalculation57.parametername = "Interest Rate Land Acquisition";
                        double TempInterestRateLandAcquisitionPF = mInterestRateLandAcquisition;
                        double InterestRateLandAcquisitionPF = (mParams.InterestRateLandAcquisition != TempInterestRateLandAcquisitionPF && mParams.InterestRateLandAcquisition != 0 ? mParams.InterestRateLandAcquisition : TempInterestRateLandAcquisitionPF);
                        mSingleCalculation57.value = InterestRateLandAcquisitionPF.ToString();
                        mCalcList.Add(mSingleCalculation57);

                        // Loan Fees Land Acquisition Loan(Manual Input)
                        CalcParameterOutputModel mSingleCalculation81 = new CalcParameterOutputModel();
                        mSingleCalculation81.parameterid = "81";
                        mSingleCalculation81.parametername = "Loan Fees Land Acquisition Loan";
                        double TempLoanFeesLandAcquisitionLoanPF = mLoanFeesLandAcquisitionLoan;
                        double LoanFeesLandAcquisitionLoanPF = (mParams.LoanFeesLandAcquisitionLoan != TempLoanFeesLandAcquisitionLoanPF && mParams.LoanFeesLandAcquisitionLoan != 0 ? mParams.LoanFeesLandAcquisitionLoan : TempLoanFeesLandAcquisitionLoanPF);
                        mSingleCalculation81.value = LoanFeesLandAcquisitionLoanPF.ToString();
                        mCalcList.Add(mSingleCalculation81);

                        // Total Financing Cost Land Acquisition((Land Value – Equity) * (Interest Rate Land Acquisition * (Acquisition Carrying Months/ 12) +(Land Value – Equity)*(Loan Fees Land Acquisition Loan)
                        CalcParameterOutputModel mSingleCalculation58 = new CalcParameterOutputModel();
                        mSingleCalculation58.parameterid = "58";
                        mSingleCalculation58.parametername = "Total Financing Cost Land Acquisition";
                        double TempTotalFinancingCostLandAcquisitionPF = (TotalLandValuePF - EquityPF) * (InterestRateLandAcquisitionPF * (AcquisitionCarryingMonthsPF / 12) + (TotalLandValuePF - EquityPF) * (LoanFeesLandAcquisitionLoanPF));
                        double TotalFinancingCostLandAcquisitionPF = (mParams.TotalFinancingCostLandAcquisition != TempTotalFinancingCostLandAcquisitionPF && mParams.TotalFinancingCostLandAcquisition != 0 ? mParams.TotalFinancingCostLandAcquisition : TempTotalFinancingCostLandAcquisitionPF);
                        mSingleCalculation58.value = TotalFinancingCostLandAcquisitionPF.ToString();
                        mCalcList.Add(mSingleCalculation58);

                        // Interest Rate Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation59 = new CalcParameterOutputModel();
                        mSingleCalculation59.parameterid = "59";
                        mSingleCalculation59.parametername = "Interest Rate Construction Loan";
                        double TempInterestRateConstructionLoan = mInterestRateConstructionLoan;
                        double InterestRateConstructionLoan = (mParams.InterestRateConstructionLoan != TempInterestRateConstructionLoan && mParams.InterestRateConstructionLoan != 0 ? mParams.InterestRateConstructionLoan : TempInterestRateConstructionLoan);
                        mSingleCalculation59.value = InterestRateConstructionLoan.ToString();
                        mCalcList.Add(mSingleCalculation59);

                        // Loan Fees Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation60 = new CalcParameterOutputModel();
                        mSingleCalculation60.parameterid = "60";
                        mSingleCalculation60.parametername = "Loan Fees Construction Loan";
                        double TempLoanFeesConstructionLoanPF = mLoanFeesConstructionLoan;
                        double LoanFeesConstructionLoanPF = (mParams.LoanFeesConstructionLoan != TempLoanFeesConstructionLoanPF && mParams.LoanFeesConstructionLoan != 0 ? mParams.LoanFeesConstructionLoan : TempLoanFeesConstructionLoanPF);
                        mSingleCalculation60.value = LoanFeesConstructionLoanPF.ToString();
                        mCalcList.Add(mSingleCalculation60);

                        // Construction Loan Months(Manual Default)
                        CalcParameterOutputModel mSingleCalculation61 = new CalcParameterOutputModel();
                        mSingleCalculation61.parameterid = "61";
                        mSingleCalculation61.parametername = "Construction Loan Months";
                        double TempConstructionLoanMonthsFI = mConstructionLoanMonths;
                        double ConstructionLoanMonthsFI = (mParams.ConstructionLoanMonths != TempConstructionLoanMonthsFI && mParams.ConstructionLoanMonths != 0 ? mParams.ConstructionLoanMonths : TempConstructionLoanMonthsFI);
                        mSingleCalculation61.value = ConstructionLoanMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation61);

                        // Total Financing Construction Cost(((Land Value -Equity) +(Total Project Cost/ 2) *((Interest Rate Construction Loan”) *(Construction Loan Months/ 12)) +((Land Value – Equity) *(Loan Fees Construction Loan”)))
                        CalcParameterOutputModel mSingleCalculation62 = new CalcParameterOutputModel();
                        mSingleCalculation62.parameterid = "62";
                        mSingleCalculation62.parametername = "Total Financing Cost Construction";
                        double TempTotalFinancingCostConstructionFI = ((TotalLandValuePF - EquityPF) + (TotalProjectCostFI / 2) * ((InterestRateConstructionLoan) * (ConstructionLoanMonthsFI / 12)) + ((TotalLandValuePF - EquityPF) * (LoanFeesConstructionLoanPF)));
                        double TotalFinancingCostConstructionFI = (mParams.TotalFinancingCostConstruction != TempTotalFinancingCostConstructionFI && mParams.TotalFinancingCostConstruction != 0 ? mParams.TotalFinancingCostConstruction : TempTotalFinancingCostConstructionFI);
                        mSingleCalculation62.value = TotalFinancingCostConstructionFI.ToString();
                        mCalcList.Add(mSingleCalculation62);

                        // Months to Sell after completion(Manual Default)
                        CalcParameterOutputModel mSingleCalculation64 = new CalcParameterOutputModel();
                        mSingleCalculation64.parameterid = "64";
                        mSingleCalculation64.parametername = "Months to Sell After completion";
                        double TempMonthstoSellAfterCompletionPF = mMonthsToSellAfterCompletion;
                        double MonthstoSellAfterCompletionPF = (mParams.MonthsToSellAfterCompletion != TempMonthstoSellAfterCompletionPF && mParams.MonthsToSellAfterCompletion != 0 ? mParams.MonthsToSellAfterCompletion : TempMonthstoSellAfterCompletionPF);
                        mSingleCalculation64.value = MonthstoSellAfterCompletionPF.ToString();
                        mCalcList.Add(mSingleCalculation64);

                        // Extra Financing After Completion(Land Value +Total Project Cost – Contributed Equity) *(Interest Rate Construction Loan * (Months to Sell after completion Months / 12)  
                        CalcParameterOutputModel mSingleCalculation65 = new CalcParameterOutputModel();
                        mSingleCalculation65.parameterid = "65";
                        mSingleCalculation65.parametername = "Extra Financing After Completion";
                        double TempExtraFinancingAfterCompletionPF = (TotalLandValuePF + TotalProjectCostCB - EquityPF) * (InterestRateConstructionLoan * (MonthstoSellAfterCompletionPF / 12));
                        double ExtraFinancingAfterCompletionPF = (mParams.ExtraFinancingAfterCompletion != TempExtraFinancingAfterCompletionPF && mParams.ExtraFinancingAfterCompletion != 0 ? mParams.ExtraFinancingAfterCompletion : TempExtraFinancingAfterCompletionPF);
                        mSingleCalculation65.value = ExtraFinancingAfterCompletionPF.ToString();
                        mCalcList.Add(mSingleCalculation65);

                        // Total Financing Fees(Sum of Green Text)
                        CalcParameterOutputModel mSingleCalculation66 = new CalcParameterOutputModel();
                        mSingleCalculation66.parameterid = "66";
                        mSingleCalculation66.parametername = "Total Financing Fees";
                        double TempTotalFinancingFeesFI = TotalFinancingCostLandAcquisitionPF + TotalFinancingCostConstructionFI + ExtraFinancingAfterCompletionPF;
                        double TotalFinancingFeesFI = (mParams.TotalFinancingFees != TempTotalFinancingFeesFI && mParams.TotalFinancingFees != 0 ? mParams.TotalFinancingFees : TempTotalFinancingFeesFI);
                        mSingleCalculation66.value = TotalFinancingFeesFI.ToString();
                        mCalcList.Add(mSingleCalculation66);

                        //Step 6 (Proformas & Financing)
                        //PEFORMA PER CONDO
                        //- Estimated Unit Value
                        CalcParameterOutputModel mSingleCalculation143 = new CalcParameterOutputModel();
                        mSingleCalculation143.parameterid = "143";
                        mSingleCalculation143.parametername = "Unit Estimated Value";
                        double TempUnitEstimatedValueFI = UnitEstimatedValueLA;
                        double UnitEstimatedValueFI = (mParams.UnitEstimatedValue != TempUnitEstimatedValueFI && mParams.UnitEstimatedValue != 0 ? mParams.UnitEstimatedValue : TempUnitEstimatedValueFI);
                        mSingleCalculation143.value = UnitEstimatedValueFI.ToString();
                        mCalcList.Add(mSingleCalculation143);

                        //-Total Building Cost Per Unit 125 "Total Building Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation125 = new CalcParameterOutputModel();
                        mSingleCalculation125.parameterid = "125";
                        mSingleCalculation125.parametername = "Total Building Cost Per Unit";
                        double TempTotalBuildingCostPerUnitFI = TotalBuildingCostPerUnitLA;
                        double TotalBuildingCostPerUnitFI = (mParams.TotalBuildingCostPerUnit != TempTotalBuildingCostPerUnitFI && mParams.TotalBuildingCostPerUnit != 0 ? mParams.TotalBuildingCostPerUnit : TempTotalBuildingCostPerUnitFI);
                        mSingleCalculation125.value = TotalBuildingCostPerUnitFI.ToString();
                        mCalcList.Add(mSingleCalculation125);

                        //-Total Sales Cost Per Unit  139 "Total Sales Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation139 = new CalcParameterOutputModel();
                        mSingleCalculation139.parameterid = "139";
                        mSingleCalculation139.parametername = "Total Sales Cost Per Unit";
                        double TempTotalSalesCostPerUnitPF = TotalSalesCostPerUnitLA;
                        double TotalSalesCostPerUnitPF = (mParams.TotalSalesCostPerUnit != TempTotalSalesCostPerUnitPF && mParams.TotalSalesCostPerUnit != 0 ? mParams.TotalSalesCostPerUnit : TempTotalSalesCostPerUnitPF);
                        mSingleCalculation139.value = TotalSalesCostPerUnitPF.ToString();
                        mCalcList.Add(mSingleCalculation139);

                        //-Value of Land Per Condo 151 "Value of Land Per Unit"
                        CalcParameterOutputModel mSingleCalculation151 = new CalcParameterOutputModel();
                        mSingleCalculation151.parameterid = "151";
                        mSingleCalculation151.parametername = "Value of Land Per Unit";
                        double TempValueofLandPerLotFI = ValueofLandPerUnitLA;
                        double ValueofLandPerLotFI = (mParams.ValueOfLandPerLot != TempValueofLandPerLotFI && mParams.ValueOfLandPerLot != 0 ? mParams.ValueOfLandPerLot : TempValueofLandPerLotFI);
                        mSingleCalculation151.value = ValueofLandPerLotFI.ToString();
                        mCalcList.Add(mSingleCalculation151);

                        //-Profit per unit before Financing(Sum of Step 5 above this line(Red is Negative Green is Positive Number for per home) 154 "Profit Per Unit before Financing"
                        CalcParameterOutputModel mSingleCalculation154 = new CalcParameterOutputModel();
                        mSingleCalculation154.parameterid = "154";
                        mSingleCalculation154.parametername = "Profit per unit before Financing";
                        double TempProfitperunitbeforeFinancingPF = UnitEstimatedValueFI - (TotalBuildingCostPerUnitFI + TotalSalesCostPerUnitPF + ValueofLandPerLotFI);
                        double ProfitperunitbeforeFinancingPF = (mParams.ProfitPerUnitBeforeFinancing != TempProfitperunitbeforeFinancingPF && mParams.ProfitPerUnitBeforeFinancing != 0 ? mParams.ProfitPerUnitBeforeFinancing : TempProfitperunitbeforeFinancingPF);
                        mSingleCalculation154.value = ProfitperunitbeforeFinancingPF.ToString();
                        mCalcList.Add(mSingleCalculation154);

                        //-Financing Costs(Total financing Cost / Number of Condos Homes) 183 "Total Financing Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation183 = new CalcParameterOutputModel();
                        mSingleCalculation183.parameterid = "183";
                        mSingleCalculation183.parametername = "Total Financing Cost Per Unit";
                        double TempTotalFinancingCostPerUnitPF = UnitEstimatedValueFI - (TotalBuildingCostPerUnitFI + TotalSalesCostPerUnitPF + ValueofLandPerLotFI);
                        double TotalFinancingCostPerUnitPF = (mParams.TotalFinancingCostPerUnit != TempTotalFinancingCostPerUnitPF && mParams.TotalFinancingCostPerUnit != 0 ? mParams.TotalFinancingCostPerUnit : TempTotalFinancingCostPerUnitPF);
                        mSingleCalculation183.value = ProfitperunitbeforeFinancingPF.ToString();
                        mCalcList.Add(mSingleCalculation183);

                        //- Net Profit Per Unit(Profit per unit before Financing - Financing Costs) 158 "Net Profit Per Unit"
                        CalcParameterOutputModel mSingleCalculation158 = new CalcParameterOutputModel();
                        mSingleCalculation158.parameterid = "158";
                        mSingleCalculation158.parametername = "Net Profit Per Unit";
                        double TempNetProfitPerUnitPF = ProfitperunitbeforeFinancingPF - TotalFinancingCostPerUnitPF;
                        double NetProfitPerUnitPF = (mParams.NetProfitPerUnit != TempNetProfitPerUnitPF && mParams.NetProfitPerUnit != 0 ? mParams.NetProfitPerUnit : TempNetProfitPerUnitPF);
                        mSingleCalculation158.value = NetProfitPerUnitPF.ToString();
                        mCalcList.Add(mSingleCalculation158);

                        // PERFORMA PER PROJECT
                        //- Total Backend Sales Price(Backend Sales Price X Number of Condo) 166 "Total Backend Sales Price"
                        CalcParameterOutputModel mSingleCalculation166 = new CalcParameterOutputModel();
                        mSingleCalculation166.parameterid = "166";
                        mSingleCalculation166.parametername = "Total Backend Sales Price";
                        double TempTotalBackendSalesPricePF = UnitEstimatedValueFI * NumberofUnits;
                        double TotalBackendSalesPricePF = (mParams.BackendSalesPrice != TempTotalBackendSalesPricePF && mParams.BackendSalesPrice != 0 ? mParams.BackendSalesPrice : TempTotalBackendSalesPricePF);
                        mSingleCalculation166.value = TotalBackendSalesPricePF.ToString();
                        mCalcList.Add(mSingleCalculation166);

                        //- Total Project Cost *See step 2 182 "Total Project Cost"
                        CalcParameterOutputModel mSingleCalculation182 = new CalcParameterOutputModel();
                        mSingleCalculation182.parameterid = "182";
                        mSingleCalculation182.parametername = "Total Project Cost";
                        double TempTotalProjectCostPF = TotalProjectCostCB;
                        double TotalProjectCostPF = (mParams.TotalProjectCost != TempTotalProjectCostPF && mParams.TotalProjectCost != 0 ? mParams.TotalProjectCost : TempTotalProjectCostPF);
                        mSingleCalculation182.value = TotalProjectCostPF.ToString();
                        mCalcList.Add(mSingleCalculation182);

                        //- Total Sales Cost Per Project *See step 3 141 "Total Sales Cost Per Project"
                        CalcParameterOutputModel mSingleCalculation141 = new CalcParameterOutputModel();
                        mSingleCalculation141.parameterid = "141";
                        mSingleCalculation141.parametername = "Total Sales Cost Per Project";
                        double TempTotalSalesCostPerProjectPF = TotalSalesCostPerProjectSCB;
                        double TotalSalesCostPerProjectPF = (mParams.TotalSalesCostPerProject != TempTotalSalesCostPerProjectPF && mParams.TotalSalesCostPerProject != 0 ? mParams.TotalSalesCostPerProject : TempTotalSalesCostPerProjectPF);
                        mSingleCalculation141.value = TotalSalesCostPerProjectPF.ToString();
                        mCalcList.Add(mSingleCalculation141);

                        //- Total Land Value *See step 4 123 "Total Land Value"
                        CalcParameterOutputModel mSingleCalculation123 = new CalcParameterOutputModel();
                        mSingleCalculation123.parameterid = "123";
                        mSingleCalculation123.parametername = "Total Land Value";
                        double TempTotalLandValuePFP = TotalLandValuePF;
                        double TotalLandValuePFP = (mParams.TotalLandValue != TempTotalLandValuePFP && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValuePFP);
                        mSingleCalculation123.value = TotalLandValuePFP.ToString();
                        mCalcList.Add(mSingleCalculation123);

                        //- Profit For Project before financing(Sum of Step 5 above this line(Red is Negative Green is Positive Number for project) 155 "Profit For Project before Financing"
                        CalcParameterOutputModel mSingleCalculation155 = new CalcParameterOutputModel();
                        mSingleCalculation155.parameterid = "155";
                        mSingleCalculation155.parametername = "Profit For Project before Financing";
                        double TempProfitForProjectbeforeFinancingPFP = TotalBackendSalesPricePF - (TotalProjectCostPF + TotalSalesCostPerProjectPF + TotalLandValuePFP);
                        double ProfitForProjectbeforeFinancingPFP = (mParams.ProfitForProjectBeforeFinancing != TempProfitForProjectbeforeFinancingPFP && mParams.ProfitForProjectBeforeFinancing != 0 ? mParams.ProfitForProjectBeforeFinancing : TempProfitForProjectbeforeFinancingPFP);
                        mSingleCalculation155.value = ProfitForProjectbeforeFinancingPFP.ToString();
                        mCalcList.Add(mSingleCalculation155);

                        //-Total Financing Costs*See Step 6 162 "Total Financing Cost"
                        CalcParameterOutputModel mSingleCalculation162 = new CalcParameterOutputModel();
                        mSingleCalculation162.parameterid = "162";
                        mSingleCalculation162.parametername = "Total Financing Cost";
                        double TempTotalFinancingCostPFP = TotalFinancingFeesFI;
                        double TotalFinancingCostPFP = (mParams.TotalFinancingFees != TempTotalFinancingCostPFP && mParams.TotalFinancingFees != 0 ? mParams.TotalFinancingFees : TempTotalFinancingCostPFP);
                        mSingleCalculation162.value = TotalFinancingCostPFP.ToString();
                        mCalcList.Add(mSingleCalculation162);

                        //- Net Profit For Project(Profit For Project before financing - Total Financing Costs) 159 "Net Profit For Project" 
                        CalcParameterOutputModel mSingleCalculation159 = new CalcParameterOutputModel();
                        mSingleCalculation159.parameterid = "159";
                        mSingleCalculation159.parametername = "Net Profit For Project";
                        double TempNetProfitForProjectPFP = ProfitForProjectbeforeFinancingPFP - TotalFinancingCostPFP;
                        double NetProfitForProjectPFP = (mParams.NetProfitForProject != TempNetProfitForProjectPFP && mParams.NetProfitForProject != 0 ? mParams.NetProfitForProject : TempNetProfitForProjectPFP);
                        mSingleCalculation159.value = NetProfitForProjectPFP.ToString();
                        mCalcList.Add(mSingleCalculation159);
                    }

                    else if (mParams.StructureType == "Apartment")
                    {
                        // Step 1
                        // - Livable Sqft (MAX FAR) – ((Inefficiency Space Percentage X MAX FAR))
                        CalcParameterOutputModel mSingleCalculation71 = new CalcParameterOutputModel();
                        mSingleCalculation71.parameterid = "71";
                        mSingleCalculation71.parametername = "Livable Sqft";
                        double TempLivableSqft = mInefficientSpacePercentage * Convert.ToDouble(FarMax);
                        double LivableSqft = (mParams.LivableSqft != TempLivableSqft && mParams.LivableSqft != 0 ? mParams.LivableSqft : TempLivableSqft);
                        mSingleCalculation71.value = LivableSqft.ToString();
                        mCalcList.Add(mSingleCalculation71);

                        // -Number of Units (Lot Size/Minimum Lot size) rounded down to the nearest whole number
                        CalcParameterOutputModel mSingleCalculation80 = new CalcParameterOutputModel();
                        mSingleCalculation80.parameterid = "80";
                        mSingleCalculation80.parametername = "Number Of Units";
                        double TempNumberofUnits = mParams.LotSize / Convert.ToDouble(mDig);
                        double NumberofUnits = (mParams.NumberOfUnits != TempNumberofUnits && mParams.NumberOfUnits != 0 ? mParams.NumberOfUnits : TempNumberofUnits);
                        mSingleCalculation80.value = NumberofUnits.ToString();
                        mCalcList.Add(mSingleCalculation80);

                        // -Sellable square footage. (Livable Sqft. /Number of Units)
                        CalcParameterOutputModel mSingleCalculation67 = new CalcParameterOutputModel();
                        mSingleCalculation67.parameterid = "67";
                        mSingleCalculation67.parametername = "Sellable Square Footage";
                        double TempSellableSquareFootage = LivableSqft / NumberofUnits;
                        double SellableSquareFootage = (mParams.SellableSqFootage != TempSellableSquareFootage && mParams.SellableSqFootage != 0 ? mParams.SellableSqFootage : TempSellableSquareFootage);
                        mSingleCalculation67.value = SellableSquareFootage.ToString();
                        mCalcList.Add(mSingleCalculation67);


                        // Step 2 (Find out the Construction Costs Breakdown)

                        // - Apartment Base Cost Per Sqft
                        CalcParameterOutputModel mSingleCalculation113 = new CalcParameterOutputModel();
                        mSingleCalculation113.parameterid = "113";
                        mSingleCalculation113.parametername = "Apartment Base Cost Per Sqft";
                        double TempApartmentBaseCostPerSqft = mApartmentBaseCostPerSqft;
                        double ApartmentBaseCostPerSqft = (mParams.ApartmentBaseCostPerSqft != TempApartmentBaseCostPerSqft && mParams.ApartmentBaseCostPerSqft != 0 ? mParams.ApartmentBaseCostPerSqft : TempApartmentBaseCostPerSqft);
                        mSingleCalculation113.value = ApartmentBaseCostPerSqft.ToString();
                        mCalcList.Add(mSingleCalculation113);

                        // - Apartment Base Cost (Apartment Base Cost Per Sqft. X Sellable Square Footage)
                        CalcParameterOutputModel mSingleCalculation102 = new CalcParameterOutputModel();
                        mSingleCalculation102.parameterid = "102";
                        mSingleCalculation102.parametername = "Apartment Base Cost";
                        double TempApartmentBaseCost = ApartmentBaseCostPerSqft * SellableSquareFootage;
                        double ApartmentBaseCost = (mParams.ApartmentBaseCost != TempApartmentBaseCost && mParams.ApartmentBaseCost != 0 ? mParams.ApartmentBaseCost : TempApartmentBaseCost);
                        mSingleCalculation102.value = ApartmentBaseCost.ToString();
                        mCalcList.Add(mSingleCalculation102);

                        // -MHA Cost (MHA Fee Table Price X MAX FAR/ Number of Units)
                        CalcParameterOutputModel mSingleCalculation121 = new CalcParameterOutputModel();
                        mSingleCalculation121.parameterid = "121";
                        mSingleCalculation121.parametername = "MHA Cost";
                        double TempMhaCost = Convert.ToDouble(mHaFee) * Convert.ToDouble(FarMax) / NumberofUnits;
                        double MhaCost = (mParams.MhaCost != TempMhaCost && mParams.MhaCost != 0 ? mParams.MhaCost : TempMhaCost);
                        mSingleCalculation121.value = MhaCost.ToString();
                        mCalcList.Add(mSingleCalculation121);

                        // Total Building Cost Per Unit(Sum of ALL RED TEXT)
                        CalcParameterOutputModel mSingleCalculation126 = new CalcParameterOutputModel();
                        mSingleCalculation126.parameterid = "126";
                        mSingleCalculation126.parametername = "Total Building Cost Per Unit";
                        double TempTotalBuildingCostPerUnitCB = EcaCost + ElevationGradeCost + WaterLineCost + SewerCost + StormDrainCost + AlleyCost + UndergroundParkingCost + MhaCost;
                        double TotalBuildingCostPerUnitCB = (mParams.TotalBuildingCostPerUnit != TempTotalBuildingCostPerUnitCB && mParams.TotalBuildingCostPerUnit != 0 ? mParams.TotalBuildingCostPerUnit : TempTotalBuildingCostPerUnitCB);
                        mSingleCalculation126.value = TotalBuildingCostPerUnitCB.ToString();
                        mCalcList.Add(mSingleCalculation126);

                        // Total Project Cost(Total Building Cost Per Unit X Number of Units)
                        CalcParameterOutputModel mSingleCalculation108 = new CalcParameterOutputModel();
                        mSingleCalculation108.parameterid = "108";
                        mSingleCalculation108.parametername = "Total Project Cost";
                        double TempTotalProjectCostCB = TotalBuildingCostPerUnitCB * NumberofUnits;
                        double TotalProjectCostCB = (mParams.TotalProjectCost != TempTotalProjectCostCB && mParams.TotalProjectCost != 0 ? mParams.TotalProjectCost : TempTotalProjectCostCB);
                        mSingleCalculation108.value = TotalProjectCostCB.ToString();
                        mCalcList.Add(mSingleCalculation108);

                        // Step 3 (Sales Cost Breakdown)
                        // -Apartment Cap Rate(Default 3 %)
                        CalcParameterOutputModel mSingleCalculation76 = new CalcParameterOutputModel();
                        mSingleCalculation76.parameterid = "76";
                        mSingleCalculation76.parametername = "Apartment Cap Rate";
                        double TempApartmentCapRateSCB = mApartmentCapRate;
                        double ApartmentCapRateSCB = (mParams.ApartmentCapRate != TempApartmentCapRateSCB && mParams.ApartmentCapRate != 0 ? mParams.ApartmentCapRate : TempApartmentCapRateSCB);
                        mSingleCalculation76.value = ApartmentCapRateSCB.ToString();
                        mCalcList.Add(mSingleCalculation76);

                        // -Monthly Rent Per Unit(Manual Input) Monthly Rent Per Unit
                        CalcParameterOutputModel mSingleCalculation72 = new CalcParameterOutputModel();
                        mSingleCalculation72.parameterid = "72";
                        mSingleCalculation72.parametername = "Monthly Rent Per Unit";
                        double TempMonthlyRentPerUnitSCB = mMonthlyRentPerUnit;
                        double MonthlyRentPerUnitSCB = (mParams.MonthlyRentPerUnit != TempMonthlyRentPerUnitSCB && mParams.MonthlyRentPerUnit != 0 ? mParams.MonthlyRentPerUnit : TempMonthlyRentPerUnitSCB);
                        mSingleCalculation72.value = MonthlyRentPerUnitSCB.ToString();
                        mCalcList.Add(mSingleCalculation72);

                        // -Annual Rent(Average Rent Per Unit X 12)
                        CalcParameterOutputModel mSingleCalculation73 = new CalcParameterOutputModel();
                        mSingleCalculation73.parameterid = "73";
                        mSingleCalculation73.parametername = "Annual Rent";
                        double TempAnnualRentSCB = mMonthlyRentPerUnit * 12;
                        double AnnualRentSCB = (mParams.AnnualRent != TempAnnualRentSCB && mParams.AnnualRent != 0 ? mParams.AnnualRent : TempAnnualRentSCB);
                        mSingleCalculation73.value = AnnualRentSCB.ToString();
                        mCalcList.Add(mSingleCalculation73);

                        // -Vacancy Rate(Default to 5 %)
                        CalcParameterOutputModel mSingleCalculation74 = new CalcParameterOutputModel();
                        mSingleCalculation74.parameterid = "74";
                        mSingleCalculation74.parametername = "Vacancy Rate";
                        double TempVacancyRate = mVacancyRate;
                        double VacancyRate = (mParams.VacancyRate != TempVacancyRate && mParams.VacancyRate != 0 ? mParams.VacancyRate : TempVacancyRate);
                        mSingleCalculation74.value = VacancyRate.ToString();
                        mCalcList.Add(mSingleCalculation74);

                        // -Total Vacancy Allowance(Vacancy Rate X Annual Rent)
                        CalcParameterOutputModel mSingleCalculation131 = new CalcParameterOutputModel();
                        mSingleCalculation131.parameterid = "131";
                        mSingleCalculation131.parametername = "Total Vacancy Allowance";
                        double TempTotalVacancyAllowance = AnnualRentSCB * VacancyRate;
                        double TotalVacancyAllowance = (mParams.TotalVacancyAllowance != TempTotalVacancyAllowance && mParams.TotalVacancyAllowance != 0 ? mParams.TotalVacancyAllowance : TempTotalVacancyAllowance);
                        mSingleCalculation131.value = TotalVacancyAllowance.ToString();
                        mCalcList.Add(mSingleCalculation131);

                        // -Apartment Gross Income(Annual Rent – Total Vacancy Allowance)
                        CalcParameterOutputModel mSingleCalculation132 = new CalcParameterOutputModel();
                        mSingleCalculation132.parameterid = "73";
                        mSingleCalculation132.parametername = "Apartment Gross Income";
                        double TempApartmentGrossIncome = AnnualRentSCB - TotalVacancyAllowance;
                        double ApartmentGrossIncome = (mParams.ApartmentGrossIncome != TempApartmentGrossIncome && mParams.ApartmentGrossIncome != 0 ? mParams.ApartmentGrossIncome : TempApartmentGrossIncome);
                        mSingleCalculation132.value = ApartmentGrossIncome.ToString();
                        mCalcList.Add(mSingleCalculation132);

                        // -Operating Expense per sqft(Default $10)
                        CalcParameterOutputModel mSingleCalculation77 = new CalcParameterOutputModel();
                        mSingleCalculation77.parameterid = "77";
                        mSingleCalculation77.parametername = "Operating Expenses Per Foot";
                        double TempOperatingExpensesPerFoot = mOperatingExpensesPerFoot;
                        double OperatingExpensesPerFoot = (mParams.OperatingExpensesPerFt != TempOperatingExpensesPerFoot && mParams.OperatingExpensesPerFt != 0 ? mParams.OperatingExpensesPerFt : TempOperatingExpensesPerFoot);
                        mSingleCalculation77.value = OperatingExpensesPerFoot.ToString();
                        mCalcList.Add(mSingleCalculation77);

                        // -Total Operating Expense(Operating Expense per sqft. XMax FAR) *Step 1
                        CalcParameterOutputModel mSingleCalculation78 = new CalcParameterOutputModel();
                        mSingleCalculation78.parameterid = "78";
                        mSingleCalculation78.parametername = "Operating Expenses";
                        double TempOperatingExpenses = mOperatingExpensesPerFoot;
                        double OperatingExpenses = (mParams.TotalOperatingExpenses != TempOperatingExpenses && mParams.TotalOperatingExpenses != 0 ? mParams.TotalOperatingExpenses : TempOperatingExpenses);
                        mSingleCalculation78.value = OperatingExpenses.ToString();
                        mCalcList.Add(mSingleCalculation78);

                        // -Yearly Apartment Taxes(Default to $50,000)
                        CalcParameterOutputModel mSingleCalculation75 = new CalcParameterOutputModel();
                        mSingleCalculation75.parameterid = "75";
                        mSingleCalculation75.parametername = "Yearly Apartment Taxes";
                        double TempYearlyApartmentTaxes = mYearlyApartmentTaxes;
                        double YearlyApartmentTaxes = (mParams.YearlyApartmentTaxes != TempYearlyApartmentTaxes && mParams.YearlyApartmentTaxes != 0 ? mParams.YearlyApartmentTaxes : TempYearlyApartmentTaxes);
                        mSingleCalculation75.value = YearlyApartmentTaxes.ToString();
                        mCalcList.Add(mSingleCalculation75);

                        // -Net Income(Step 3 Green text minus Red text)
                        CalcParameterOutputModel mSingleCalculation152 = new CalcParameterOutputModel();
                        mSingleCalculation152.parameterid = "152";
                        mSingleCalculation152.parametername = "Net Income";
                        double TempNetIncome = ApartmentGrossIncome - (OperatingExpenses + YearlyApartmentTaxes);
                        double NetIncome = (mParams.NetIncome != TempNetIncome && mParams.NetIncome != 0 ? mParams.NetIncome : TempNetIncome);
                        mSingleCalculation152.value = NetIncome.ToString();
                        mCalcList.Add(mSingleCalculation152);

                        // -Apartment Estimated Value(Net Income/ Apartment Cap Rate)
                        CalcParameterOutputModel mSingleCalculation69 = new CalcParameterOutputModel();
                        mSingleCalculation69.parameterid = "69";
                        mSingleCalculation69.parametername = "Apartment Estimated Value";
                        double TempApartmentEstimatedValue = NetIncome / ApartmentCapRateSCB;
                        double ApartmentEstimatedValue = (mParams.ApartmentEstimatedValue != TempApartmentEstimatedValue && mParams.ApartmentEstimatedValue != 0 ? mParams.ApartmentEstimatedValue : TempApartmentEstimatedValue);
                        mSingleCalculation69.value = ApartmentEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation69);

                        // -Unit Estimated Value(Apartment Estimated Value / Number of Units)
                        CalcParameterOutputModel mSingleCalculation142 = new CalcParameterOutputModel();
                        mSingleCalculation142.parameterid = "142";
                        mSingleCalculation142.parametername = "Unit Estimated Value";
                        double TempUnitEstimatedValue = ApartmentEstimatedValue / NumberofUnits;
                        double UnitEstimatedValue = (mParams.UnitEstimatedValue != TempUnitEstimatedValue && mParams.UnitEstimatedValue != 0 ? mParams.UnitEstimatedValue : TempUnitEstimatedValue);
                        mSingleCalculation142.value = UnitEstimatedValue.ToString();
                        mCalcList.Add(mSingleCalculation142);

                        // -Sales Commission(Sales Commission Table * Apartment Estimated Value / Number of Units)
                        CalcParameterOutputModel mSingleCalculation41 = new CalcParameterOutputModel();
                        mSingleCalculation41.parameterid = "41";
                        mSingleCalculation41.parametername = "Sales Commission";
                        double TempSalesCommission = 0.06 * ApartmentEstimatedValue / NumberofUnits;
                        double SalesCommission = (mParams.SalesCommission != TempSalesCommission && mParams.SalesCommission != 0 ? mParams.SalesCommission : TempSalesCommission);
                        mSingleCalculation41.value = SalesCommission.ToString();
                        mCalcList.Add(mSingleCalculation41);

                        // -Excise Tax (Excise Tax Table* Apartment Estimated Value / Number of Units )
                        CalcParameterOutputModel mSingleCalculation42 = new CalcParameterOutputModel();
                        mSingleCalculation42.parameterid = "42";
                        mSingleCalculation42.parametername = "Excise Tax";
                        double TempExciseTaxSCB = ExciseOutput(ApartmentEstimatedValue) / NumberofUnits;
                        double ExciseTaxSCB = (mParams.ExciseTax != TempExciseTaxSCB && mParams.ExciseTax != 0 ? mParams.ExciseTax : TempExciseTaxSCB);
                        mSingleCalculation42.value = ExciseTaxSCB.ToString();
                        mCalcList.Add(mSingleCalculation42);

                        //- Escrow Fees / Insurance(Escrow and Title Fee table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation43 = new CalcParameterOutputModel();
                        mSingleCalculation43.parameterid = "43";
                        mSingleCalculation43.parametername = "Escrow Fees";
                        double TempEscrowFeesSCB = mFactor4 * ApartmentEstimatedValue / NumberofUnits;
                        double EscrowFeesSCB = (mParams.EscrowFees != TempEscrowFeesSCB && mParams.EscrowFees != 0 ? mParams.EscrowFees : TempEscrowFeesSCB);
                        mSingleCalculation43.value = EscrowFeesSCB.ToString();
                        mCalcList.Add(mSingleCalculation43);

                        //- Marketing / Staging(Marketing and Staging table X Unit Estimated Value)
                        CalcParameterOutputModel mSingleCalculation44 = new CalcParameterOutputModel();
                        mSingleCalculation44.parameterid = "44";
                        mSingleCalculation44.parametername = "Marketing and Staging";
                        double TempMarketingandStagingSCB = mFactor5 * ApartmentEstimatedValue / NumberofUnits;
                        double MarketingandStagingSCB = (mParams.MarketingAndStaging != TempMarketingandStagingSCB && mParams.MarketingAndStaging != 0 ? mParams.MarketingAndStaging : TempMarketingandStagingSCB);
                        mSingleCalculation44.value = MarketingandStagingSCB.ToString();
                        mCalcList.Add(mSingleCalculation44);

                        // -Total Sales Cost per Unit(Sum of all the Blue Above)
                        CalcParameterOutputModel mSingleCalculation138 = new CalcParameterOutputModel();
                        mSingleCalculation138.parameterid = "138";
                        mSingleCalculation138.parametername = "Total Sales Cost Per Unit";
                        double TempTotalSalesCostPerUnitSCB = SalesCommission + ExciseTaxSCB + EscrowFeesSCB + MarketingandStagingSCB;
                        double TotalSalesCostPerUnitSCB = (mParams.TotalSalesCostPerUnit != TempTotalSalesCostPerUnitSCB && mParams.TotalSalesCostPerUnit != 0 ? mParams.TotalSalesCostPerUnit : TempTotalSalesCostPerUnitSCB);
                        mSingleCalculation138.value = TotalSalesCostPerUnitSCB.ToString();
                        mCalcList.Add(mSingleCalculation138);

                        // -Total Sales Cost per Project(Total Sales Cost per Unit X Number of Units)
                        CalcParameterOutputModel mSingleCalculation140 = new CalcParameterOutputModel();
                        mSingleCalculation140.parameterid = "140";
                        mSingleCalculation140.parametername = "Total Sales Cost Per Project";
                        double TempTotalSalesCostPerProjectSCB = TotalBuildingCostPerUnitCB * NumberofUnits;
                        double TotalSalesCostPerProjectSCB = (mParams.TotalSalesCostPerProject != TempTotalSalesCostPerProjectSCB && mParams.TotalSalesCostPerProject != 0 ? mParams.TotalSalesCostPerProject : TempTotalSalesCostPerProjectSCB);
                        mSingleCalculation140.value = TotalSalesCostPerProjectSCB.ToString();
                        mCalcList.Add(mSingleCalculation140);

                        //Step 4 Land Analysis
                        // -Unit Estimated Value
                        CalcParameterOutputModel mSingleCalculation144 = new CalcParameterOutputModel();
                        mSingleCalculation144.parameterid = "144";
                        mSingleCalculation144.parametername = "Unit Estimated Value";
                        double TempUnitEstimatedValueLA = mUnitEstimatedValue;
                        double UnitEstimatedValueLA = (mParams.UnitEstimatedValue != TempUnitEstimatedValueLA && mParams.UnitEstimatedValue != 0 ? mParams.UnitEstimatedValue : TempUnitEstimatedValueLA);
                        mSingleCalculation144.value = UnitEstimatedValueLA.ToString();
                        mCalcList.Add(mSingleCalculation144);

                        //-Total Building Cost Per Unit 176 "Total Building Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation176 = new CalcParameterOutputModel();
                        mSingleCalculation176.parameterid = "176";
                        mSingleCalculation176.parametername = "Total Building Cost Per Unit";
                        double TempTotalBuildingCostPerUnitLA = TotalBuildingCostPerUnitCB;
                        double TotalBuildingCostPerUnitLA = (mParams.TotalBuildingCostPerUnit != TempTotalBuildingCostPerUnitLA && mParams.TotalBuildingCostPerUnit != 0 ? mParams.TotalBuildingCostPerUnit : TempTotalBuildingCostPerUnitLA);
                        mSingleCalculation176.value = TotalBuildingCostPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation176);

                        //-Total Sales Cost Per Unit 174 "Total Sales Cost Per Unit"
                        CalcParameterOutputModel mSingleCalculation174 = new CalcParameterOutputModel();
                        mSingleCalculation174.parameterid = "174";
                        mSingleCalculation174.parametername = "Total Sales Cost Per Unit";
                        double TempTotalSalesCostPerUnitLA = TotalBuildingCostPerUnitCB;
                        double TotalSalesCostPerUnitLA = (mParams.TotalSalesCostPerUnit != TempTotalSalesCostPerUnitLA && mParams.TotalSalesCostPerUnit != 0 ? mParams.TotalSalesCostPerUnit : TempTotalSalesCostPerUnitLA);
                        mSingleCalculation174.value = TotalSalesCostPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation174);

                        // -Desired Profit Percent
                        CalcParameterOutputModel mSingleCalculation181 = new CalcParameterOutputModel();
                        mSingleCalculation181.parameterid = "181";
                        mSingleCalculation181.parametername = "Desired Profit Percent";
                        double TempDesiredProfitPercentLA = mDesiredProfitPercent;
                        double DesiredProfitPercentLA = (mParams.DesiredProfitPercent != TempDesiredProfitPercentLA && mParams.DesiredProfitPercent != 0 ? mParams.DesiredProfitPercent : TempDesiredProfitPercentLA);
                        mSingleCalculation181.value = DesiredProfitPercentLA.ToString();
                        mCalcList.Add(mSingleCalculation181);

                        //-Desired Profit before financing(Desired profit % X Backend Sales Price) 45 "Desired Profit Before Financing"
                        CalcParameterOutputModel mSingleCalculation45 = new CalcParameterOutputModel();
                        mSingleCalculation45.parameterid = "45";
                        mSingleCalculation45.parametername = "Desired Profit Before Financing";
                        double TempDesiredProfitBeforeFinancingLA = DesiredProfitPercentLA * UnitEstimatedValueLA;
                        double DesiredProfitBeforeFinancingLA = (mParams.DesiredProfitBeforeFinancing != TempDesiredProfitBeforeFinancingLA && mParams.Equity != 0 ? mParams.DesiredProfitBeforeFinancing : TempDesiredProfitBeforeFinancingLA);
                        mSingleCalculation45.value = DesiredProfitBeforeFinancingLA.ToString();
                        mCalcList.Add(mSingleCalculation45);

                        //- Value of Land Per Unit(Sum of Step 4 above this line(Red is Negative Green is Positive Number) 150 "Value of Land Per Unit"
                        CalcParameterOutputModel mSingleCalculation150 = new CalcParameterOutputModel();
                        mSingleCalculation150.parameterid = "150";
                        mSingleCalculation150.parametername = "Value of Land Per Unit";
                        double TempValueofLandPerUnitLA = UnitEstimatedValueLA - (TotalBuildingCostPerUnitLA + TotalSalesCostPerUnitLA + DesiredProfitBeforeFinancingLA);
                        double ValueofLandPerUnitLA = (mParams.ValueOfLandPerUnit != TempValueofLandPerUnitLA && mParams.ValueOfLandPerUnit != 0 ? mParams.ValueOfLandPerUnit : TempValueofLandPerUnitLA);
                        mSingleCalculation150.value = ValueofLandPerUnitLA.ToString();
                        mCalcList.Add(mSingleCalculation150);

                        //- Total Land Value(Value of Land Per Unit X Number of Units) 47 "Total Land Value"
                        CalcParameterOutputModel mSingleCalculation47 = new CalcParameterOutputModel();
                        mSingleCalculation47.parameterid = "47";
                        mSingleCalculation47.parametername = "Total Land Value";
                        double TempTotalLandValueLA = ValueofLandPerUnitLA * NumberofUnits;
                        double TotalLandValueLA = (mParams.TotalLandValue != TempTotalLandValueLA && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValueLA);
                        mSingleCalculation47.value = TotalLandValueLA.ToString();
                        mCalcList.Add(mSingleCalculation47);

                        // Step 5 Financing
                        // PERFORMA PER PROJECT
                        // Total Project Cost* Step 2  
                        CalcParameterOutputModel mSingleCalculation178 = new CalcParameterOutputModel();
                        mSingleCalculation178.parameterid = "178";
                        mSingleCalculation178.parametername = "Total Project Cost";
                        double TempTotalProjectCostFI = TotalProjectCostCB;
                        double TotalProjectCostFI = (mParams.TotalProjectCost != TempTotalProjectCostFI && mParams.TotalProjectCost != 0 ? mParams.TotalProjectCost : TempTotalProjectCostFI);
                        mSingleCalculation178.value = TotalProjectCostFI.ToString();
                        mCalcList.Add(mSingleCalculation178);

                        // Total Land Value* Step 4
                        CalcParameterOutputModel mSingleCalculation179 = new CalcParameterOutputModel();
                        mSingleCalculation179.parameterid = "179";
                        mSingleCalculation179.parametername = "Total Land Value";
                        double TempTotalLandValuePF = TotalLandValueLA;
                        double TotalLandValuePF = (mParams.TotalLandValue != TempTotalLandValuePF && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValuePF);
                        mSingleCalculation179.value = TotalLandValuePF.ToString();
                        mCalcList.Add(mSingleCalculation179);

                        // Equity(Manual Input)
                        CalcParameterOutputModel mSingleCalculation55 = new CalcParameterOutputModel();
                        mSingleCalculation55.parameterid = "55";
                        mSingleCalculation55.parametername = "Equity";
                        double TempEquityPF = mEquity;
                        double EquityPF = (mParams.Equity != TempEquityPF && mParams.Equity != 0 ? mParams.Equity : TempEquityPF);
                        mSingleCalculation55.value = EquityPF.ToString();
                        mCalcList.Add(mSingleCalculation55);

                        // Acquisition Carrying Months(Manual Input)
                        CalcParameterOutputModel mSingleCalculation56 = new CalcParameterOutputModel();
                        mSingleCalculation56.parameterid = "56";
                        mSingleCalculation56.parametername = "Acquisition Carrying Months";
                        double TempAcquisitionCarryingMonthsPF = mAcquisitionCarryingMonths;
                        double AcquisitionCarryingMonthsPF = (mParams.AcquisitionCarryingMonths != TempAcquisitionCarryingMonthsPF && mParams.AcquisitionCarryingMonths != 0 ? mParams.AcquisitionCarryingMonths : TempAcquisitionCarryingMonthsPF);
                        mSingleCalculation56.value = AcquisitionCarryingMonthsPF.ToString();
                        mCalcList.Add(mSingleCalculation56);

                        // Interest Rate Land Acquisition(Manual Input)
                        CalcParameterOutputModel mSingleCalculation57 = new CalcParameterOutputModel();
                        mSingleCalculation57.parameterid = "57";
                        mSingleCalculation57.parametername = "Interest Rate Land Acquisition";
                        double TempInterestRateLandAcquisitionPF = mInterestRateLandAcquisition;
                        double InterestRateLandAcquisitionPF = (mParams.InterestRateLandAcquisition != TempInterestRateLandAcquisitionPF && mParams.InterestRateLandAcquisition != 0 ? mParams.InterestRateLandAcquisition : TempInterestRateLandAcquisitionPF);
                        mSingleCalculation57.value = InterestRateLandAcquisitionPF.ToString();
                        mCalcList.Add(mSingleCalculation57);

                        // Loan Fees Land Acquisition Loan(Manual Input)
                        CalcParameterOutputModel mSingleCalculation81 = new CalcParameterOutputModel();
                        mSingleCalculation81.parameterid = "81";
                        mSingleCalculation81.parametername = "Loan Fees Land Acquisition Loan";
                        double TempLoanFeesLandAcquisitionLoanPF = mLoanFeesLandAcquisitionLoan;
                        double LoanFeesLandAcquisitionLoanPF = (mParams.LoanFeesLandAcquisitionLoan != TempLoanFeesLandAcquisitionLoanPF && mParams.LoanFeesLandAcquisitionLoan != 0 ? mParams.LoanFeesLandAcquisitionLoan : TempLoanFeesLandAcquisitionLoanPF);
                        mSingleCalculation81.value = LoanFeesLandAcquisitionLoanPF.ToString();
                        mCalcList.Add(mSingleCalculation81);

                        // Total Financing Cost Land Acquisition((Land Value – Equity) * (Interest Rate Land Acquisition * (Acquisition Carrying Months/ 12) +(Land Value – Equity)*(Loan Fees Land Acquisition Loan)
                        CalcParameterOutputModel mSingleCalculation58 = new CalcParameterOutputModel();
                        mSingleCalculation58.parameterid = "58";
                        mSingleCalculation58.parametername = "Total Financing Cost Land Acquisition";
                        double TempTotalFinancingCostLandAcquisitionPF = (TotalLandValuePF - EquityPF) * (InterestRateLandAcquisitionPF * (AcquisitionCarryingMonthsPF / 12) + (TotalLandValuePF - EquityPF) * (LoanFeesLandAcquisitionLoanPF));
                        double TotalFinancingCostLandAcquisitionPF = (mParams.TotalFinancingCostLandAcquisition != TempTotalFinancingCostLandAcquisitionPF && mParams.TotalFinancingCostLandAcquisition != 0 ? mParams.TotalFinancingCostLandAcquisition : TempTotalFinancingCostLandAcquisitionPF);
                        mSingleCalculation58.value = TotalFinancingCostLandAcquisitionPF.ToString();
                        mCalcList.Add(mSingleCalculation58);

                        // Interest Rate Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation59 = new CalcParameterOutputModel();
                        mSingleCalculation59.parameterid = "59";
                        mSingleCalculation59.parametername = "Interest Rate Construction Loan";
                        double TempInterestRateConstructionLoan = mInterestRateConstructionLoan;
                        double InterestRateConstructionLoan = (mParams.InterestRateConstructionLoan != TempInterestRateConstructionLoan && mParams.InterestRateConstructionLoan != 0 ? mParams.InterestRateConstructionLoan : TempInterestRateConstructionLoan);
                        mSingleCalculation59.value = InterestRateConstructionLoan.ToString();
                        mCalcList.Add(mSingleCalculation59);

                        // Loan Fees Construction Loan(Manual Default)
                        CalcParameterOutputModel mSingleCalculation60 = new CalcParameterOutputModel();
                        mSingleCalculation60.parameterid = "60";
                        mSingleCalculation60.parametername = "Loan Fees Construction Loan";
                        double TempLoanFeesConstructionLoanPF = mLoanFeesConstructionLoan;
                        double LoanFeesConstructionLoanPF = (mParams.LoanFeesConstructionLoan != TempLoanFeesConstructionLoanPF && mParams.LoanFeesConstructionLoan != 0 ? mParams.LoanFeesConstructionLoan : TempLoanFeesConstructionLoanPF);
                        mSingleCalculation60.value = LoanFeesConstructionLoanPF.ToString();
                        mCalcList.Add(mSingleCalculation60);

                        // Construction Loan Months(Manual Default)
                        CalcParameterOutputModel mSingleCalculation61 = new CalcParameterOutputModel();
                        mSingleCalculation61.parameterid = "61";
                        mSingleCalculation61.parametername = "Construction Loan Months";
                        double TempConstructionLoanMonthsFI = mConstructionLoanMonths;
                        double ConstructionLoanMonthsFI = (mParams.ConstructionLoanMonths != TempConstructionLoanMonthsFI && mParams.ConstructionLoanMonths != 0 ? mParams.ConstructionLoanMonths : TempConstructionLoanMonthsFI);
                        mSingleCalculation61.value = ConstructionLoanMonthsFI.ToString();
                        mCalcList.Add(mSingleCalculation61);

                        // Total Financing Construction Cost(((Land Value -Equity) +(Total Project Cost/ 2) *((Interest Rate Construction Loan”) *(Construction Loan Months/ 12)) +((Land Value – Equity) *(Loan Fees Construction Loan”)))
                        CalcParameterOutputModel mSingleCalculation62 = new CalcParameterOutputModel();
                        mSingleCalculation62.parameterid = "62";
                        mSingleCalculation62.parametername = "Total Financing Cost Construction";
                        double TempTotalFinancingCostConstructionFI = ((TotalLandValuePF - EquityPF) + (TotalProjectCostFI / 2) * ((InterestRateConstructionLoan) * (ConstructionLoanMonthsFI / 12)) + ((TotalLandValuePF - EquityPF) * (LoanFeesConstructionLoanPF)));
                        double TotalFinancingCostConstructionFI = (mParams.TotalFinancingCostConstruction != TempTotalFinancingCostConstructionFI && mParams.TotalFinancingCostConstruction != 0 ? mParams.TotalFinancingCostConstruction : TempTotalFinancingCostConstructionFI);
                        mSingleCalculation62.value = TotalFinancingCostConstructionFI.ToString();
                        mCalcList.Add(mSingleCalculation62);

                        // Months to Sell after completion(Manual Default)
                        CalcParameterOutputModel mSingleCalculation64 = new CalcParameterOutputModel();
                        mSingleCalculation64.parameterid = "64";
                        mSingleCalculation64.parametername = "Months to Sell After completion";
                        double TempMonthstoSellAfterCompletionPF = mMonthsToSellAfterCompletion;
                        double MonthstoSellAfterCompletionPF = (mParams.MonthsToSellAfterCompletion != TempMonthstoSellAfterCompletionPF && mParams.MonthsToSellAfterCompletion != 0 ? mParams.MonthsToSellAfterCompletion : TempMonthstoSellAfterCompletionPF);
                        mSingleCalculation64.value = MonthstoSellAfterCompletionPF.ToString();
                        mCalcList.Add(mSingleCalculation64);

                        // Extra Financing After Completion(Land Value +Total Project Cost – Contributed Equity) *(Interest Rate Construction Loan * (Months to Sell after completion Months / 12)  
                        CalcParameterOutputModel mSingleCalculation65 = new CalcParameterOutputModel();
                        mSingleCalculation65.parameterid = "65";
                        mSingleCalculation65.parametername = "Extra Financing After Completion";
                        double TempExtraFinancingAfterCompletionPF = (TotalLandValuePF + TotalProjectCostCB - EquityPF) * (InterestRateConstructionLoan * (MonthstoSellAfterCompletionPF / 12));
                        double ExtraFinancingAfterCompletionPF = (mParams.ExtraFinancingAfterCompletion != TempExtraFinancingAfterCompletionPF && mParams.ExtraFinancingAfterCompletion != 0 ? mParams.ExtraFinancingAfterCompletion : TempExtraFinancingAfterCompletionPF);
                        mSingleCalculation65.value = ExtraFinancingAfterCompletionPF.ToString();
                        mCalcList.Add(mSingleCalculation65);

                        // Total Financing Fees(Sum of Green Text)
                        CalcParameterOutputModel mSingleCalculation66 = new CalcParameterOutputModel();
                        mSingleCalculation66.parameterid = "66";
                        mSingleCalculation66.parametername = "Total Financing Fees";
                        double TempTotalFinancingFeesFI = TotalFinancingCostLandAcquisitionPF + TotalFinancingCostConstructionFI + ExtraFinancingAfterCompletionPF;
                        double TotalFinancingFeesFI = (mParams.TotalFinancingFees != TempTotalFinancingFeesFI && mParams.TotalFinancingFees != 0 ? mParams.TotalFinancingFees : TempTotalFinancingFeesFI);
                        mSingleCalculation66.value = TotalFinancingFeesFI.ToString();
                        mCalcList.Add(mSingleCalculation66);

                        // Step 6
                        // PERFORMA PER PROJECT
                        // -Apartment Value 
                        CalcParameterOutputModel mSingleCalculation160 = new CalcParameterOutputModel();
                        mSingleCalculation160.parameterid = "160";
                        mSingleCalculation160.parametername = "Apartment Estimated Value";
                        double TempApartmentEstimatedValuePF = ApartmentEstimatedValue;
                        double ApartmentEstimatedValuePF = (mParams.ApartmentEstimatedValue != TempApartmentEstimatedValuePF && mParams.ApartmentEstimatedValue != 0 ? mParams.ApartmentEstimatedValue : TempApartmentEstimatedValuePF);
                        mSingleCalculation160.value = ApartmentEstimatedValuePF.ToString();
                        mCalcList.Add(mSingleCalculation160);

                        //- Total Project Cost *See step 2 182 "Total Project Cost"
                        CalcParameterOutputModel mSingleCalculation182 = new CalcParameterOutputModel();
                        mSingleCalculation182.parameterid = "182";
                        mSingleCalculation182.parametername = "Total Project Cost";
                        double TempTotalProjectCostPF = TotalProjectCostCB;
                        double TotalProjectCostPF = (mParams.TotalProjectCost != TempTotalProjectCostPF && mParams.TotalProjectCost != 0 ? mParams.TotalProjectCost : TempTotalProjectCostPF);
                        mSingleCalculation182.value = TotalProjectCostPF.ToString();
                        mCalcList.Add(mSingleCalculation182);

                        //- Total Sales Cost Per Project *See step 3 141 "Total Sales Cost Per Project"
                        CalcParameterOutputModel mSingleCalculation141 = new CalcParameterOutputModel();
                        mSingleCalculation141.parameterid = "141";
                        mSingleCalculation141.parametername = "Total Sales Cost Per Project";
                        double TempTotalSalesCostPerProjectPF = TotalSalesCostPerProjectSCB;
                        double TotalSalesCostPerProjectPF = (mParams.TotalSalesCostPerProject != TempTotalSalesCostPerProjectPF && mParams.TotalSalesCostPerProject != 0 ? mParams.TotalSalesCostPerProject : TempTotalSalesCostPerProjectPF);
                        mSingleCalculation141.value = TotalSalesCostPerProjectPF.ToString();
                        mCalcList.Add(mSingleCalculation141);

                        //- Total Land Value *See step 4 123 "Total Land Value"
                        CalcParameterOutputModel mSingleCalculation123 = new CalcParameterOutputModel();
                        mSingleCalculation123.parameterid = "123";
                        mSingleCalculation123.parametername = "Total Land Value";
                        double TempTotalLandValuePFP = TotalLandValuePF;
                        double TotalLandValuePFP = (mParams.TotalLandValue != TempTotalLandValuePFP && mParams.TotalLandValue != 0 ? mParams.TotalLandValue : TempTotalLandValuePFP);
                        mSingleCalculation123.value = TotalLandValuePFP.ToString();
                        mCalcList.Add(mSingleCalculation123);

                        //- Profit For Project before financing(Sum of Step 5 above this line(Red is Negative Green is Positive Number for project) 155 "Profit For Project before Financing"
                        CalcParameterOutputModel mSingleCalculation155 = new CalcParameterOutputModel();
                        mSingleCalculation155.parameterid = "155";
                        mSingleCalculation155.parametername = "Profit For Project before Financing";
                        double TempProfitForProjectbeforeFinancingPFP = ApartmentEstimatedValuePF - (TotalProjectCostPF + TotalSalesCostPerProjectPF + TotalLandValuePFP);
                        double ProfitForProjectbeforeFinancingPFP = (mParams.ProfitForProjectBeforeFinancing != TempProfitForProjectbeforeFinancingPFP && mParams.ProfitForProjectBeforeFinancing != 0 ? mParams.ProfitForProjectBeforeFinancing : TempProfitForProjectbeforeFinancingPFP);
                        mSingleCalculation155.value = ProfitForProjectbeforeFinancingPFP.ToString();
                        mCalcList.Add(mSingleCalculation155);

                        //-Total Financing Costs*See Step 6 162 "Total Financing Cost"
                        CalcParameterOutputModel mSingleCalculation162 = new CalcParameterOutputModel();
                        mSingleCalculation162.parameterid = "162";
                        mSingleCalculation162.parametername = "Total Financing Cost";
                        double TempTotalFinancingCostPFP = TotalFinancingFeesFI;
                        double TotalFinancingCostPFP = (mParams.TotalFinancingFees != TempTotalFinancingCostPFP && mParams.TotalFinancingFees != 0 ? mParams.TotalFinancingFees : TempTotalFinancingCostPFP);
                        mSingleCalculation162.value = TotalFinancingCostPFP.ToString();
                        mCalcList.Add(mSingleCalculation162);

                        //- Net Profit For Project(Profit For Project before financing - Total Financing Costs) 159 "Net Profit For Project" 
                        CalcParameterOutputModel mSingleCalculation159 = new CalcParameterOutputModel();
                        mSingleCalculation159.parameterid = "159";
                        mSingleCalculation159.parametername = "Net Profit For Project";
                        double TempNetProfitForProjectPFP = ProfitForProjectbeforeFinancingPFP - TotalFinancingCostPFP;
                        double NetProfitForProjectPFP = (mParams.NetProfitForProject != TempNetProfitForProjectPFP && mParams.NetProfitForProject != 0 ? mParams.NetProfitForProject : TempNetProfitForProjectPFP);
                        mSingleCalculation159.value = NetProfitForProjectPFP.ToString();
                        mCalcList.Add(mSingleCalculation159);
                    }

                    // Append all Calculation to list of calculations
                    mCalcOutput.Calculations = mCalcList;
                    mOutModel = mCalcOutput;
                }

                return mOutModel;
            }
        }
    }
}
