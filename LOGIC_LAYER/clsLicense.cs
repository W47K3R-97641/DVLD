using DATA_LAYER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LOGIC_LAYER.clsLicense;
using static System.Net.Mime.MediaTypeNames;

namespace LOGIC_LAYER
{
    public class clsLicense
    {
        
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };
        
        public clsLicenseClass LicenseClassInfo;
        public clsDriver DriverInfo { set; get; }

        public int LicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int CreatedByUserID { set; get; }
        public int LicenseClassID 
        {
            set
            {
                LicenseClassInfo = new clsLicenseClass();
                LicenseClassInfo.LicenseClassID = value;
            }
            get
            {
                return LicenseClassInfo.LicenseClassID;
            }
        }
        public int DriverID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public clsDetainedLicense DetainedInfo { set; get; }
        public bool IsDetained
        {
            get { return clsDetainedLicense.IsLicenseDetained(this.LicenseID); }
        }

        public decimal PaidFees { set; get; }
        public bool IsActive { set; get; }
        public enIssueReason IssueReason { set; get; }
        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(this.IssueReason);
            }
        }
        public string Notes { set; get; }

        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClassID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        private clsLicense(int ID, int applicationID, int driverID, int licenseID, DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, bool isActive, enIssueReason issueReason, int createdByUserID)
        {
            this.LicenseID = ID;
            this.ApplicationID = applicationID;
            this.DriverID = driverID;
            this.LicenseClassID = licenseID;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.Notes = notes;
            this.PaidFees = paidFees;
            this.IsActive = isActive;
            this.IssueReason = issueReason;
            this.CreatedByUserID = createdByUserID;

            this.DriverInfo = clsDriver.GetDriverByID(this.DriverID);
            this.LicenseClassInfo = clsLicenseClass.Find(this.LicenseClassID);
            this.DetainedInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);
            Mode = enMode.Update;
        }

        private clsLicense(stLicenseInfo licenseInfo)
        {
            this.LicenseID       = licenseInfo.LicenseID;
            this.ApplicationID   = licenseInfo.ApplicationID;
            this.DriverID        = licenseInfo.DriverID;
            this.LicenseClassID  = licenseInfo.LicenseClass;
            this.IssueDate       = licenseInfo.IssueDate;
            this.ExpirationDate  = licenseInfo.ExpirationDate;
            this.Notes           = licenseInfo.Notes;
            this.PaidFees        = licenseInfo.PaidFees;
            this.IsActive        = licenseInfo.IsActive;
            this.IssueReason     = (enIssueReason)licenseInfo.IssueReason;
            this.CreatedByUserID = licenseInfo.CreatedByUserID;

            this.DriverInfo = clsDriver.GetDriverByID(this.DriverID);
            this.LicenseClassInfo = clsLicenseClass.Find(this.LicenseClassID);
            this.DetainedInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);

            Mode = enMode.Update;
        }

        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }
        public static clsLicense Find(int licenseID)
        {
            stLicenseInfo? licenseInfo = clsLicenseData.GetLicenseInfoByID(licenseID);

            return (licenseInfo.HasValue) ? new clsLicense(licenseInfo.Value) : null;
        }
        public static int GetActiveLicenseIDByPersonID(int personID, int licenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(personID, licenseClassID);
        }

        public bool IsLicenseExpired()
        {
            return this.ExpirationDate < DateTime.Now;
        }

        public bool DeactivateCurrentLicense()
        {
            return clsLicenseData.DeactivateLicense(this.LicenseID);
        }
        public static string GetIssueReasonText(enIssueReason IssueReason)
        {

            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }
        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(ApplicationID, DriverID, LicenseClassID, IssueDate,ExpirationDate,Notes, PaidFees, IsActive, (int)IssueReason, CreatedByUserID);

            if (this.LicenseID != -1)
            {
                this.DriverInfo = clsDriver.GetDriverByID(DriverID);
                this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
                return true;
            }
            return false;
        }
        private bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (int)IssueReason, CreatedByUserID);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else return false;
                case enMode.Update:
                    return _UpdateLicense();
            }

            return false;
        }

        public static bool IsLicenseExistByPersonID(int personID, int licenseClassID)
        {
            return (GetActiveLicenseIDByPersonID(personID, licenseClassID) != -1);
        }
        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }

        public DataTable GetDriverLicenses()
        {
            return clsLicenseData.GetDriverLicenses(this.DriverID);
        }

        public clsLicense Replace(enIssueReason IssueReason, int CreatedByUserID)
        {
            //First Create Applicaiton 
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (IssueReason == enIssueReason.DamagedReplacement) ?
                (int)enApplicationType.ReplaceDamagedDrivingLicense :
                (int)enApplicationType.ReplaceLostDrivingLicense;
            Application.ApplicationStatus = enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).Fees;
            Application.CreatedByUserID = CreatedByUserID;


            if (!Application.Save())
                return null;

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassInfo = this.LicenseClassInfo;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            NewLicense.PaidFees = 0;// no fees for the license because it's a replacement.
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save())
                return null;

            //we need to deactivate the old License.
            this.DeactivateCurrentLicense();

            return NewLicense;


        }

        public clsLicense RenewLicense(string Notes, int CreatedByUserID)
        {
            //First Create Applicaiton 
            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)enApplicationType.RenewDrivingLicense).Fees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
                return null;

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassInfo = this.LicenseClassInfo;
            NewLicense.IssueDate = DateTime.Now;

            int DefaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;

            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save())
                return null;

            //we need to deactivate the old License.
            DeactivateCurrentLicense();

            return NewLicense;

        }

        public int Detain(decimal FineFees, int byUserID)
        {
            clsDetainedLicense detainedLicense = new clsDetainedLicense();
            detainedLicense.LicenseID = this.LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = Convert.ToDecimal(FineFees);
            detainedLicense.CreatedByUserID = byUserID;

            if (!detainedLicense.Save())
            {

                return -1;
            }

            return detainedLicense.DetainID;
        }

        public bool ReleaseDetainedLicens(int ReleasedByUserID, ref int ApplicationID)
        {

            //First Create Applicaiton 
            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)enApplicationType.ReleaseDetainedDrivingLicsense;
            Application.ApplicationStatus = enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)enApplicationType.ReleaseDetainedDrivingLicsense).Fees;
            Application.CreatedByUserID = ReleasedByUserID;

            if (!Application.Save())
            {
                ApplicationID = -1;
                return false;
            }

            ApplicationID = Application.ApplicationID;

            
            return this.DetainedInfo.ReleaseDetainedLicense(ReleasedByUserID, Application.ApplicationID);

        }

    }
}
