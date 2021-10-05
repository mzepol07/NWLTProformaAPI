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

        public async Task<List<ParametersOutputModel>> GetFormParameters(object request, ILambdaContext context) //task is the model if want to return
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
            ParametersList mParams = new ParametersList();

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
                            case "1":
                                mParams.State = mParam.value.ToString();
                                break;
                            case "2":
                                mParams.City = Convert.ToInt32(mParam.value);
                                break;
                            case "3":
                                mParams.Address = mParam.value.ToString();
                                break;
                            case "4":
                                mParams.YearBuilt = Convert.ToInt32(mParam.value);
                                break;
                            case "5":
                                mParams.LotSize = Convert.ToDouble(mParam.value);
                                break;
                            case "6":
                                mParams.Zoning = mParam.value.ToString();
                                break;
                            case "7":
                                mParams.MHASuffix = mParam.value.ToString();
                                break;
                            case "8":
                                mParams.AccessedValue = Convert.ToDouble(mParam.value);
                                break;
                            case "9":
                                mParams.LotWidth = Convert.ToDouble(mParam.value);
                                break;
                            case "10":
                                mParams.LotLength = Convert.ToDouble(mParam.value);
                                break;
                            case "11":
                                mParams.HighEndArea = mParam.value.ToString();
                                break;
                            case "12":
                                mParams.MajorArterial = mParam.value.ToString();
                                break;
                            case "13":
                                mParams.CornerLot = mParam.value.ToString();
                                break;
                            case "14":
                                mParams.Alley = mParam.value.ToString();
                                break;
                            case "15":
                                mParams.GrowthArea = mParam.value.ToString();
                                break;
                            case "16":
                                mParams.ElevationChange = mParam.value.ToString();
                                break;
                            case "17":
                                mParams.SlopeDirection = mParam.value.ToString();
                                break;
                            case "18":
                                mParams.TreeCanopy = mParam.value.ToString();
                                break;
                            case "19":
                                mParams.ECAs = mParam.value.ToString();
                                break;
                            case "20":
                                mParams.WaterMainExtension = mParam.value.ToString();
                                break;
                            case "21":
                                mParams.WaterMainLength = Convert.ToDouble(mParam.value);
                                break;
                            case "22":
                                mParams.SewerMainExtension = mParam.value.ToString();
                                break;
                            case "23":
                                mParams.SewerMainLength = Convert.ToDouble(mParam.value);
                                break;
                            case "24":
                                mParams.AlleyReconstruction = mParam.value.ToString();
                                break;
                            case "25":
                                mParams.AlleyReconstructionLength = Convert.ToDouble(mParam.value);
                                break;
                            case "26":
                                mParams.UndergroundGarage = mParam.value.ToString();
                                break;
                            case "27":
                                mParams.NumberofSpots = Convert.ToInt32(mParam.value);
                                break;
                            case "28":
                                mParams.StructureType = mParam.value.ToString();
                                break;
                            case "29":
                                mParams.MHaArea = mParam.value.ToString();
                                break;
                            case "30":
                                mParams.NumberofHomes = Convert.ToInt32(mParam.value);
                                break;
                            case "31":
                                mParams.ListingPrice = Convert.ToDouble(mParam.value);
                                break;
                            case "32":
                                mParams.AADU = Convert.ToDouble(mParam.value);
                                break;
                            case "33":
                                mParams.AADUSize = Convert.ToDouble(mParam.value);
                                break;
                            case "34":
                                mParams.DADU = Convert.ToDouble(mParam.value);
                                break;
                            case "35":
                                mParams.DADUSize = Convert.ToDouble(mParam.value);
                                break;
                            case "36":
                                mParams.EstimatedValue = Convert.ToDouble(mParam.value);
                                break;
                        }
                    }
                }
               
                // Step 1.    Save Inputs to DB -- this could eventually check for a valid returning formID and if not it cannot move to next steps
                mResponse.FormId = await _sqlaccess.SaveSFProformaInput(mInputParams);
                // Step 2.    Assign other vital data to OUTPUT stream 
                mResponse.Address = mInputParams.address;
                mResponse.CityId = Convert.ToInt32(mInputParams.CityId);
                mResponse.InputUser = mInputParams.username;
                mResponse.FormValues = mInputParams.parameters;
                // Step 3.    Start the calculation
                mResponse.StructureTypes = _calcEngine.CalcSFProforma(mParams);
                // Step 4.    Save Calculations to DB with same FormID
                //  TODO  ----  pass List<StructureTypesVM>  to a sql helper and save like you save InputParams
                int mRet2 = await _sqlaccess.SFSaveFCalcValues(mResponse.StructureTypes, mResponse.FormId);
            }

            return mResponse;
        }


        #endregion
    }
}
