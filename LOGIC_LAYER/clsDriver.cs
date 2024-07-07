using DATA_LAYER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC_LAYER
{
    public class clsDriver
    {

        public enum enMode : byte { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        private int personID;
        public clsPerson PersonInfo;
        public int DriverID { set; get; }
        public int PersonID 
        { 
            set { 
                personID = value;
                PersonInfo = clsPerson.Find(personID);
            }
            get { return personID; }
        }
        public int CreatedByUserID { set; get; }
        public DateTime CreatedDate { get; }

        public clsDriver()
        {
            this.DriverID        = -1;
            this.PersonID        = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate     = DateTime.Now;
            Mode = enMode.AddNew;
        }
        private clsDriver(int driverID, int personID,  int createdByUserID, DateTime createdDate)
        {
            this.DriverID        = driverID;
            this.PersonID        = personID;
            this.CreatedByUserID = createdByUserID;
            this.CreatedDate     = createdDate;
            this.PersonInfo      = clsPerson.Find(personID);
            Mode = enMode.Update;
        }
        private clsDriver(stDriver driverInfo)
        {
          
            this.DriverID        = driverInfo.DriverID;
            this.PersonID        = driverInfo.PersonID;
            this.CreatedByUserID = driverInfo.CreatedByUserID;
            this.CreatedDate     = driverInfo.CreatedDate;
            this.PersonInfo = clsPerson.Find(this.PersonID);
            Mode = enMode.Update;
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriversData.GetAllDrivers();
        }
        public static clsDriver GetDriverByID(int driverID)
        {
            stDriver? driverInfo = clsDriversData.GetDriverByID(driverID);
            return driverInfo.HasValue ? new clsDriver(driverInfo.Value) : null;
        }
        public static clsDriver GetDriverByPersonID(int PersonID)
        {
            stDriver? driverInfo = clsDriversData.GetDriverByPersonID(PersonID);
            return driverInfo.HasValue ? new clsDriver(driverInfo.Value) : null;
        }
        private bool _AddNewDriver()
        {
            this.DriverID = clsDriversData.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);

            return (DriverID != -1);
        }
        private bool _UpdateDriver()
        {
            return clsDriversData.UpdateDriver(this.DriverID, this.personID, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else return false;
                case enMode.Update:
                    return _UpdateDriver();
            }

            return false;
        }

        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicense.GetDriverLicenses(DriverID);
        }

        public DataTable GetLicenses()
        {
            return clsLicense.GetDriverLicenses(this.DriverID);
        }

        public static DataTable GetInternationalLicenses(int DriverID)
        {
            return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        }

        public DataTable GetInternationalLicenses()
        {
            return clsInternationalLicense.GetDriverInternationalLicenses(this.DriverID);
        }
    }
}
