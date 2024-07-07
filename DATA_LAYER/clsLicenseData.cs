using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATA_LAYER
{
    public struct stLicenseInfo
    {
        public int LicenseID            { set; get; }
        public int ApplicationID        { set; get; }
        public int CreatedByUserID      { set; get; }
        public int LicenseClass         { set; get; }
        public int DriverID             { set; get; }
        public DateTime IssueDate       { set; get; }
        public DateTime ExpirationDate  { set; get; }
        public decimal PaidFees         { set; get; }
        public bool IsActive            { set; get; }
        public byte IssueReason{ set; get; }
        public string Notes { set; get; }
    }
    public class clsLicenseData
    {
        public static DataTable GetAllLicenses()
        {
            DataTable table = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "select * from Licenses";

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

        private static stLicenseInfo _ConvertDataRecordToStLicenseInfo(IDataRecord record)
        {

            stLicenseInfo lp = new stLicenseInfo();

            lp.LicenseID = (int)record["LicenseID"];
            lp.ApplicationID = (int)record["ApplicationID"];
            lp.LicenseClass = (int)record["LicenseClass"];

            lp.CreatedByUserID = (int)record["CreatedByUserID"];
            lp.DriverID = (int)record["DriverID"];
            lp.IssueDate = (DateTime)record["IssueDate"];
            lp.ExpirationDate = (DateTime)record["ExpirationDate"];
            lp.PaidFees = Convert.ToDecimal(record["PaidFees"]);
            lp.IsActive = (bool)record["IsActive"];
            lp.IssueReason = (byte)record["IssueReason"];
            lp.Notes = (record["Notes"] == DBNull.Value) ? "" : (string)record["Notes"];

            return lp;

        }
        public static stLicenseInfo? GetLicenseInfoByID(int licenseID)
        {
            stLicenseInfo? LicenseInfo = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM Licenses WHERE LicenseID = @licenseID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseID", licenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    LicenseInfo = _ConvertDataRecordToStLicenseInfo((IDataRecord)reader);
                }
                reader.Close();

            }
            catch
            {
                LicenseInfo = null;
            }
            finally
            {
                connection.Close();
            }

            return LicenseInfo;
        }

        public static int AddNewLicense(int applicationID, int driverID, int licenseClassID, DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, bool isActive, int issueReason, int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"insert into Licenses 
                             values(@applicationID, @driverID, @licenseClassID, @issueDate, @expirationDate, @notes, @paidFees, @isActive, @issueReason, @createdByUserID)
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID",  applicationID);
            command.Parameters.AddWithValue("@driverID",       driverID);
            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);
            command.Parameters.AddWithValue("@issueDate",      issueDate);
            command.Parameters.AddWithValue("@expirationDate", expirationDate);
            command.Parameters.AddWithValue("@paidFees",       paidFees);
            command.Parameters.AddWithValue("@isActive",       isActive);
            command.Parameters.AddWithValue("@issueReason",    issueReason);
            command.Parameters.AddWithValue("@createdByUserID",createdByUserID);
            if (String.IsNullOrEmpty(notes))
                command.Parameters.AddWithValue("@notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@notes", notes);

            int LicenseID = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    LicenseID = insertedID;
            }
            catch { LicenseID = -1; }
            finally { connection.Close(); }

            return LicenseID;
        }

        public static bool UpdateLicense(int licenseID,int applicationID, int driverID, int licenseClassID, DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, bool isActive, int issueReason, int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE Licenses
                           SET ApplicationID=@applicationID,
                               DriverID = @driverID,
                               LicenseClass = @licenseClass,
                               IssueDate = @issueDate,
                               ExpirationDate = @expirationDate,
                               Notes = @notes,
                               PaidFees = @paidFees,
                               IsActive = @isActive,
                               IssueReason = @issueReason,
                               CreatedByUserID = @createdByUserID
                           WHERE licenseID=@licenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationID", applicationID);
            command.Parameters.AddWithValue("@driverID", driverID);
            command.Parameters.AddWithValue("@licenseClass", licenseClassID);
            command.Parameters.AddWithValue("@issueDate", issueDate);
            command.Parameters.AddWithValue("@expirationDate", expirationDate);
            command.Parameters.AddWithValue("@paidFees", paidFees);
            command.Parameters.AddWithValue("@isActive", isActive);
            command.Parameters.AddWithValue("@issueReason", issueReason);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
            command.Parameters.AddWithValue("@licenseID", licenseID);

            if (String.IsNullOrEmpty(notes))
                command.Parameters.AddWithValue("@notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@notes", notes);

            int rowAffected = -1;

            try
            {
                rowAffected = command.ExecuteNonQuery();
            }
            catch { rowAffected = 0; }
            finally { connection.Close(); }

            return (rowAffected > 0);
        }

        public static int GetActiveLicenseIDByPersonID(int personID, int licenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select Licenses.LicenseID from Licenses
                            join Drivers on Drivers.DriverID = Licenses.DriverID
                            where Licenses.LicenseClass = @licenseClassID 
                            AND Drivers.PersonID = @personID
                            And IsActive=1";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);
            command.Parameters.AddWithValue("@personID", personID);
            
            int LicenseID = -1;
            
            try
            {
                
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }

            }
            catch
            {
                LicenseID = -1;
            }
            finally
            {
                connection.Close();
            }

            return LicenseID;

        }

        public static bool DeactivateLicense(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE Licenses
                           SET IsActive = 0
                           WHERE licenseID=@licenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseID", LicenseID);

            int rowsAffected = 0;

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

        public static DataTable GetDriverLicenses(int DriverID)
        {

            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT     
                           Licenses.LicenseID,
                           ApplicationID,
		                   LicenseClasses.ClassName, Licenses.IssueDate, 
		                   Licenses.ExpirationDate, Licenses.IsActive
                           FROM Licenses INNER JOIN
                                LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where DriverID=@DriverID
                            Order By IsActive Desc, ExpirationDate Desc";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);

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
    }
}
