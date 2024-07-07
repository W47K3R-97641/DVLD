using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATA_LAYER;
using static DATA_LAYER.clsPeopleData;

namespace LOGIC_LAYER
{
    public class clsPerson
    {
        
        public enum enMode : byte { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }

        }
        public string NationalNo { set; get; }
        public DateTime DateOfBirth { set; get; }
        public byte Gendor { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }

        public clsCountries CountryInfo;

        private string _ImagePath;

        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }
        public clsPerson() 
        {
            this.PersonID = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";

            Mode = enMode.AddNew;
        }
        private clsPerson(stpersonInfo personInfo)
        {
            this.PersonID    = personInfo.PersonID;
            this.FirstName   = personInfo.FirstName;
            this.SecondName  = personInfo.SecondName;
            this.ThirdName   = personInfo.ThirdName;
            this.LastName    = personInfo.LastName;
            this.NationalNo  = personInfo.NationalNo;
            this.DateOfBirth = personInfo.DateOfBirth;
            this.Gendor      = personInfo.Gendor;
            this.Address     = personInfo.Address;
            this.Phone       = personInfo.Phone;
            this.Email       = personInfo.Email;
            this.ImagePath   = personInfo.ImagePath;
            this.NationalityCountryID = personInfo.NationalityCountryID;
            this.CountryInfo          = clsCountries.Find(NationalityCountryID);
            Mode = enMode.Update;
        }
        private clsPerson(int PersonID, string FirstName, string SecondName, string ThirdName,
            string LastName, string NationalNo, DateTime DateOfBirth, byte Gendor,
             string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)

        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            this.CountryInfo = clsCountries.Find(NationalityCountryID);
            Mode = enMode.Update;
        }
        private bool AddNew()
        {
            this.PersonID = clsPeopleData.AddNewPerson(
                this.FirstName, this.SecondName, this.ThirdName,
                this.LastName, this.NationalNo,
                this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email,
                this.NationalityCountryID, this.ImagePath);

            

            return (this.PersonID != -1);
        }
        public static clsPerson Find(int personID)
        {
            stpersonInfo? personInfo = clsPeopleData.GetPersonInfoByID(personID);
           
            return personInfo.HasValue ? new clsPerson(personInfo.Value) : null;
        }

       
        
        public static clsPerson Find(string nationalNo)
        {
            stpersonInfo? personInfo = clsPeopleData.GetPersonInfoByNationalNo(nationalNo);

            return personInfo.HasValue ? new clsPerson(personInfo.Value) : null;
        }
        private bool Update()
        {
            return clsPeopleData.UpdatePerson(this.PersonID, this.FirstName, this.SecondName, this.ThirdName,
                this.LastName, this.NationalNo, this.DateOfBirth, this.Gendor,
                this.Address, this.Phone, this.Email,
                  this.NationalityCountryID, this.ImagePath);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNew())
                    {
                        Mode = enMode.Update;
                        return true;

                    }
                    else
                        return false;
                case enMode.Update:
                    return Update();
            }
            return false;
        }
        static public DataTable GetAllPepole()
        {
            return clsPeopleData.GetAllPeople();
        }
        static public bool DeletePerson(int personID)
        {
            return clsPeopleData.DeletePerson(personID);
        }
       
        public static bool IsExist(int personID)
        {
            return clsPeopleData.IsExist(personID);
        }
        public static bool IsExist(string nationalNo)
        {
            return clsPeopleData.IsExist(nationalNo);
        }
    }
}
