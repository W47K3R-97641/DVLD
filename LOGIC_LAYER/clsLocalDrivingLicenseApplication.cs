using DATA_LAYER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC_LAYER
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        

        public enMode mode = enMode.AddNew;

        

        public clsLicenseClass LicenseClassInfo;

        public int LocalDrivingLicenseApplicationID { set; get; }
        public int LicenseClassID { set; get; }


        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;

            this.mode = enMode.AddNew;
            base.Mode = enMode.AddNew;
        }

        private clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int ApplicantPersonID,
           DateTime ApplicationDate, int ApplicationTypeID,
            enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
            decimal PaidFees, int CreatedByUserID, int LicenseClassID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID; ;
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = (int)ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
           
            this.mode = enMode.Update;
            base.Mode = enMode.Update;
        }

        private clsLocalDrivingLicenseApplication(stDLAppInfo dLAppInfo) 
        {
            clsApplication application = clsApplication.FindBaseApplication(dLAppInfo.ApplicationID);
            
            base.ApplicantPersonID                = application.ApplicantPersonID;
            base.ApplicationDate                  = application.ApplicationDate;
            base.ApplicationTypeID                = (int)application.ApplicationTypeID;
            base.ApplicationStatus                = application.ApplicationStatus;
            base.LastStatusDate                   = application.LastStatusDate;
            base.PaidFees                         = application.PaidFees;
            base.CreatedByUserID                  = application.CreatedByUserID;

            
            
            this.LocalDrivingLicenseApplicationID = dLAppInfo.LocalDrivingLicenseApplicationID;
            this.ApplicationID                    = dLAppInfo.ApplicationID;
            this.LicenseClassID                   = dLAppInfo.LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);


            this.mode = enMode.Update;
            base.Mode = enMode.Update;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }

        public static clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {
            stDLAppInfo? dLAppInfo = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByApplicationID(ApplicationID);

            return dLAppInfo != null ? new clsLocalDrivingLicenseApplication(dLAppInfo.Value): null;
        }

        public static clsLocalDrivingLicenseApplication FindByLocalDrivingAppLicenseID(int LocalDrivingLicenseApplicationID)
        {
            stDLAppInfo? dLAppInfo = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID(LocalDrivingLicenseApplicationID);

            return dLAppInfo != null ? new clsLocalDrivingLicenseApplication(dLAppInfo.Value) : null;
        }
       
        private bool _AddNewLocalDrivingLicenseApplication()
        {
            //call DataAccess Layer 

            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(ApplicationID, LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            //call DataAccess Layer 

            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);

        }

        public override bool Save()
        {

            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode = (clsApplication.enMode)this.mode;
            if (!base.Save())
                return false;

            //After we save the main application now we save the sub application.
            switch (this.mode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {

                        this.mode = enMode.Update;
                        return true;
                    }
                        return false;
                    
                    
                case enMode.Update:

                    return _UpdateLocalDrivingLicenseApplication();

            }

            return false;
        }
        public override bool Delete()
        {
            bool IsLocalDrivingApplicationDeleted = clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID); ;
            bool IsBaseApplicationDeleted = base.Delete();

            if (!IsLocalDrivingApplicationDeleted)
                return false;

            return IsBaseApplicationDeleted;
        }

        public byte GetPassedTestCount()
        {
            return clsTest.GetPassedTestCount(this.LocalDrivingLicenseApplicationID);
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public int GetActiveLicenseID()
        {//this will get the license id that belongs to this application
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }


        public  bool IsThereAnActiveScheduledTest(clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID,clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public clsTest GetLastTestPerTestType(clsTestTypes.enTestType TestTypeID)
        {
            return clsTest.FindLastTestPerPersonAndLicenseClass(this.ApplicantPersonID, this.LicenseClassID, TestTypeID);
        }

        public bool DoesAttendTestType(clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool DoesPassTestType(clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public byte TotalTrialsPerTest(clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, clsTestTypes.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool IsLicenseIssued()
        {
            return (GetActiveLicenseID() != -1);
        }

        public int IssueLicenseForTheFirtTime(string Notes, int CreatedByUserID)
        {
            clsDriver Driver = clsDriver.GetDriverByPersonID(this.ApplicantPersonID);

            if (Driver == null)
            {
                Driver = new clsDriver();

                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                
                if (Driver.Save() == false)
                {
                    return -1;
                }
            }

            clsLicense License = new clsLicense();
            License.DriverID = Driver.DriverID;
            License.DriverInfo = Driver;
            License.ApplicationID = this.ApplicationID;
            License.CreatedByUserID = CreatedByUserID;
            License.LicenseClassID = LicenseClassID;
            License.LicenseClassInfo = LicenseClassInfo;
            License.IsActive = true;
            License.Notes = Notes;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.PaidFees = this.LicenseClassInfo.ClassFees;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);

            if (License.Save())
            {
                this.SetComplete();
                return License.LicenseID;
            }
            else
            {
                return -1;
            }
        }

    }
}
