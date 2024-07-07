using DATA_LAYER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC_LAYER
{
    public class clsApplicationType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { set; get; }
        public string Title { set; get; }
        public decimal Fees { set; get; }

       
        public clsApplicationType()
        {
            this.ID = -1;
            this.Title = "";
            this.Fees = 0;
            Mode = enMode.AddNew;
        }

        private clsApplicationType(int ID, string title, decimal fees)
        {
            this.ID = ID;
            this.Title = title;
            this.Fees = fees;
            Mode = enMode.Update;
        }

        private clsApplicationType(stAppTypeInfo appInfo)
        {
            this.ID =    appInfo.ID;
            this.Title = appInfo.Title;
            this.Fees =  appInfo.Fees;
            Mode = enMode.Update;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationType();
        }
        public static clsApplicationType Find(int ID)
        {
            stAppTypeInfo? appTypeInfo = clsApplicationTypesData.GetApplicationTypeByID(ID);

            return appTypeInfo != null ? new clsApplicationType(appTypeInfo.Value) : null;
        }
        private bool _AddNew()
        {
            this.ID = clsApplicationTypesData.AddNew(this.Title, this.Fees);
            return (this.ID != -1);
        }
        private bool _Update()
        {
            return clsApplicationTypesData.Update(ID, this.Title, this.Fees);
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else return false;
                case enMode.Update:
                    return _Update();
            }
            return false;
        }
    }
}
