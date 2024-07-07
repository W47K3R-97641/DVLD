using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DATA_LAYER.clsPeopleData;

namespace DATA_LAYER
{
    public struct stUserInfo
    {
        public int userId;
        public int personId;
        public string username;
        public string password;
        public bool isActive;
    }
    public class clsUsersData
    { 
        public static DataTable GetAllUsers()
        {
            DataTable table = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select UserID, Users.PersonID, 
                            FullName = Firstname + ' ' + ISNULL(SecondName,'') + ' ' + ISNULL(ThirdName,'') + ' ' + LastName,
                            Users.UserName ,Users.IsActive
                            from Users
                            inner join People on Users.PersonID = People.PersonID;";

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
        private static stUserInfo _ConvertDataReaderToStUserInfo(SqlDataReader row)
        {
            stUserInfo userInfo = new stUserInfo();

            userInfo.userId =   (int)row["UserID"];
            userInfo.personId = (int)row["PersonID"];
            userInfo.username = (string)row["UserName"];
            userInfo.password = (string)row["Password"];
            userInfo.isActive = (bool)  row["IsActive"];

            return userInfo;
        }
        public static stUserInfo? GetUserById(int userId)
        {
            stUserInfo? user = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select * from Users
                             where UserID = @userID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@userID", userId);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user = _ConvertDataReaderToStUserInfo(reader);
                    
                }
            }
            catch
            {
                user = null;
            }
            finally
            {
                connection.Close();
            }

            return user;
        }
        public static stUserInfo? GetUserByPersonId(int personID)
        {
            stUserInfo? user = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select * from Users
                             where PersonID = @personID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personID", personID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user = _ConvertDataReaderToStUserInfo(reader);

                }

                reader.Close();
            }
            catch
            {
                user = null;
            }
            finally
            {
                connection.Close();
            }

            return user;
        }
        public static stUserInfo? GetUserInfoByUsernameAndPassword(string username, string password)
        {
            stUserInfo? user = null;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select * from Users
                             where UserName = @username and Password = @password";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);


            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user = _ConvertDataReaderToStUserInfo(reader);

                }
            }
            catch
            {
                user = null;
            }
            finally
            {
                connection.Close();
            }

            return user;
        }
        public static int AddNewUser(int personID, string username, string password, bool isActive)
        {
            int userID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"insert into Users values(@personID, @username, @password, @isActive);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@isActive", isActive);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (int.TryParse(result.ToString(), out int insertedID) && result != null)
                {
                    userID = insertedID;
                }
            }
            catch(Exception ex)
            {
                userID = -1;
            }
            finally
            {
                connection.Close();
            }

            return userID;

        }
        public static bool UpdateUser(int userID, int personID, string username, string password, bool isActive)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            
            string updateStatement = @"UPDATE Users
                         SET UserName = @username, 
                         Password = @password, 
                         IsActive = @isActive,
                         PersonID = @personID
                         WHERE UserID = @userID";

            SqlCommand command = new SqlCommand(updateStatement, connection);

            command.Parameters.AddWithValue("@userID", userID);
            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@isActive", isActive);

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
        public static bool DeleteUser(int userID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"delete from Users
                             where UserID = @userID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@userID", userID);
            int rowaffected = 0;
            try
            {
                connection.Open();
                rowaffected = command.ExecuteNonQuery();
            }
            catch
            {
                rowaffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return rowaffected > 0;
        }
        public static bool IsExistUserID(int userID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found=1 from Users 
                             where UserID = @userID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@userID", userID);

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

        public static bool IsExistUser(string username)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found=1 from Users 
                             where UserName = @username";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@username", username);

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
        public static bool IsExistPersonID(int personID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found=1 from Users 
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
    }
}
