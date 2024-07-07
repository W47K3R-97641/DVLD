using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_LAYER
{
    public struct stDetainedLicenseInfo
    {
        public int DetainID { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }
        public decimal FineFees { set; get; }
        public int CreatedByUserID { set; get; }
        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }
        public int ReleasedByUserID { set; get; }
        public int ReleaseApplicationID { set; get; }
    }
    public class clsDetainedLicenseData
    {
        public static DataTable GetAllDetainedLicenses()
        {

            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "select * from detainedLicenses_View order by IsReleased ,LicenseID;";

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
                // Console.WriteLine("Error: " + ex.Message);
                table = null;
            }
            finally
            {
                connection.Close();
            }

            return table;

        }
        private static stDetainedLicenseInfo _ConvertRecordToStDetainedLicenseInfo(IDataRecord record)
        {
            return new stDetainedLicenseInfo
            {
                DetainID   = (int)record["LicenseID"],
                LicenseID  = (int)record["LicenseID"],
                DetainDate = (DateTime)record["DetainDate"],
                FineFees   = Convert.ToDecimal(record["FineFees"]),
                CreatedByUserID = (int)record["CreatedByUserID"],
                IsReleased      = (bool)record["IsReleased"],
                ReleaseDate     = (record["ReleaseDate"] == DBNull.Value) ? DateTime.MaxValue : (DateTime)record["ReleaseDate"],
                ReleasedByUserID = (record["ReleasedByUserID"] == DBNull.Value) ? -1 : (int)record["ReleasedByUserID"],
                ReleaseApplicationID = (record["ReleaseApplicationID"] == DBNull.Value) ? -1 : (int)record["ReleaseApplicationID"]
            };
        }
        public static stDetainedLicenseInfo? GetDetainedLicenseInfoByID(int DetainID)
        {
            stDetainedLicenseInfo? detainedLicenseInfo = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM DetainedLicenses WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", DetainID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    detainedLicenseInfo = _ConvertRecordToStDetainedLicenseInfo((IDataRecord)reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                detainedLicenseInfo = null;
            }
            finally
            {
                connection.Close();
            }

            return detainedLicenseInfo;
        }

        public static stDetainedLicenseInfo? GetDetainedLicenseInfoByLicenseID(int LicenseID)
        {
            stDetainedLicenseInfo? detainedLicenseInfo = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM DetainedLicenses WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    detainedLicenseInfo = _ConvertRecordToStDetainedLicenseInfo((IDataRecord)reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                detainedLicenseInfo = null;
            }
            finally
            {
                connection.Close();
            }

            return detainedLicenseInfo;
        }
        public static int AddNewDetainedLicense(
           int LicenseID, DateTime DetainDate,
           decimal FineFees, int CreatedByUserID)
        {
            

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO dbo.DetainedLicenses
                               (LicenseID,
                               DetainDate,
                               FineFees,
                               CreatedByUserID,
                               IsReleased
                               )
                            VALUES
                               (@LicenseID,
                               @DetainDate, 
                               @FineFees, 
                               @CreatedByUserID,
                               0
                             );
                            
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int DetainID = -1;
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DetainID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                DetainID = -1;
            }

            finally
            {
                connection.Close();
            }


            return DetainID;

        }

        public static bool UpdateDetainedLicense(int DetainID,
           int LicenseID, DateTime DetainDate,
           decimal FineFees, int CreatedByUserID)
        {

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE dbo.DetainedLicenses
                              SET LicenseID = @LicenseID, 
                              DetainDate = @DetainDate, 
                              FineFees = @FineFees,
                              CreatedByUserID = @CreatedByUserID,   
                              WHERE LicenseID=@LicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainedLicenseID", DetainID);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                rowsAffected = 0;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool IsLicenseDetained(int licenseID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select IsDetained=1 from DetainedLicenses where LicenseID = @licenseID and IsReleased = 0";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@licenseID", licenseID);

            bool IsDetained = false;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    IsDetained = Convert.ToBoolean(result);
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return IsDetained;
        }

        public static bool ReleaseDetainedLicense(int DetainID,
                int ReleasedByUserID, int ReleaseApplicationID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE dbo.DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate, 
                              ReleaseApplicationID = @ReleaseApplicationID   
                              WHERE LicenseID=@LicenseID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", DetainID);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

    }


}
