using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_LAYER
{
    public struct stLicenseClassInfo
    {
        public int LicenseClassID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public decimal ClassFees { set; get; }
    }
    public class clsLicenseClassData
    {

        
        public static DataTable GetAllLicenseClasses()
        {
            DataTable table = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "select * from LicenseClasses";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    table.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                table = null;
            }
            finally { connection.Close(); }

            return table;
        }
        private static stLicenseClassInfo _ConvertDataRecordToStLicenseClassInfo(IDataRecord record)
        {
            return new stLicenseClassInfo
            {
                LicenseClassID = (int)record["LicenseClassID"],
                ClassName = (string)record["ClassName"],
                ClassDescription = (string)record["ClassDescription"],
                MinimumAllowedAge = (byte)record["MinimumAllowedAge"],
                DefaultValidityLength = (byte)record["DefaultValidityLength"],
                ClassFees = Convert.ToDecimal(record["ClassFees"])
            };
        }
        public static stLicenseClassInfo? GetLicenseClassByID(int licenseClassID)
        {
            stLicenseClassInfo? licenseClassInfo = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select * from LicenseClasses
                             where LicenseClassID = @licenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    licenseClassInfo = _ConvertDataRecordToStLicenseClassInfo((IDataRecord)reader);
                }

                reader.Close();
            }
            catch
            {
                licenseClassInfo = null;
            }
            finally
            {
                connection.Close();
            }

            return licenseClassInfo;


        }
        public static stLicenseClassInfo? GetLicenseClassByName(string className)
        {
            stLicenseClassInfo? licenseClassInfo = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select * from LicenseClasses
                             where ClassName = @className";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@className", className);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    licenseClassInfo = _ConvertDataRecordToStLicenseClassInfo((IDataRecord)reader);
                }

                reader.Close();
            }
            catch
            {
                licenseClassInfo = null;
            }
            finally
            {
                connection.Close();
            }

            return licenseClassInfo;


        }
        public static int AddNew(string className, string classDescription, byte minimumAllowedAge, byte defaultValidityLength, decimal classFees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"insert into LicenseClasses 
                             values(@className, @classDescription, @minimumAllowedAge, @defaultValidityLength, @classFees)
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@className", className);
            command.Parameters.AddWithValue("@classDescription", classDescription);
            command.Parameters.AddWithValue("@minimumAllowedAge", minimumAllowedAge);
            command.Parameters.AddWithValue("@defaultValidityLength", defaultValidityLength);
            command.Parameters.AddWithValue("@classFees", classFees);


            int newLicenseClassID = -1;
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    newLicenseClassID = insertedID;
                }
            }
            catch
            {
                newLicenseClassID = -1;
            }
            finally
            {
                connection.Close();
            }

            return newLicenseClassID;

        }
        public static bool Update(int licenseClassID ,string className, string classDescription, byte minimumAllowedAge, byte defaultValidityLength, decimal classFees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update LicenseClasses  
                            set className = @className,
                                classDescription = @classDescription,
                                minimumAllowedAge = @minimumAllowedAge,
                                defaultValidityLength = @defaultValidityLength,
                                classFees = @classFees
                                where licenseClassID = @licenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@className", className);
            command.Parameters.AddWithValue("@classDescription", classDescription);
            command.Parameters.AddWithValue("@minimumAllowedAge", minimumAllowedAge);
            command.Parameters.AddWithValue("@defaultValidityLength", defaultValidityLength);
            command.Parameters.AddWithValue("@classFees", classFees);
            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);

            int rowAffected = 0;
            try
            {
                connection.Open();

                rowAffected = command.ExecuteNonQuery();
            }
            catch
            {
                rowAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return rowAffected > 0;

        }
    }
}
