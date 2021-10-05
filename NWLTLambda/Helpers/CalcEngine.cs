using NWLTLambda.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;


namespace NWLTLambda
{
    public class CalcEngine
    {
        public List<StructureTypesVM> CalcSFProforma(ParametersList mParams)
        {
            List<StructureTypesVM> mOutModel = new List<StructureTypesVM>();
            mySqlAccess _sqlaccess = new mySqlAccess();

            const double mFactor1 = 1.07;
            const double mFactor2 = 0.06;
            const double mFactor3 = 0.0175;
            const double mFactor4 = 0.0075;
            const double mFactor5 = 0.005;
            const double mFactor6 = 0.25;
            if (mParams.AADU == 0)
            {
                mParams.AADU = 1;
                mParams.AADUSize = 1000;
            }                
            if (mParams.AADUSize == 0)
            {
                mParams.AADUSize = 1000;
            }
            if (mParams.DADU == 0)
            {
                mParams.DADU = 1;
                mParams.DADUSize = 1000;
            }
            if (mParams.DADUSize == 0)
            {
                mParams.DADUSize = 1000;
            }

            // TODO ----   you have to get a LIST OR ARRAY of the StructureType,  There could be an ID associated with them, but you could just use the names LIKE...SFHome, RowHome, Apartment, TownHome
            string[] mStructureType = { "Single Family", "Town Home", "Apartment", "Cottage", "Rowhouse", "Condo" };

            // List of structure types
            foreach (string mStructType in mStructureType)
            {
                List<CalcParameterOutputModel> mCalcList = new List<CalcParameterOutputModel>();
                CalcParameterOutputModel mSingleCalculation = new CalcParameterOutputModel();
                StructureTypesVM mCalcOutput = new StructureTypesVM();
                mCalcOutput.StructureType = mStructType;
                
                //  TODO --- You will have to replace  mParams.StructureType with mStructureType  to run the next 3 calculations
                int BuildCost = _sqlaccess.GetBaseCost(mParams.City, mParams.HighEndArea, mStructType); //Pass in structure type from loop
                int mFAR = _sqlaccess.GetFAR(mParams.City, mParams.Zoning, mStructType, mParams.MHASuffix, mParams.GrowthArea);
                int mHaFee = _sqlaccess.GetMHAFee(mParams.City, mParams.MHaArea, mParams.MHASuffix);
                int mDig = _sqlaccess.GetDensityLimit(mParams.City, mParams.Zoning, mStructType, mParams.MHASuffix, mParams.CornerLot);

                double FarMax = mParams.LotSize * mFAR;

                // Number of Homes
                mSingleCalculation.parameterid = "30";
                mSingleCalculation.parametername = "Number of Homes";
                double NumberOfHomes = Math.Floor(mParams.LotSize / mDig);
                mSingleCalculation.value = NumberOfHomes.ToString();
                mCalcList.Add(mSingleCalculation);
                // Sellable Square Footage
                mSingleCalculation.parameterid = "67";
                mSingleCalculation.parametername = "Sellable Square Footage";
                double SellableSquareFootage = FarMax / NumberOfHomes * mFactor1;
                mSingleCalculation.value = SellableSquareFootage.ToString();
                mCalcList.Add(mSingleCalculation);
                // SF Estimated Value 37
                mSingleCalculation.parameterid = "37";
                mSingleCalculation.parametername = "SF Estimated Value";
                double SFEstimatedValue = mParams.EstimatedValue;
                mSingleCalculation.value = SFEstimatedValue.ToString();
                mCalcList.Add(mSingleCalculation);
                // ADU Estimated Value 38
                mSingleCalculation.parameterid = "38";
                mSingleCalculation.parametername = "ADU Estimated Value";
                double ADUEstimatedValue = mParams.AADU * mParams.AADUSize * 250;
                mSingleCalculation.value = ADUEstimatedValue.ToString();
                mCalcList.Add(mSingleCalculation);
                // DADU Estimated Value 39
                mSingleCalculation.parameterid = "39";
                mSingleCalculation.parametername = "DADU Estimated Value";
                double DADUEstimatedValue = mParams.DADU * mParams.DADUSize * 250;
                mSingleCalculation.value = DADUEstimatedValue.ToString();
                mCalcList.Add(mSingleCalculation);
                // Construction Cost 40
                mSingleCalculation.parameterid = "40";
                mSingleCalculation.parametername = "Construction Cost";
                int ConstructionCost = BuildCost;
                mSingleCalculation.value = BuildCost.ToString();
                mCalcList.Add(mSingleCalculation);
                // Sales Commision 41
                mSingleCalculation.parameterid = "41";
                mSingleCalculation.parametername = "Sales Commission";
                double SalesCommission = (SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue) * mFactor2;
                mSingleCalculation.value = SalesCommission.ToString();
                mCalcList.Add(mSingleCalculation);     
                // Excise tax 42
                mSingleCalculation.parameterid = "42";
                mSingleCalculation.parametername = "Excise Tax";
                double ExciseTax = (SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue) * mFactor3;
                mSingleCalculation.value = ExciseTax.ToString();
                mCalcList.Add(mSingleCalculation);
                // Escrow Fees/Insurance 43
                mSingleCalculation.parameterid = "43";
                mSingleCalculation.parametername = "Escrow Fees/Insurance";
                double Escrow = (SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue) * mFactor4;
                mSingleCalculation.value = Escrow.ToString();
                mCalcList.Add(mSingleCalculation);
                // Marketing/Cleaners 44
                mSingleCalculation.parameterid = "44";
                mSingleCalculation.parametername = "Marketing/Cleaners";
                double Marketing = (SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue) * mFactor5;
                mSingleCalculation.value = Marketing.ToString();
                mCalcList.Add(mSingleCalculation);
                // Profit 45
                mSingleCalculation.parameterid = "45";
                mSingleCalculation.parametername = "Profit";
                double Profit1 = (SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue) * mFactor6;
                mSingleCalculation.value = Profit1.ToString();
                mCalcList.Add(mSingleCalculation);
                // Homes Land Value 46
                mSingleCalculation.parameterid = "46";
                mSingleCalculation.parametername = "Homes Land Value";
                double HomeLandValue = (SFEstimatedValue + ADUEstimatedValue + DADUEstimatedValue) - (ConstructionCost + SalesCommission + ExciseTax + Escrow + Marketing + Profit1);
                mSingleCalculation.value = HomeLandValue.ToString();
                mCalcList.Add(mSingleCalculation);
                // Properties Land Value 47
                mSingleCalculation.parameterid = "47";
                mSingleCalculation.parametername = "Properties Land Value";
                double PropertyLandValue = HomeLandValue * mParams.NumberofHomes;
                mSingleCalculation.value = PropertyLandValue.ToString();
                mCalcList.Add(mSingleCalculation);
                // Total Financing Fees 48  TODO  ----------------
                mSingleCalculation.parameterid = "48";
                mSingleCalculation.parametername = "Total Financing Fees";
                double FinancingFee = 0;
                mSingleCalculation.value = FinancingFee.ToString();
                mCalcList.Add(mSingleCalculation);
                // Financing Fees Per Home 49
                mSingleCalculation.parameterid = "49";
                mSingleCalculation.parametername = "Financing Fees Per Home";
                double FinancingFeePerHome = FinancingFee / mParams.NumberofHomes;
                mSingleCalculation.value = FinancingFeePerHome.ToString();
                mCalcList.Add(mSingleCalculation);
                // Net Profit Per Home 50
                mSingleCalculation.parameterid = "50";
                mSingleCalculation.parametername = "Net Profit Per Home";
                double NetProfitPerHome = Profit1 + FinancingFeePerHome;
                mSingleCalculation.value = NetProfitPerHome.ToString();
                mCalcList.Add(mSingleCalculation);
                // Total Net Profit 51
                mSingleCalculation.parameterid = "51";
                mSingleCalculation.parametername = "Total Net Profit";
                double TotalProfit = NetProfitPerHome * mParams.NumberofHomes;
                mSingleCalculation.value = TotalProfit.ToString();
                mCalcList.Add(mSingleCalculation);

                if (mStructType == "Apartment")
                {
                    //Apartment Estimated Value


                    //Building Square Footage

                    // Livable Square Footage

                    // Rent Per Unit

                    // Annual Rent

                    // Vacancy Rate

                    // Yearly Apartment Taxes

                    // Apartment Cap Rate

                    // Operating Expenses Per Foot
                }

                if (mStructType == "Townhome" || mStructType == "Rowhouse" || mStructType == "Cottage")
                {

                }
            }

            return mOutModel;
        }
    }
}
