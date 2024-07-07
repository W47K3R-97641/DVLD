using DATA_LAYER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC_LAYER
{
   
    public class clsTestTypes
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;

        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };

        private int _ID;
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                Type = (enTestType)value; // Assuming Type should be set based on a valid enTestType
            }
        }
        public enTestType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Fees { get; set; }

        public clsTestTypes()
        {
            ID = -1;
            Title = "";
            Description = "";
            Fees = 0;
            Mode = enMode.AddNew;
        }

        private clsTestTypes(int ID, string title, string description, decimal fees)
        {
            this.ID = ID;
            Title   = title;
            Description = description;
            Fees = fees;
            Mode = enMode.Update;

        }
        private clsTestTypes(stTestTypeInfo testTypeInfo)
        {
            ID = testTypeInfo.ID;
            Title = testTypeInfo.Title;
            Description = testTypeInfo.Description;
            Fees = testTypeInfo.Fees;
            Mode = enMode.Update;
        }
        private bool _AddNew()
        {
            this.ID = clsTestTypesData.AddNew(Title, Description, Fees);

            return this.ID != -1;
        }

        private bool _Update()
        {
            return clsTestTypesData.Update((int)ID, Title, Description, Fees);
        }
        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();
        }
        public static clsTestTypes Find(int ID)
        {
            stTestTypeInfo? testTypeInfo = clsTestTypesData.GetTestTypeByID(ID);
            return (testTypeInfo != null) ? new clsTestTypes(testTypeInfo.Value) : null;
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
                    } else return false;
                case enMode.Update:
                    return _Update();
            }
            return false;
        }
        

    }
}
