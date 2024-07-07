using DATA_LAYER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC_LAYER
{
    public class clsLicenseClass
    {

        public enum enMode : byte { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LicenseClassID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public decimal ClassFees { set; get; }
        public clsLicenseClass()
        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;

            Mode = enMode.AddNew;
        }
        public clsLicenseClass(int LicenseClassID, string ClassName,
           string ClassDescription,
           byte MinimumAllowedAge, byte DefaultValidityLength, decimal ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
            Mode = enMode.Update;
        }
        private clsLicenseClass(stLicenseClassInfo licenseClassInfo)
        {
            LicenseClassID = licenseClassInfo.LicenseClassID;
            ClassName = licenseClassInfo.ClassName;
            ClassDescription = licenseClassInfo.ClassDescription;
            MinimumAllowedAge = licenseClassInfo.MinimumAllowedAge;
            DefaultValidityLength = licenseClassInfo.DefaultValidityLength;
            ClassFees = licenseClassInfo.ClassFees;
            Mode = enMode.Update;
        }
        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }
        public static clsLicenseClass Find(int LicenseClassID)
        {
            stLicenseClassInfo? licenseClassInfo = clsLicenseClassData.GetLicenseClassByID(LicenseClassID);

            return licenseClassInfo != null ? new clsLicenseClass(licenseClassInfo.Value) : null;
        }
        public static clsLicenseClass Find(string ClassName)
        {
            stLicenseClassInfo? licenseClassInfo = clsLicenseClassData.GetLicenseClassByName(ClassName);

            return licenseClassInfo != null ? new clsLicenseClass(licenseClassInfo.Value) : null;
        }
        private bool _AddNewLicenseClass()
        {
            //call DataAccess Layer 

            this.LicenseClassID = clsLicenseClassData.AddNew(this.ClassName, this.ClassDescription,
                this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);


            return (this.LicenseClassID != -1);
        }
        private bool _UpdateLicenseClass()
        {
            //call DataAccess Layer 

            return clsLicenseClassData.Update(this.LicenseClassID, this.ClassName, this.ClassDescription,
                this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicenseClass())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicenseClass();

            }

            return false;
        }
    }
}
