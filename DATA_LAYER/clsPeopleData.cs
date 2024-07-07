using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;


namespace DATA_LAYER
{
    public class clsPeopleData
    {
        public struct stpersonInfo
        {
            public int PersonID;
            public string FirstName;
            public string SecondName;
            public string ThirdName;
            public string LastName;
            public string NationalNo;
            public string Address;
            public string Phone;
            public string Email;
            public string ImagePath;
            public byte Gendor;
            public DateTime DateOfBirth;
            public int NationalityCountryID;
        }
        static public DataTable GetAllPeople()
        {
            DataTable table = new DataTable();
            
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select PersonID,
                            NationalNo,
                            FirstName,
                            SecondName,
                            ThirdName,
                            LastName, 
                            CASE
                                WHEN Gendor = 0 THEN 'Male'
                                WHEN Gendor = 1 THEN 'Female'
                                ELSE null
                            END as GendorCaption,
                            DateOfBirth,
                            CountryName,
                            Phone,
                            Email
                            from People
                            join Countries on NationalityCountryID = Countries.CountryID";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    table.Load(reader);

                reader.Close();
            }
            catch
            {
                table = null;
            }
            finally
            {
                connection.Close();
            }

            return table;
        }

        public static int AddNewPerson(string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gendor, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            int personID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"insert into People values(@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gendor, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            
            if (ThirdName != "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            if (Email != "" && Email != null)
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    personID = insertedID;
                }
            }
            catch 
            {
                personID = -1;
            }
            finally { connection.Close(); }

            return personID;
        }

        public static bool UpdatePerson(int PersonID,string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gendor, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);


            string updateStatement = @"UPDATE People
                         SET FirstName = @FirstName, 
                         SecondName = @SecondName, 
                         ThirdName = @ThirdName, 
                         LastName = @LastName, 
                         NationalNo = @NationalNo, 
                         DateOfBirth = @DateOfBirth, 
                         Gendor = @Gendor,
                         Address = @Address, 
                         Phone = @Phone, 
                         Email = @Email, 
                         NationalityCountryID = @NationalityCountryID,
                         ImagePath = @ImagePath
                         WHERE PersonID = @PersonID"
            ;



            SqlCommand command = new SqlCommand(updateStatement, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ThirdName != "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            if (Email != "" && Email != null)
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

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

        static private stpersonInfo _ConvertDataReaderToStPersonInfo(SqlDataReader row)
        {
            stpersonInfo stpersonInfo = new stpersonInfo();

            stpersonInfo.PersonID  = (int)row["PersonId"];
            stpersonInfo.FirstName = (string)row["FirstName"];
            stpersonInfo.SecondName = (string)row["SecondName"] ;
            stpersonInfo.LastName = (string)row["LastName"];
            stpersonInfo.DateOfBirth = (DateTime)row["DateOfBirth"];
            stpersonInfo.Phone   = (string)row["Phone"];
            stpersonInfo.Address = (string)row["Address"];
            stpersonInfo.Gendor  = (byte)row["Gendor"];
            stpersonInfo.NationalityCountryID = (int)row["NationalityCountryID"];
            stpersonInfo.NationalNo = (string)row["NationalNo"];

            // Thoes Allow Null
            stpersonInfo.ThirdName = row["ThirdName"] != DBNull.Value ? (string)row["ThirdName"] : "";
            stpersonInfo.Email     = row["Email"]     != DBNull.Value ? (string)row["Email"]     : "";
            stpersonInfo.ImagePath = row["ImagePath"] != DBNull.Value ? (string)row["ImagePath"] : "";

            return stpersonInfo;
        }

        public static stpersonInfo? GetPersonInfoByID(int personID)
        {
            stpersonInfo? person = null;


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID",personID);

            
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    person = _ConvertDataReaderToStPersonInfo(reader);
                }
                 reader.Close();
            }
            catch
            {
                person = null;
            }
            finally
            {
                connection.Close();

            }

            return person;


        }

        public static stpersonInfo? GetPersonInfoByNationalNo(string nationalNo)
        {
            stpersonInfo? person = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM People WHERE NationalNo = @nationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@nationalNo", nationalNo);


            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    person = _ConvertDataReaderToStPersonInfo(reader);
                }
                reader.Close();
            }
            catch
            {
                person = null;
            }
            finally
            {
                connection.Close();

            }

            return person;
        }

        public static bool DeletePerson(int personID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Delete from People
                              where PersonID = @personID";
            
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personID", personID);

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

        public static bool IsExist(int personID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found=1 from People 
                             where PersonID = @personID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personID", personID);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;

        }

        public static bool IsExist(string nationalNo)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found=1 from People 
                             where NationalNo = @nationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@nationalNo", nationalNo);

            bool isFound = false;
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch
            {
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
