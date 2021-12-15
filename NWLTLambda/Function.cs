using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Amazon.Lambda.Core;
using NWLTLambda.Models;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace NWLTLambda
{
    public class Functions
    {
        public Functions()
        {

        }

        #region Resource (endpoints)
        public async Task<List<FormModel>> GetFormList(object request, ILambdaContext context) //task is the model if want to return
        {
            List<FormModel> mResponse = new List<FormModel>();
            mySqlAccess _sqlaccess = new mySqlAccess();
            ApiCityAuthorize mFullAuthRequest = new ApiCityAuthorize();
            CityInputModel mCityParams = new CityInputModel();
            FormModel mSingleResponse = new FormModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityAuthorize>(request.ToString());
                    mCityParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                mResponse = _sqlaccess.GetForms(mCityParams);
            }
            return mResponse;
        }
        
        public async Task<List<FormModel>> SearchForms(object request, ILambdaContext context) //task is the model if want to return
        {
            List<FormModel> mResponse = new List<FormModel>();
            mySqlAccess _sqlaccess = new mySqlAccess();
            ApiFormSearchAuthorize mFullAuthRequest = new ApiFormSearchAuthorize();
            FormSearchInputModel mSearchParams = new FormSearchInputModel();
            FormModel mSingleResponse = new FormModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiFormSearchAuthorize>(request.ToString());
                    mSearchParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                mResponse = _sqlaccess.SearchForms(mSearchParams);
            }
            return mResponse;
        }
        
        public async Task<List<FormValueModel>> GetFormValues (object request, ILambdaContext context) //task is the model if want to return
        {
            List<FormValueModel> mResponse = new List<FormValueModel>();
            mySqlAccess _sqlaccess = new mySqlAccess();
            ApiFormAuthorize mFullAuthRequest = new ApiFormAuthorize();
            FormValueInputModel mInputForm = new FormValueInputModel();
            FormValueModel mSingleResponse = new FormValueModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiFormAuthorize>(request.ToString());
                    mInputForm = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                mResponse = _sqlaccess.GetFormValues(mInputForm);
            }
            return mResponse;
        }

        public async Task<List<CityModel>> GetCities(object request, ILambdaContext context) //task is the model if want to return
        {
            List<CityModel> mResponse = new List<CityModel>();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CityModel mSingleResponse = new CityModel();
            // Verify Request

            context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");



            // Save Both Inputs to DB
            mResponse = _sqlaccess.GetCities();


            return mResponse;
        }

        public async Task<List<ParametersOutputModel>> GetParameters(object request, ILambdaContext context) //task is the model if want to return
        {
            List<ParametersOutputModel> mResponse = new List<ParametersOutputModel>();
            ApiCityAuthorize mFullAuthRequest = new ApiCityAuthorize();
            CityInputModel mCityParams = new CityInputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CalcEngine _calcEngine = new CalcEngine();
            ParametersOutputModel mSingleResponse = new ParametersOutputModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityAuthorize>(request.ToString());
                    mCityParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }



                // Save Both Inputs to DB
                mResponse = _sqlaccess.GetCityParams(mCityParams);
            }

            return mResponse;
        }

        public async Task<List<AllParametersOutputModel>> GetAllParameters(object request, ILambdaContext context) //task is the model if want to return
        {
            List<AllParametersOutputModel> mResponse = new List<AllParametersOutputModel>();
            ApiCityAuthorize mFullAuthRequest = new ApiCityAuthorize();
            CityInputModel mCityParams = new CityInputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CalcEngine _calcEngine = new CalcEngine();
            AllParametersOutputModel mSingleResponse = new AllParametersOutputModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityAuthorize>(request.ToString());
                    mCityParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }



                // Request from DB
                mResponse = _sqlaccess.GetAllParams(mCityParams);
            }

            return mResponse;
        }

        public async Task<List<ParametersOutputModel>> GetCalcParameters(object request, ILambdaContext context) //task is the model if want to return
        {
            List<ParametersOutputModel> mResponse = new List<ParametersOutputModel>();
            ApiCityAuthorize mFullAuthRequest = new ApiCityAuthorize();
            CityInputModel mCityParams = new CityInputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CalcEngine _calcEngine = new CalcEngine();
            ParametersOutputModel mSingleResponse = new ParametersOutputModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityAuthorize>(request.ToString());
                    mCityParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }



                // Request from DB
                mResponse = _sqlaccess.GetCalcParams(mCityParams);
            }

            return mResponse;
        }

        public async Task<List<ParametersOutputModel2>> GetFormParameters(object request, ILambdaContext context) //task is the model if want to return
        {
            List<ParametersOutputModel2> mResponse = new List<ParametersOutputModel2>();
            ApiCityAuthorize mFullAuthRequest = new ApiCityAuthorize();
            CityInputModel mCityParams = new CityInputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CalcEngine _calcEngine = new CalcEngine();
            ParametersOutputModel2 mSingleResponse = new ParametersOutputModel2();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityAuthorize>(request.ToString());
                    mCityParams = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }



                // Save Both Inputs to DB
                mResponse = _sqlaccess.GetFormParams(mCityParams);
            }

            return mResponse;
        }

        public async Task<List<ParameterValuesOutputModel>> GetParameterValues(object request, ILambdaContext context)
        {
            List<ParameterValuesOutputModel> mResponse = new List<ParameterValuesOutputModel>();
            ApiCityParamAuthorize mFullAuthRequest = new ApiCityParamAuthorize();
            ParameterValuesInputModel mParamValues = new ParameterValuesInputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            ParameterValuesOutputModel mSingleResponse = new ParameterValuesOutputModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiCityParamAuthorize>(request.ToString());
                    mParamValues = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }


                // Save Both Inputs to DB
                mResponse = _sqlaccess.GetParameterValues(mParamValues);
            }

            return mResponse;
        }

        public async Task<OutputModel> SFProforma(object request, ILambdaContext context)
        {
            OutputModel mResponse = new OutputModel();
            ApiAuthorize  mFullRequest = new ApiAuthorize();
            InputModel mInputParams = new InputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            CalcEngine _calcEngine = new CalcEngine();
            ReCalcEngine _recalcEngine = new ReCalcEngine();
            ParametersList mParams = new ParametersList();
            List<StructureTypesVM> mStructureParams = new List<StructureTypesVM>();

            // Verify Request
            if (request == null)
            {
                mResponse.MessageText = "Request cannot be empty";
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullRequest = JsonConvert.DeserializeObject<ApiAuthorize>(request.ToString());
                    mInputParams = mFullRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mResponse.MessageText = "Request is Invalid: " + ex.Message;
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }

                if (mInputParams.parameters.Count > 0)
                {
                    foreach (ParameterInputModel mParam in mInputParams.parameters)
                    {
                        switch (mParam.parameterid)
                        {   
                            case 4:
                                mParams.YearBuilt = Convert.ToInt32(mParam.value);
                                break;
                            case 5:
                                mParams.LotSize = Convert.ToDouble(mParam.value);
                                break;
                            case 6:
                                mParams.Zoning = mParam.value.ToString();
                                break;
                            case 7:
                                mParams.MhaSuffix = mParam.value.ToString();
                                break;
                            case 8:
                                mParams.AssessedValue = Convert.ToDouble(mParam.value);
                                break;
                            case 9:
                                mParams.LotWidth = Convert.ToDouble(mParam.value);
                                break;
                            case 10:
                                mParams.LotLength = Convert.ToDouble(mParam.value);
                                break;
                            case 11:
                                mParams.HighEndArea = mParam.value.ToString();
                                break;
                            case 12:
                                mParams.MajorArterial = mParam.value.ToString();
                                break;
                            case 13:
                                mParams.CornerLot = mParam.value.ToString();
                                break;
                            case 14:
                                mParams.Alley = mParam.value.ToString();
                                break;
                            case 15:
                                mParams.GrowthArea = mParam.value.ToString();
                                break;
                            case 16:
                                mParams.ElevationChange = Convert.ToDouble(mParam.value);
                                break;
                            case 17:
                                mParams.SlopeDirection = mParam.value.ToString();
                                break;
                            case 18:
                                mParams.TreeCanopyCoverage = mParam.value.ToString();
                                break;
                            case 19:
                                mParams.Ecas = mParam.value.ToString();
                                break;
                            case 20:
                                mParams.WaterMainExtension = mParam.value.ToString();
                                break;
                            case 21:
                                mParams.WaterMainLength = Convert.ToDouble(mParam.value);
                                break;
                            case 22:
                                mParams.SewerMainExtension = mParam.value.ToString();
                                break;
                            case 23:
                                mParams.SewerMainLength = Convert.ToDouble(mParam.value);
                                break;
                            case 24:
                                mParams.AlleyConstruction = mParam.value.ToString();
                                break;
                            case 25:
                                mParams.AlleyConstructionLength = Convert.ToDouble(mParam.value);
                                break;
                            case 26:
                                mParams.UndergroundGarage = mParam.value.ToString();
                                break;
                            case 27:
                                mParams.NumberOfSpots = Convert.ToInt32(mParam.value);
                                break;
                            //case 28:
                            //    mParams.StructureType = mParam.value.ToString();
                            //    break;
                            case 29:
                                mParams.Mhaarea = mParam.value.ToString();
                                break;
                            case 83:
                                mParams.DrainageMain = mParam.value.ToString();
                                break;
                            case 84:
                                mParams.DrainageDistance = Convert.ToDouble(mParam.value);
                                break;
                            case 30:
                                mParams.Number_of_homes = Convert.ToInt32(mParam.value);
                                break;
                            case 32:
                                mParams.Aadu = Convert.ToDouble(mParam.value);
                                break;
                            case 33:
                                mParams.Aadu_size = Convert.ToDouble(mParam.value);
                                break;
                            case 34:
                                mParams.Dadu = Convert.ToDouble(mParam.value);
                                break;
                            case 35:
                                mParams.Dadu_size = Convert.ToDouble(mParam.value);
                                break;
                            case 81:
                                mParams.LoanFeesLandAcquisitionLoan = Convert.ToDouble(mParam.value);
                                break;
                            case 60:
                                mParams.LoanFeesConstructionLoan = Convert.ToDouble(mParam.value);
                                break;
                        }
                    }
                }
                mParams.CityId = Convert.ToInt32(mInputParams.CityId);
                int mRet2 = 0;
                // Step 1.    Save Inputs to DB -- this could eventually check for a valid returning formID and if not it cannot move to next steps
                if (mInputParams.formId is object)
                {
                    // Add structure type to mInput Params
                    
                    mResponse.FormId = Convert.ToInt32(mInputParams.formId);
                    mResponse.Address = mInputParams.address;
                    mResponse.CityId = Convert.ToInt32(mInputParams.CityId);
                    mResponse.InputUser = mInputParams.username;
                    mResponse.FormValues = mInputParams.parameters;
                    // Step 3.    Start the calculation
                    foreach (StructureTypesVM mStructure in mInputParams.StructureTypes)
                    {
                        mParams.StructureType = mStructure.StructureType;
                        var mLatestStruct = _recalcEngine.ReCalcProforma(mParams);
                        mStructureParams.Add(mLatestStruct);
                    }
                    mResponse.StructureTypes = mStructureParams;
                    mRet2 = await _sqlaccess.ProformaUpdateValues(mResponse.StructureTypes, mResponse.FormId);
                }
                else
                {
                    var UINT64FormID = await _sqlaccess.SaveSFProformaInput(mInputParams);
                    mResponse.FormId = Convert.ToInt32(UINT64FormID);
                    // Step 2.    Assign other vital data to OUTPUT stream 
                    mResponse.Address = mInputParams.address;
                    mResponse.CityId = Convert.ToInt32(mInputParams.CityId);
                    mResponse.InputUser = mInputParams.username;
                    mResponse.FormValues = mInputParams.parameters;
                    // Step 3.    Start the calculation
                    mResponse.StructureTypes = _calcEngine.CalcSFProforma(mParams);
                    mRet2 = await _sqlaccess.SFSaveFCalcValues(mResponse.StructureTypes, mResponse.FormId);
                }

                // Step 4.    Save Calculations to DB with same FormID
                //  TODO  ----  pass List<StructureTypesVM>  to a sql helper and save like you save InputParams
                
            }

            return mResponse;
        }

        public async Task<OutputModel> GetFormValues2(object request, ILambdaContext context) //task is the model if want to return
        {
            OutputModel mResponse = new OutputModel();
            mySqlAccess _sqlaccess = new mySqlAccess();
            ApiFormAuthorize mFullAuthRequest = new ApiFormAuthorize();
            FormValueInputModel mInputForm = new FormValueInputModel();
            FormValueModel mSingleResponse = new FormValueModel();
            // Verify Request
            if (request == null)
            {
                mSingleResponse.mResponseMessage = "Request cannot be empty";
                //mResponse.Add(mSingleResponse);
                context.Logger.LogLine($"ERROR:" + "RequestID: " + context.AwsRequestId + " Request string is Empty");

            }
            else
            {
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                try
                {
                    mFullAuthRequest = JsonConvert.DeserializeObject<ApiFormAuthorize>(request.ToString());
                    mInputForm = mFullAuthRequest.Body;
                    context.Logger.LogLine($"Info: Request sent is: " + "RequestID: " + context.AwsRequestId + " " + request.ToString());
                }
                catch (Exception ex)
                {
                    mSingleResponse.mResponseMessage = "Request is Invalid: " + ex.Message;
                    //mResponse.Add(mSingleResponse);
                    context.Logger.LogLine($"ERROR: " + "RequestID: " + context.AwsRequestId + " Invalid Request: " + ex.Message);
                    return mResponse;
                }
                context.Logger.LogLine($"Info: " + "RequestID: " + context.AwsRequestId + ", Deserializing request string");
                mResponse = _sqlaccess.GetFormValues2(mInputForm);
            }

            return mResponse;
        }
        #endregion
    }
}
