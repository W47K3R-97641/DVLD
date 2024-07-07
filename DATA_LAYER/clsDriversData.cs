using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATA_LAYER
{
    public struct stDriver
    {
        public int DriverID;
        public int PersonID;
        public int CreatedByUserID;
        public DateTime CreatedDate;
    }
   
    public class clsDriversData
    {

        public static DataTable GetAllDrivers()
        {
            DataTable table = new DataTable();
            
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "select * from Drivers_View";

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
            {
                connection.Close();
            }

            return table;
        }

        private static stDriver _ConvertRecordToStDriver(IDataRecord record)
        {
            return new stDriver
            {
                DriverID = (int)record["DriverID"],
                PersonID = (int)record["PersonID"],
                CreatedDate = (DateTime)record["CreatedDate"],
                CreatedByUserID = (int)record["CreatedByUserID"]
             };
        }
        public static stDriver? GetDriverByID(int driverID)
        {
            stDriver? driver = null;

            

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select * from Drivers where DriverID = @driverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@driverID", driverID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    driver = _ConvertRecordToStDriver((IDataRecord)reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                driver = null;
            }
            finally
            {
                connection.Close();
            }

            return driver;
        }

        public static stDriver? GetDriverByPersonID(int personID)
        {
            stDriver? driver = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select * from Drivers 
                             where PersonID = @personID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personID", personID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    driver = _ConvertRecordToStDriver((IDataRecord)reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                driver = null;
            }
            finally
            {
                connection.Close();
            }

            return driver;
        }

        public static int AddNewDriver(int personID, int createdByUserID, DateTime createdDate)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"insert into Drivers values(@driverID, @createdByUserID, @createdDate);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@driverID", personID);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
            command.Parameters.AddWithValue("@createdDate", createdDate);

            int driverID = -1;
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (int.TryParse(result.ToString(), out int insertedID) && result != null)
                {
                    driverID = insertedID;
                }
            }
            catch(Exception ex)
            {
                driverID = -1;
            }
            finally
            {
                connection.Close();
            }

            return driverID;


        }

        public static bool UpdateDriver(int driverID, int personID, int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Drivers  
                            set PersonID = @driverID,
                                CreatedByUserID = @createdByUserID
                                where DriverID = @driverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@driverID", personID);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
            command.Parameters.AddWithValue("@driverID", driverID);

           

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
            { connection.Close(); }

            return rowAffected > 0;
        }
    }
}
