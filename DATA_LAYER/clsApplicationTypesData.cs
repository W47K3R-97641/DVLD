using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_LAYER
{
    public struct stAppTypeInfo
    {
        public int ID;
        public string Title;
        public decimal Fees;
    }
    public class clsApplicationTypesData
    {
        public static DataTable GetAllApplicationType()
        {
            DataTable table = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "select * from ApplicationTypes";

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
        public static bool Update(int appID, string applicationTypeTitle,decimal applicationFees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update ApplicationTypes  
                            set ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                                where ApplicationTypeID = @applicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@applicationTypeID", appID);
            command.Parameters.AddWithValue("@Title", applicationTypeTitle);
            command.Parameters.AddWithValue("@Fees", applicationFees);

            int rowAffected = -1;

            try
            {
                connection.Open();

                rowAffected = command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                rowAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return rowAffected > 0;
        }
        public static int AddNew(string applicationTypeTitle, decimal applicationFees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"insert into ApplicationTypes 
                             values(@Title, @Fees)
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Title", applicationTypeTitle);
            command.Parameters.AddWithValue("@Fees", applicationFees);

            int applicationTypeID = -1;

            try
            {
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    applicationTypeID = insertedID;
            }
            catch { applicationTypeID = -1; }
            finally { connection.Close(); }

            return applicationTypeID;

        }
        private static stAppTypeInfo _ConvertDataRecordToStAppTypeInfo(IDataRecord record)
        {
            return new stAppTypeInfo
            {
                ID    =  (int)record["ApplicationTypeID"],
                Title =  (string)record["ApplicationTypeTitle"],
                Fees  =  (decimal)record["ApplicationFees"]
            };

        }
        public static stAppTypeInfo? GetApplicationTypeByID(int appID)
        {
            stAppTypeInfo? appTpInfo = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID", appID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    appTpInfo=_ConvertDataRecordToStAppTypeInfo((IDataRecord)reader);  
                }
                reader.Close();

            }
            catch
            {
                appTpInfo = null;
            }
            finally
            {
                connection.Close();
            }

            return appTpInfo;
        }
    }
}
