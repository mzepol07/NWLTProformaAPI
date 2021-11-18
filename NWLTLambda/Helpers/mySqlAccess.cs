using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NWLTLambda.Models;
using System.IO;
using Amazon.SecretsManager;
using Amazon;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Amazon.SecretsManager.Model;

namespace NWLTLambda
{
    public class mySqlAccess
    {
        public async Task<UInt64> SaveSFProformaInput(InputModel mInputmodel)
        {
            string mConnection = GetSecret("properties");
            UInt64 mRet = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.sp_save_sfproforma", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@mydata", JsonConvert.SerializeObject(mInputmodel));

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        // mRetNumber = await Cmd.ExecuteNonQueryAsync();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            mRet = (UInt64)mSqlReader[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }

            return mRet;
        }

        public async Task<int> SFSaveFCalcValues(List<StructureTypesVM> mStructureTypes, int mformID)
        {
            string mConnection = GetSecret("properties");
            int mRetNumber = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspSFSaveCalcValues", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@paramList", JsonConvert.SerializeObject(mStructureTypes));
                    Cmd.Parameters.AddWithValue("@formId", mformID);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspSFSaveCalcValues stored Proc. ");
                        Conn.Open();
                        mRetNumber = await Cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mRetNumber;
        }
        public async Task<int> ProformaUpdateValues(List<StructureTypesVM> mStructureTypes, int mformID)
        {
            string mConnection = GetSecret("properties");
            int mRetNumber = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspUpdateSfProforma", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@paramList", JsonConvert.SerializeObject(mStructureTypes));
                    Cmd.Parameters.AddWithValue("@formId", mformID);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspUpdateSfProforma stored Proc. ");
                        Conn.Open();
                        mRetNumber = await Cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }
            return mRetNumber;
        }
        public List<CityModel> GetCities()
        {
            string mConnection = GetSecret("properties");
            List<CityModel> mParamList = new List<CityModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetCities", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetCites stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            CityModel mParam = new CityModel();
                            mParam.CityId = (int)mSqlReader[0];
                            mParam.CityName = mSqlReader[1].ToString();
                            mParam.StateAbbrev = mSqlReader[2].ToString();
                            mParam.Region = mSqlReader[3].ToString();
                            mParam.TimeZone = mSqlReader[4].ToString();
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<FormModel> GetForms(CityInputModel mCityInputModel)
        {
            string mConnection = GetSecret("properties");
            List<FormModel> mParamList = new List<FormModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetFormList", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityInputModel.CityId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            FormModel mParam = new FormModel();
                            mParam.city_id = (int)mSqlReader[0];
                            mParam.form_id = (int)mSqlReader[1];
                            mParam.form_name = mSqlReader[2].ToString();
                            mParam.form_creator = mSqlReader[3].ToString();
                            mParam.form_creation_date = mSqlReader[4].ToString();
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }
        public List<FormModel> SearchForms(FormSearchInputModel mFormSearchInputModel)
        {
            string mConnection = GetSecret("properties");
            List<FormModel> mParamList = new List<FormModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspSearchForms", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mFormSearchInputModel.cityId);
                    Cmd.Parameters.AddWithValue("searchString", mFormSearchInputModel.SearchString);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            FormModel mParam = new FormModel();
                            mParam.city_id = (int)mSqlReader[0];
                            mParam.form_id = (int)mSqlReader[1];
                            mParam.form_name = mSqlReader[2].ToString();
                            mParam.form_creator = mSqlReader[3].ToString();
                            mParam.form_creation_date = mSqlReader[4].ToString();
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<FormValueModel> GetFormValues(FormValueInputModel mInputModel)
        {
            string mConnection = GetSecret("properties");
            List<FormValueModel> mParamList = new List<FormValueModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetFormValues", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("formID", mInputModel.FormId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetFormValues stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            FormValueModel mParam = new FormValueModel();
                            mParam.property_id = (int)mSqlReader[0];
                            mParam.form_name = mSqlReader[1].ToString();
                            mParam.city_name = mSqlReader[2].ToString();
                            mParam.form_creation_date = mSqlReader[3].ToString();
                            mParam.parameter_id = (int)mSqlReader[4];
                            mParam.parameter_name = mSqlReader[5].ToString();
                            mParam.value = mSqlReader[6].ToString();
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public OutputModel GetFormValues2(FormValueInputModel mInputModel)
        {
            string mConnection = GetSecret("properties");
            int mFormID = 0;
            string mAddress = "";
            string mCityID = "";
            string mInputUser = "";
            string mStructureTypeCurrent = "";

            OutputModel mParamList = new OutputModel();
            List<ParameterInputModel> mParamInputList = new List<ParameterInputModel>();
            List<StructureTypesVM> mStructuresList = new List<StructureTypesVM>();
            List<CalcParameterOutputModel> mListOfCalcParam = new List<CalcParameterOutputModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetAllFormValues", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("formID", mInputModel.FormId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetFormValues stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            //  group all form input values together
                            if (mSqlReader[4].ToString() == "N/A")
                            {
                                ParameterInputModel mParamInput = new ParameterInputModel();
                                mParamInput.parameterid = (int)mSqlReader[5];
                                mParamInput.parametername = mSqlReader[6].ToString();
                                mParamInput.value = mSqlReader[9].ToString();
                                mParamInputList.Add(mParamInput);
                            }
                            else
                            {
                                //   Group all by structuretype
                                //  StructureType, List<CalcParameterOutputModel>
                                StructureTypesVM mCalcParam = new StructureTypesVM();
                                if (mStructureTypeCurrent != mSqlReader[4].ToString())
                                {
                                    if (mStructureTypeCurrent != "")
                                    {
                                        mStructuresList.Add(mCalcParam);
                                        mListOfCalcParam.Clear();
                                    }
                                    mStructureTypeCurrent = mSqlReader[4].ToString();
                                    mCalcParam.StructureType = mStructureTypeCurrent;
                                }
                                CalcParameterOutputModel mCParamOutput = new CalcParameterOutputModel();
                                mCParamOutput.parameterid = mSqlReader[5].ToString();
                                mCParamOutput.parametername = mSqlReader[6].ToString();
                                mCParamOutput.value = mSqlReader[9].ToString();
                                mListOfCalcParam.Add(mCParamOutput);
                            }
                            // save formid, address, cityid, inputuser  to Vars
                            mFormID = (int)mSqlReader[0];
                            mAddress = mSqlReader[1].ToString();
                            mCityID = mSqlReader[10].ToString();
                            mInputUser = "";  //  TODO -- add inputuser to sP
                        }
                        mParamList.FormId = mFormID;
                        mParamList.CityId = Convert.ToInt32(mCityID);
                        mParamList.FormValues = mParamInputList;
                        mParamList.StructureTypes = mStructuresList;
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<ParametersOutputModel> GetCityParams(CityInputModel mCityInputModel)
        {
            string mConnection = GetSecret("properties");
            List<ParametersOutputModel> mParamList = new List<ParametersOutputModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetParameters", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityInputModel.CityId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            ParametersOutputModel mParam = new ParametersOutputModel();
                            mParam.ParameterId = (int)mSqlReader[0];
                            mParam.ParameterName = mSqlReader[1].ToString();
                            mParam.HtmlHeader = mSqlReader[2].ToString();
                            mParam.HtmlTag = mSqlReader[3].ToString();
                            mParam.Phase = mSqlReader[4].ToString();
                            mParam.Stage = mSqlReader[5].ToString();
                            mParam.ParameterTypeId = (int)mSqlReader[6];
                            mParam.ParamOrder = (int)mSqlReader[7];
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<AllParametersOutputModel> GetAllParams(CityInputModel mCityInputModel)
        {
            string mConnection = GetSecret("properties");
            List<AllParametersOutputModel> mParamList = new List<AllParametersOutputModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetAllParameters", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityInputModel.CityId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetAllParameters stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            AllParametersOutputModel mParam = new AllParametersOutputModel();
                            mParam.ParameterId = (int)mSqlReader[0];
                            mParam.ParameterName = mSqlReader[1].ToString();
                            mParam.HtmlHeader = mSqlReader[2].ToString();
                            mParam.HtmlTag = mSqlReader[3].ToString();
                            mParam.Phase = mSqlReader[4].ToString();
                            mParam.Stage = mSqlReader[5].ToString();
                            mParam.ParameterTypeId = (int)mSqlReader[5];
                            mParam.ParamOrder = (int)mSqlReader[6];
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<ParametersOutputModel> GetCalcParams(CityInputModel mCityInputModel)
        {
            string mConnection = GetSecret("properties");
            List<ParametersOutputModel> mParamList = new List<ParametersOutputModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"properties.uspGetCalcParameters", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityInputModel.CityId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetCalcParameters stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            ParametersOutputModel mParam = new ParametersOutputModel();
                            mParam.ParameterId = (int)mSqlReader[0];
                            mParam.ParameterName = mSqlReader[1].ToString();
                            mParam.HtmlHeader = mSqlReader[2].ToString();
                            mParam.HtmlTag = mSqlReader[3].ToString();
                            mParam.Phase = mSqlReader[4].ToString();
                            mParam.ParameterTypeId = (int)mSqlReader[5];
                            mParam.ParamOrder = (int)mSqlReader[6];
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<ParametersOutputModel2> GetFormParams(CityInputModel mCityInputModel)
        {
            string mConnection = GetSecret("parameters");
            List<ParametersOutputModel2> mParamList = new List<ParametersOutputModel2>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"parameters.uspGetFormParameters", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityInputModel.CityId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            ParametersOutputModel2 mParam = new ParametersOutputModel2();
                            mParam.ParameterId = (int)mSqlReader[0];
                            mParam.ParameterName = mSqlReader[1].ToString();
                            mParam.HtmlHeader = mSqlReader[2].ToString();
                            mParam.HtmlTag = mSqlReader[3].ToString();
                            mParam.Phase = mSqlReader[4].ToString();
                            mParam.Stage = mSqlReader[5].ToString();
                            mParam.ParameterTypeId = (int)mSqlReader[6];
                            mParam.ParamOrder = (int)mSqlReader[7];
                            mParamList.Add(mParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mParamList;
        }

        public List<ParameterValuesOutputModel> GetParameterValues(ParameterValuesInputModel mParameterValuesInputModel)
        {

            string mConnection = GetSecret("parameters");

            List<ParameterValuesOutputModel> mListValues = new List<ParameterValuesOutputModel>();

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"parameters.uspGetParameterValues", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mParameterValuesInputModel.CityId);
                    Cmd.Parameters.AddWithValue("parameterID", mParameterValuesInputModel.ParameterId);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspGetParameterValues stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            ParameterValuesOutputModel mParamValue = new ParameterValuesOutputModel();
                            mParamValue.ParameterId = (int)mSqlReader[0];
                            mParamValue.ParameterName = mSqlReader[1].ToString();
                            mParamValue.HtmlHeader = mSqlReader[2].ToString();
                            mParamValue.HtmlTag = mSqlReader[3].ToString();
                            mParamValue.Phase = mSqlReader[4].ToString();
                            mParamValue.Value = mSqlReader[5].ToString();
                            mListValues.Add(mParamValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }


            return mListValues;
        }

        public decimal GetBaseCost(int mCityID, string isHighEnd, string mStructureType) 
        {
            string mConnection = GetSecret("lookups");
            decimal mReturn = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"lookups.uspLookupBuildingBaseCost", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityID);
                    Cmd.Parameters.AddWithValue("isHighEnd", isHighEnd);
                    Cmd.Parameters.AddWithValue("structureType", mStructureType);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            mReturn =  (decimal)mSqlReader[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }

            return mReturn;
        }

        public decimal GetDensityLimit(int mCityID, string mZoning , string mStructureType, string hasMhaSuffix, string isCorner)
        {
            string mConnection = GetSecret("lookups");
            decimal mReturn = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"lookups.uspLookupDensityLimit", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityID);
                    Cmd.Parameters.AddWithValue("zoningCode", mZoning);
                    Cmd.Parameters.AddWithValue("structureType", mStructureType);
                    Cmd.Parameters.AddWithValue("hasMhaSuffix", hasMhaSuffix);
                    Cmd.Parameters.AddWithValue("isCorner", isCorner);


                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            mReturn = (decimal)mSqlReader[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }

            return mReturn;
        }

        public decimal GetFAR(int mCityID, string mZoning, string mStructureType, string hasMhaSuffix, string isGrowthArea)
        {
            string mConnection = GetSecret("lookups");
            decimal mReturn = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"lookups.uspLookupFAR", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityID);
                    Cmd.Parameters.AddWithValue("zoningCode", mZoning);
                    Cmd.Parameters.AddWithValue("structureType", mStructureType);
                    Cmd.Parameters.AddWithValue("hasMhaSuffix", hasMhaSuffix);
                    Cmd.Parameters.AddWithValue("isGrowthArea", isGrowthArea);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            mReturn = (decimal)mSqlReader[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }

            return mReturn;
        }

        public decimal GetMHAFee(int mCityID, string mhaAreaID, string mhaSuffixID)
        {
            string mConnection = GetSecret("lookups");
            decimal mReturn = 0;

            using (var Conn = new MySqlConnection(mConnection))
            {
                using (var Cmd = new MySqlCommand($"lookups.uspLookupMHAFee", Conn))
                {
                    // Open SQL Connection
                    Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("cityId", mCityID);
                    Cmd.Parameters.AddWithValue("mhaArea", mhaAreaID);
                    Cmd.Parameters.AddWithValue("mhaSuffix", mhaSuffixID);

                    // Execute SQL Command
                    try
                    {
                        LambdaLogger.Log($"SQL INFO: executing uspStoreInputParams stored Proc. ");
                        Conn.Open();
                        MySqlDataReader mSqlReader = Cmd.ExecuteReader();
                        while (mSqlReader.Read())
                        {
                            mReturn = (decimal)mSqlReader[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        var mMessage = ex.Message;
                        LambdaLogger.Log($"SQL ERROR: " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
            }

            return mReturn;
        }

        public string GetSecret(string mSchema)
        {
            string secretName = "databaseconn";
            string region = "us-west-2";
            string secret = "";

            MemoryStream memoryStream = new MemoryStream();

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;
            request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

            GetSecretValueResponse response = null;

            // In this sample we only handle the specific exceptions for the 'GetSecretValue' API.
            // See https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            // We rethrow the exception by default.

            try
            {
                response = client.GetSecretValueAsync(request).Result;
            }
            catch (DecryptionFailureException e)
            {
                // Secrets Manager can't decrypt the protected secret text using the provided KMS key.
                // Deal with the exception here, and/or rethrow at your discretion.
                string mMessage = e.Message;
                throw;
            }
            catch (InternalServiceErrorException e)
            {
                // An error occurred on the server side.
                // Deal with the exception here, and/or rethrow at your discretion.
                string mMessage = e.Message;
                throw;
            }
            catch (InvalidParameterException e)
            {
                // You provided an invalid value for a parameter.
                // Deal with the exception here, and/or rethrow at your discretion
                string mMessage = e.Message;
                throw;
            }
            catch (InvalidRequestException e)
            {
                // You provided a parameter value that is not valid for the current state of the resource.
                // Deal with the exception here, and/or rethrow at your discretion.
                string mMessage = e.Message;
                throw;
            }
            catch (ResourceNotFoundException e)
            {
                // We can't find the resource that you asked for.
                // Deal with the exception here, and/or rethrow at your discretion.
                string mMessage = e.Message;
                throw;
            }
            catch (System.AggregateException e)
            {
                // More than one of the above exceptions were triggered.
                // Deal with the exception here, and/or rethrow at your discretion.
                string mMessage = e.Message;
                throw;
            }

            // Decrypts secret using the associated KMS CMK.
            // Depending on whether the secret is a string or binary, one of these fields will be populated.
            if (response.SecretString != null)
            {
                secret = response.SecretString;
                SecretDBModel mDBModel = JsonConvert.DeserializeObject<SecretDBModel>(secret);
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = mDBModel.host;
                builder.UserID = mDBModel.username;
                builder.Password = mDBModel.password;
                builder.Database = mSchema;
                builder.Port = mDBModel.port;
                secret = builder.ToString();
            }
            else
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                secret = decodedBinarySecret;

                secret = "";
            }
            return secret;
            // Your code goes here.
        }



    }
}
