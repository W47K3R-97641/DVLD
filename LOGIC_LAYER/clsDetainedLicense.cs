using DATA_LAYER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC_LAYER
{
    public class clsDetainedLicense
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int DetainID { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }
        public decimal FineFees { set; get; }
        public int CreatedByUserID { set; get; }
        public clsUser CreatedByUserInfo { set; get; }
        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }
        public int ReleasedByUserID { set; get; }
        public clsUser ReleasedByUserInfo { set; get; }
        public int ReleaseApplicationID { set; get; }

        public clsDetainedLicense()

        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = 0;
            this.ReleaseApplicationID = -1;



            Mode = enMode.AddNew;

        }

        public clsDetainedLicense(int DetainID,
            int LicenseID, DateTime DetainDate,
            decimal FineFees, int CreatedByUserID,
            bool IsReleased, DateTime ReleaseDate,
            int ReleasedByUserID, int ReleaseApplicationID)

        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.GetUserById(this.CreatedByUserID);
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;
            this.ReleasedByUserInfo = clsUser.GetUserByPersonId(this.ReleasedByUserID);
            Mode = enMode.Update;
        }

        public clsDetainedLicense(stDetainedLicenseInfo detainedLicenseInfo)
        {
            this.DetainID = detainedLicenseInfo.DetainID;
            this.LicenseID = detainedLicenseInfo.LicenseID;
            this.DetainDate = detainedLicenseInfo.DetainDate;
            this.FineFees = detainedLicenseInfo.FineFees;
            this.CreatedByUserID = detainedLicenseInfo.CreatedByUserID;
            this.CreatedByUserInfo = clsUser.GetUserById(this.CreatedByUserID);
            this.IsReleased = detainedLicenseInfo.IsReleased;
            this.ReleaseDate = detainedLicenseInfo.ReleaseDate;
            this.ReleasedByUserID = detainedLicenseInfo.ReleasedByUserID;
            this.ReleaseApplicationID = detainedLicenseInfo.ReleaseApplicationID;
            this.ReleasedByUserInfo = clsUser.GetUserByPersonId(this.ReleasedByUserID);
            Mode = enMode.Update;
        }


        public static clsDetainedLicense Find(int DetainID)
        {
            stDetainedLicenseInfo? licenseInfo = clsDetainedLicenseData.GetDetainedLicenseInfoByID(DetainID);

            return licenseInfo == null ? null : new clsDetainedLicense(licenseInfo.Value);
        }

        public static clsDetainedLicense FindByLicenseID(int LicenseID)
        {
            stDetainedLicenseInfo? licenseInfo = clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(LicenseID);

            return licenseInfo == null ? null : new clsDetainedLicense(licenseInfo.Value);
        }

        public static bool IsLicenseDetained(int licenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(licenseID);
        }
        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();

        }

        private bool _AddNewDetainedLicense()
        {
            //call DataAccess Layer 

            this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense(
                this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);

            return (this.DetainID != -1);
        }

        private bool _UpdateDetainedLicense()
        {
            //call DataAccess Layer 

            return clsDetainedLicenseData.UpdateDetainedLicense(
                this.DetainID, this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDetainedLicense();

            }

            return false;
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            return clsDetainedLicenseData.ReleaseDetainedLicense(this.DetainID,
                   ReleasedByUserID, ReleaseApplicationID);
        }
    }
}
