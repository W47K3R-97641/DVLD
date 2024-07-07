using DATA_LAYER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC_LAYER
{
    public class clsInternationalLicense:clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsDriver DriverInfo;
        public int InternationalLicenseID { set; get; }
        public int DriverID 
        {
            set
            {

                DriverInfo = clsDriver.GetDriverByID(value);

                if (DriverInfo == null)
                {
                    DriverInfo = new clsDriver();
                    DriverInfo.DriverID = value;
                }
            }

            get
            {
                return DriverInfo.DriverID;
            }
        }
        public int IssuedUsingLocalLicenseID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }
        public clsInternationalLicense()

        {
            //here we set the applicaiton type to New International License.
            this.ApplicationTypeID = (int)enApplicationType.NewInternationalLicense;

            this.InternationalLicenseID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;

            this.IsActive = true;


            Mode = enMode.AddNew;

        }

        public clsInternationalLicense(int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             decimal PaidFees, int CreatedByUserID,
             int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive)

        {
            //this is for the base clase
            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            base.ApplicationTypeID = (int)enApplicationType.NewInternationalLicense;
            base.ApplicationStatus = ApplicationStatus;
            base.LastStatusDate = LastStatusDate;
            base.PaidFees = PaidFees;
            base.CreatedByUserID = CreatedByUserID;

            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;

            this.DriverInfo = clsDriver.GetDriverByID(this.DriverID);

            Mode = enMode.Update;
        }
        private clsInternationalLicense(stInternationalLicenseInfo internationalLicenseInfo)
        {
            this.InternationalLicenseID = internationalLicenseInfo.InternationalLicenseID;
            this.DriverID = internationalLicenseInfo.DriverID;
            this.IssuedUsingLocalLicenseID = internationalLicenseInfo.IssuedUsingLocalLicenseID;
            this.IssueDate = internationalLicenseInfo.IssueDate;
            this.ExpirationDate = internationalLicenseInfo.ExpirationDate;
            this.IsActive = internationalLicenseInfo.IsActive;
            

            clsApplication BaseApplication = clsApplication.FindBaseApplication(internationalLicenseInfo.ApplicationID);

            if (BaseApplication != null)
            {
                //this is for the base clase
                
                base.ApplicantPersonID = BaseApplication.ApplicantPersonID;
                base.ApplicationDate = BaseApplication.ApplicationDate;
                base.ApplicationTypeID = (int)enApplicationType.NewInternationalLicense;
                base.ApplicationStatus = BaseApplication.ApplicationStatus;
                base.LastStatusDate = BaseApplication.LastStatusDate;
                base.PaidFees = BaseApplication.PaidFees;
                base.CreatedByUserID = BaseApplication.CreatedByUserID;
                base.ApplicationID = BaseApplication.ApplicationID;

                Mode = enMode.Update;

            }

        }


        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            stInternationalLicenseInfo? internationalLicenseInfo = 
                clsInternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID);

            return internationalLicenseInfo.HasValue ? new clsInternationalLicense(internationalLicenseInfo.Value) : null;
        }
        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {

            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);

        }
        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }
        private bool _AddNewInternationalLicense()
        {
            //call DataAccess Layer 

            this.InternationalLicenseID =
                clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID,
               this.IssueDate, this.ExpirationDate,
               this.IsActive, this.CreatedByUserID);


            return (this.InternationalLicenseID != -1);
        }
        private bool _UpdateInternationalLicense()
        {
            //call DataAccess Layer 

            return clsInternationalLicenseData.UpdateInternationalLicense(
                this.InternationalLicenseID, this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID,
               this.IssueDate, this.ExpirationDate,
               this.IsActive, this.CreatedByUserID);
        }
        public bool Save()
        {

            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode = (clsApplication.enMode)Mode;
            if (!base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateInternationalLicense();

            }

            return false;
        }
    }
}
