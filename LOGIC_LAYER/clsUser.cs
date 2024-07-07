using DATA_LAYER;
using LOGIC_LAYER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DATA_LAYER.clsPeopleData;

namespace LOGIC_LAYER
{
    public class clsUser
    {
        private enum enMode : byte { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;

        public int userID { get; set; }
        public int personID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool isActive { get; set; }

        public clsUser()
        {
            userID = -1;
            personID = -1;
            username = string.Empty;
            password = string.Empty;

            Mode = enMode.AddNew;
        }
        private clsUser(int userID, int personID, string username, string password, bool isActive)
        {
            this.userID = userID;
            this.personID = personID;
            this.username = username;
            this.password = password;
            this.isActive = isActive;
            Mode = enMode.Update;
        }
        private clsUser(stUserInfo userInfo)
        {
            this.userID   = userInfo.userId;
            this.personID = userInfo.personId;
            this.username = userInfo.username;
            this.password = userInfo.password;
            this.isActive = userInfo.isActive;
            Mode = enMode.Update;
        }


        public static DataTable GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }
        public static clsUser GetUserById(int userId)
        {
            stUserInfo? userInfo = clsUsersData.GetUserById(userId);

            return userInfo.HasValue ? new clsUser(userInfo.Value) : null;
        }
        public static clsUser GetUserByPersonId(int personID)
        {
            stUserInfo? userInfo = clsUsersData.GetUserByPersonId(personID);

            return userInfo.HasValue ? new clsUser(userInfo.Value) : null;
        }
        public static clsUser GetUserInfoByUsernameAndPassword(string username, string password)
        {
            stUserInfo? userInfo = clsUsersData.GetUserInfoByUsernameAndPassword(username, password);

            return userInfo.HasValue ? new clsUser(userInfo.Value) : null;
        }
        public static bool DeleteUser(int userID)
        {
            return clsUsersData.DeleteUser(userID);
        }
        public static bool IsExistUserID(int userID)
        {
            return clsUsersData.IsExistUserID(userID);
        }

        public static bool IsExistUser(string username)
        {
            return clsUsersData.IsExistUser(username);
        }
        public static bool IsExistPersonID(int personID)
        {
            return clsUsersData.IsExistPersonID(personID);
        }
        private bool _AddNew()
        {
            this.userID = clsUsersData.AddNewUser(personID, username, password, isActive);

            return this.userID != -1;
        }
        private bool _Update()
        {
            return clsUsersData.UpdateUser(userID, personID, username, password, isActive);
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _Update();

            }

            return false;
        }
    }
}


