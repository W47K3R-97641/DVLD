using DATA_LAYER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC_LAYER
{
    public enum enApplicationStatus : byte { New = 1, Cancelled = 2, Completed = 3 };

    public enum enApplicationType : byte
    {
        NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
        ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
    };

    public class clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };

        public clsPerson ApplicantPersonInfo;
        public clsApplicationType ApplicationTypeInfo;
        public clsUser CreatedByUserInfo;

        public enMode Mode { get; set; }
        public int ApplicationID { get; set; }
        public int ApplicantPersonID
        {
            set
            {
                
                ApplicantPersonInfo = clsPerson.Find(value);

                if (ApplicantPersonInfo == null)
                {
                    ApplicantPersonInfo = new clsPerson();
                    ApplicantPersonInfo.PersonID = value;
                }
            }

            get
            {
                return ApplicantPersonInfo.PersonID;
            }
        }
        public string ApplicantFullName
        {
            get
            {
                return clsPerson.Find(ApplicantPersonID).FullName;
            }
        }
        public DateTime ApplicationDate { get; set; }
        
        public int ApplicationTypeID { set; get; }
        public enApplicationStatus ApplicationStatus;
        public int CreatedByUserID {set; get;}
        public string CreatedByUserName { set; get; }

        public DateTime LastStatusDate { set; get; }
        public decimal PaidFees { set; get; }
        public string StatusText
        {
            get
            {

                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }

        }

        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
           
            this.Mode = enMode.AddNew;
        }

        private clsApplication(int ApplicationID, int ApplicantPersonID,
           DateTime ApplicationDate, int ApplicationTypeID,
            enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
            decimal PaidFees, int CreatedByUserID)
        {
            this.ApplicationID     = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate   = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate    = LastStatusDate;
            this.PaidFees          = PaidFees;
            this.CreatedByUserID   = CreatedByUserID;
            this.CreatedByUserName = clsUser.GetUserById(CreatedByUserID).username;
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.ApplicantPersonInfo = clsPerson.Find(ApplicantPersonID);
            this.Mode = enMode.Update;
            
        }

        private clsApplication(stApplicationInfo applicationInfo)
        {
            this.ApplicationID     = applicationInfo.ApplicationID;
            this.ApplicantPersonID = applicationInfo.ApplicantPersonID;
            this.ApplicationDate   = applicationInfo.ApplicationDate;
            this.ApplicationTypeID = applicationInfo.ApplicationTypeID;
            this.ApplicationStatus = (enApplicationStatus)applicationInfo.ApplicationStatus;
            this.LastStatusDate    = applicationInfo.LastStatusDate;
            this.PaidFees          = applicationInfo.PaidFees;
            this.CreatedByUserID   = applicationInfo.CreatedByUserID;
            this.CreatedByUserName = clsUser.GetUserById(CreatedByUserID).username;
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.ApplicantPersonInfo = clsPerson.Find(ApplicantPersonID);

            this.Mode = enMode.Update;
        }
        
        public static clsApplication FindBaseApplication(int ApplicationID)
        {
            stApplicationInfo? applicationInfo = clsApplicationData.GetApplicationTypeByID(ApplicationID);

            return (applicationInfo == null) ? null : new clsApplication(applicationInfo.Value);
        }

        public bool Cancel()
        {
            return clsApplicationData.UpdateStatus(ApplicationID, (byte)enApplicationStatus.Cancelled);
        }

        public bool SetComplete()
        {
            return clsApplicationData.UpdateStatus(ApplicationID, (byte)enApplicationStatus.Completed);
        }


        private bool _AddNewApplication()
        {
            //call DataAccess Layer 

            this.ApplicationID = clsApplicationData.AddNew(
                this.ApplicantPersonID, this.ApplicationDate,
                this.ApplicationTypeID, (byte)this.ApplicationStatus,
                this.LastStatusDate, this.PaidFees, this.CreatedByUserID);


            this.CreatedByUserName = clsUser.GetUserById(CreatedByUserID).username;
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.ApplicantPersonInfo = clsPerson.Find(ApplicantPersonID);

            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            //call DataAccess Layer 
            this.CreatedByUserName = clsUser.GetUserById(CreatedByUserID).username;
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.ApplicantPersonInfo = clsPerson.Find(ApplicantPersonID);

            return clsApplicationData.Update(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate,
                this.ApplicationTypeID, (byte)this.ApplicationStatus,
                this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

        }

        public virtual bool Delete()
        {
            return clsApplicationData.DeleteApplication(this.ApplicationID); 
        }
        public virtual bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {

                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateApplication();

            }

            return false;
        }


        public static bool IsApplicationExist(int ApplicationID)
        {
           return clsApplicationData.IsApplicationExist(ApplicationID);
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID,int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(PersonID,ApplicationTypeID);
        }

        public  bool DoesPersonHaveActiveApplication( int ApplicationTypeID)
        {
            return DoesPersonHaveActiveApplication(this.ApplicantPersonID, ApplicationTypeID);
        }

        public static int GetActiveApplicationID(int PersonID, enApplicationType ApplicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(PersonID,(int) ApplicationTypeID);
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, enApplicationType ApplicationTypeID,int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID,LicenseClassID );
        }
       
        public  int GetActiveApplicationID(enApplicationType ApplicationTypeID)
        {
            return GetActiveApplicationID(this.ApplicantPersonID, ApplicationTypeID);
        }
    }
}
