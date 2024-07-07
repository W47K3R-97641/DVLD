using DATA_LAYER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC_LAYER
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestAppointmentID {  get; set; }
        public int LocalDrivingLicenseAppID { get; set; }
        public clsTestTypes.enTestType TestTypeID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { set; get; }
        public bool IsLocked { set; get; }
        public int RetakeTestApplicationID { set; get; }
        public clsApplication RetakeTestAppInfo { set; get; }
        public int TestID
        {
            get { return clsTestAppointmentData.GetTestID(this.TestAppointmentID); }

        }
        public clsTestAppointment()

        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestTypes.enTestType.VisionTest;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.RetakeTestApplicationID = -1;
            Mode = enMode.AddNew;

        }

        public clsTestAppointment(int TestAppointmentID, clsTestTypes.enTestType TestTypeID,
           int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees,
           int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)

        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseAppID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            this.RetakeTestAppInfo = clsApplication.FindBaseApplication(RetakeTestApplicationID);
            Mode = enMode.Update;
        }

        public clsTestAppointment(stTestAppointmentInfo testAppointmentInfo)
        {
            this.TestAppointmentID = testAppointmentInfo.TestAppointmentID;
            this.TestTypeID = (clsTestTypes.enTestType)testAppointmentInfo.TestTypeID;
            this.LocalDrivingLicenseAppID = testAppointmentInfo.LocalDrivingLicenseAppID;
            this.AppointmentDate = testAppointmentInfo.AppointmentDate;
            this.PaidFees = testAppointmentInfo.PaidFees;
            this.CreatedByUserID = testAppointmentInfo.CreatedByUserID;
            this.IsLocked = testAppointmentInfo.IsLocked;
            this.RetakeTestApplicationID = testAppointmentInfo.RetakeTestApplicationID;
            this.RetakeTestAppInfo = clsApplication.FindBaseApplication(RetakeTestApplicationID);
            Mode = enMode.Update;
        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();
        }

        public static clsTestAppointment Find(int TestAppointmentID)
        {
            stTestAppointmentInfo? testAppointmentInfo = clsTestAppointmentData.GetTestAppointmentInfoByID(TestAppointmentID);

            return testAppointmentInfo == null ? null : new clsTestAppointment(testAppointmentInfo.Value);
        }

        public static clsTestAppointment GetLastTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            stTestAppointmentInfo? testAppointmentInfo = clsTestAppointmentData.GetLastTestAppointment(LocalDrivingLicenseApplicationID, TestTypeID);

            return testAppointmentInfo == null ? null : new clsTestAppointment(testAppointmentInfo.Value);
        }

        public DataTable GetApplicationTestAppointmentsPerTestType(clsTestTypes.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(this.LocalDrivingLicenseAppID, (int)TestTypeID);

        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, clsTestTypes.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);

        }

        private bool _AddNewTestAppointment()
        {
            //call DataAccess Layer 

            this.TestAppointmentID = clsTestAppointmentData.AddNewTestAppointment((int)this.TestTypeID, this.LocalDrivingLicenseAppID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.RetakeTestApplicationID);

            return (this.TestAppointmentID != -1);
        }
        private bool _UpdateTestAppointment()
        {
            //call DataAccess Layer 
            
            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, (int)this.TestTypeID, this.LocalDrivingLicenseAppID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestAppointment();

            }

            return false;
        }
    }
}
