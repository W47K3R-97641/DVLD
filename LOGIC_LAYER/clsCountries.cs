using DATA_LAYER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC_LAYER
{
    public class clsCountries
    {
        enum enmode : byte { AddNew = 0, Update = 1 };
        private enmode mode = enmode.AddNew;

        public int countryID { get; set; }
        public string countryName { get; set; }


        private clsCountries(int countryID, string countryName)
        {
            this.countryID = countryID;
            this.countryName = countryName;


            this.mode = enmode.Update;
        }

        public clsCountries()
        {
            this.countryID = -1;
            this.countryName = "";

            this.mode = enmode.AddNew;
        }

        public static DataTable GetAllCountries()
        {
            return clsCountriesData.GetAllCountries();
        }
        public static clsCountries Find(int ID)
        {
            string countryName = "";

            if (clsCountriesData.FindByID(ID, ref countryName))
            {
                return new clsCountries(ID, countryName);
            }
            else
            {
                return null;
            }
        }
        public static clsCountries Find(string countryName)
        {
            int ID = -1;
            
            if (clsCountriesData.FindByName(countryName, ref ID))
            {
                return new clsCountries(ID, countryName);
            }
            else
            {
                return null;
            }
        }
        public static bool DeleteCountry(int ID)
        {
            return clsCountriesData.DeleteCountry(ID);
        }
        public static bool IsCountryExist(int ID)
        {
            return clsCountriesData.IsCountryExist(ID);
        }
        public static bool IsCountryExist(string countryName)
        {
            return clsCountriesData.IsCountryExist(countryName);
        }


        private bool _AddNewCountry()
        {
            this.countryID = clsCountriesData.AddNewCountry(this.countryName);

            return this.countryID != -1;
        }
        private bool _UpdateCountry()
        {
            return clsCountriesData.UpdateCountry(this.countryID, this.countryName);
        }
        public bool Save()
        {
            switch (mode)
            {
                case enmode.AddNew:
                    if (_AddNewCountry())
                    {
                        mode = enmode.Update;
                        return true;
                    }
                    else
                        return false;
                case enmode.Update:
                    return _UpdateCountry();
            }
            return false;
        }


    }
}



