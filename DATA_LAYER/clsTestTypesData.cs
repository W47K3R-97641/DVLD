using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_LAYER
{
    public struct stTestTypeInfo
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Fees { get; set; }
    }
    public class clsTestTypesData
    {
        private static stTestTypeInfo _ConvertDataRecordToStTestTypeInfo(IDataRecord record)
        {
            return new stTestTypeInfo
            {
                ID = (int)record["TestTypeId"],
                Title = (string)record["TestTypeTitle"],
                Description = (string)record["TestTypeDescription"],
                Fees = Convert.ToDecimal(record["TestTypeFees"])
            };
        }
        
        public static DataTable GetAllTestTypes()
        {
            DataTable table = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "select * from TestTypes";

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
        public static stTestTypeInfo? GetTestTypeByID(int testTypeID)
        {
            stTestTypeInfo? testTypeInfo = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select * from TestTypes
                             where TestTypeID = @testTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@testTypeID", testTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) 
                {
                    testTypeInfo = _ConvertDataRecordToStTestTypeInfo((IDataRecord)reader);
                }

                reader.Close();
            }
            catch
            {
                testTypeInfo = null;
            }
            finally
            {
                connection.Close();
            }

            return testTypeInfo;


        }

        public static int AddNew(string title, string description, decimal fees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"insert into TestTypes 
                             values(@title, @description, @fees)
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@title", title);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@fees", fees);
            int newTestTypeID = -1;
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    newTestTypeID = insertedID;
                }
            }
            catch
            {
                newTestTypeID = -1;
            }
            finally
            {
                connection.Close();
            }

            return newTestTypeID;

        }

        public static bool Update(int ID, string title, string description, decimal fees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update TestTypes  
                            set TestTypeTitle = @Title,
                                TestTypeDescription = @TestTypeDescription
                                TestTypeFees = @TestTypeFees
                                where TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeTitle", title);
            command.Parameters.AddWithValue("@TestTypeDescription", description);
            command.Parameters.AddWithValue("@TestTypeFees", fees);
            command.Parameters.AddWithValue("@TestTypeID", ID);

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
