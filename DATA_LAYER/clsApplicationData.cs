using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace DATA_LAYER
{
    public struct stApplicationInfo
    {
        public int ApplicationID;
        public int ApplicantPersonID;
        public DateTime ApplicationDate;
        public int ApplicationTypeID;
        public byte ApplicationStatus;
        public int CreatedByUserID;
        public DateTime LastStatusDate;
        public decimal PaidFees;
    }
    public class clsApplicationData
    {
        public static DataTable GetAllApplicationType()
        {
            DataTable table = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "select * from Applications order by ApplicationDate desc";

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
            finally
            { connection.Close(); }

            return table;
        }
        public static bool Update(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             byte ApplicationStatus, DateTime LastStatusDate,
             decimal PaidFees, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Applications  
                            set ApplicantPersonID = @ApplicantPersonID,
                                ApplicationDate = @ApplicationDate,
                                ApplicationTypeID = @ApplicationTypeID,
                                ApplicationStatus = @ApplicationStatus,
                                LastStatusDate = @LastStatusDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID
                             where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            int rowAffected = -1;

            try
            {
                connection.Open();

                rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                rowAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return rowAffected > 0;
        }
        public static int AddNew(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             byte ApplicationStatus, DateTime LastStatusDate,
             decimal PaidFees, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"insert into Applications 
                             values(@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID)
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            

            int applicationID = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    applicationID = insertedID;
            }
            catch { applicationID = -1; }
            finally { connection.Close(); }

            return applicationID;

        }
        private static stApplicationInfo _ConvertDataRecordToStApplicationInfo(IDataRecord record)
        {
            return new stApplicationInfo
            {
                ApplicationID = (int)record["ApplicationID"],
                ApplicantPersonID = (int)record["ApplicantPersonID"],
                ApplicationDate = (DateTime)record["ApplicationDate"],
                ApplicationTypeID = (int)record["ApplicationTypeID"],
                ApplicationStatus = (byte)record["ApplicationStatus"],
                LastStatusDate = (DateTime)record["LastStatusDate"],
                PaidFees = (decimal)record["PaidFees"],
                CreatedByUserID = (int)record["CreatedByUserID"],
            };

        }
        public static stApplicationInfo? GetApplicationTypeByID(int ApplicationID)
        {
            stApplicationInfo? ApplicationInfo = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ApplicationInfo = _ConvertDataRecordToStApplicationInfo((IDataRecord)reader);
                }
                reader.Close();

            }
            catch
            {
                ApplicationInfo = null;
            }
            finally
            {
                connection.Close();
            }

            return ApplicationInfo;
        }
        public static bool DeleteApplication(int ApplicationID)
        {

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Delete Applications 
                                where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            int rowsAffected = 0;
            
            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                rowsAffected = 0;
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);
        }
        public static int GetActiveApplicationID(int ApplicantPersonID, int ApplicationTypeID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

           
            string query = @"Select Found=1 from Applications
                              where ApplicantPersonID = @ApplicantPersonID and ApplicationStatus = 1 and ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            int ActiveApplicationID = -1;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int AppID))
                {
                    ActiveApplicationID = AppID;
                }

            }
            catch
            {
                ActiveApplicationID = -1;
            }
            finally
            {
                connection.Close();
            }
            return ActiveApplicationID;
        }
        public static int GetActiveApplicationIDForLicenseClass(int ApplicantPersonID, int ApplicationTypeID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);


            string query = @"SELECT ActiveApplicationID=Applications.ApplicationID  
                            From
                            Applications JOIN
                            LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            WHERE ApplicantPersonID = @ApplicantPersonID 
                            and ApplicationTypeID=@ApplicationTypeID 
							and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClass
                            and ApplicationStatus=1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);


            int ActiveApplicationID = -1;

            try
            {
                connection.Open();

                

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int AppID))
                {
                    ActiveApplicationID = AppID;
                }

            }
            catch
            {
                ActiveApplicationID = -1;
            }
            finally
            {
                connection.Close();
            }
            return ActiveApplicationID;
        }
        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return GetActiveApplicationID(PersonID, ApplicationTypeID) != -1;
        }
        public static bool UpdateStatus(int ApplicationID, short NewStatus)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Applications  
                            set ApplicationStatus = @ApplicationStatus
                                where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

          
            command.Parameters.AddWithValue("@ApplicationStatus", NewStatus);
            
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            int rowAffected = -1;

            try
            {
                connection.Open();

                rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                rowAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return rowAffected > 0;
        }
        public static bool IsApplicationExist(int ApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT Found=1 FROM Applications WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}
