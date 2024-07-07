using LOGIC_LAYER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_LAYER.Global_Classes;

namespace UI_LAYER.Applications.Local_Driving_License
{
    public partial class frmAddUpdateLocalDrivingLicesnseApplication : Form
    {

        public enum enMode : byte { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int LocalApplicationID;
        private int LicenseClassID;

        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();
        }
        public frmAddUpdateLocalDrivingLicesnseApplication(int LocalApplicationID)
        {
            InitializeComponent();
            this.LocalApplicationID = LocalApplicationID;

            Mode = enMode.Update;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ctrlPersonInfoWithFilter1_OnPersonSelectedChange(int obj)
        {
           tpApplicationInfo.Enabled = ctrlPersonInfoWithFilter1.selectedPersonInfo != null; 
        }
        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (ctrlPersonInfoWithFilter1.selectedPersonInfo != null)
            {
                tabControl1.SelectedTab = tabControl1.TabPages["tpApplicationInfo"];
                btnSave.Enabled = true;
            }
            else
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        private void _FillLicenseClassesInComoboBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
                
            }
        }
        private void _LoadData()
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalApplicationID);

            if (_LocalDrivingLicenseApplication == null )
            {
                MessageBox.Show("No Application with ID = " + LocalApplicationID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            lblApplicationDate.Text = clsFormat.DateToShort(_LocalDrivingLicenseApplication.ApplicationDate);

            lblCreatedByUser.Text = _LocalDrivingLicenseApplication.CreatedByUserName;

            lblFees.Text = clsApplicationType.Find(_LocalDrivingLicenseApplication.ApplicationTypeID).Fees.ToString();

            lblLocalDrivingLicebseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();

            cbLicenseClass.SelectedIndex = _LocalDrivingLicenseApplication.LicenseClassID - 1;

            ctrlPersonInfoWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);
        }
        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (Mode == enMode.Update)
            {
                _LoadData();
            }

        }
        private void _ResetDefualtValues()
        {
            _FillLicenseClassesInComoboBox();
            
            switch (Mode)
            {
                case enMode.AddNew:
                    _SetNewApplicationDefaults();
                    break;
                case enMode.Update:
                    _SetUpdateApplicationDefaults();
                    break;
            }
        }
        private void _SetUpdateApplicationDefaults()
        {
            lblTitle.Text = "Update Local Driving License Application";
            tpApplicationInfo.Enabled = true;
            btnSave.Enabled = true;
            ctrlPersonInfoWithFilter1.FilterEnabled = false;
        }
        private void _SetNewApplicationDefaults()
        {
            lblTitle.Text = "New Local Driving License Application";
            ctrlPersonInfoWithFilter1.FilterFocus();
            tpApplicationInfo.Enabled = false;
            cbLicenseClass.SelectedIndex = 2;
            
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUser.Text = clsGlobal.CurrentUser.username;
            lblFees.Text = clsApplicationType.Find((int)enApplicationType.NewDrivingLicense).Fees.ToString();

            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           
           if (!_ValidateActiveLicenseApplication() || !_ValidateDuplicateLicense())
           { 
               return;
           }
            
            _UpdateApplicationDetails();

            if (_LocalDrivingLicenseApplication.Save())
            {
                _UpdateFormUI(_LocalDrivingLicenseApplication.ApplicationID);

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool _ValidateActiveLicenseApplication()
        {
            int ActiveApplicationID = clsLocalDrivingLicenseApplication.GetActiveApplicationIDForLicenseClass(ctrlPersonInfoWithFilter1.selectedPersonInfo.PersonID, enApplicationType.NewDrivingLicense, LicenseClassID);
            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return false;
            }

            return true;
        }

        private bool _ValidateDuplicateLicense()
        {
            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonInfoWithFilter1.selectedPersonInfo.PersonID, LicenseClassID))
            {
                MessageBox.Show("Person already has a license with the same applied driving class, Choose different driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void _UpdateFormUI(int applicationId)
        {
            lblLocalDrivingLicebseApplicationID.Text = applicationId.ToString();
            Mode = enMode.Update;
            lblTitle.Text = "Update Local Driving License Application";
        }
        private void _UpdateApplicationDetails()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                    _LocalDrivingLicenseApplication.ApplicantPersonID = ctrlPersonInfoWithFilter1.selectedPersonInfo.PersonID;
                    _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
                    _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
                    _LocalDrivingLicenseApplication.ApplicationStatus = enApplicationStatus.New;
                    _LocalDrivingLicenseApplication.ApplicationTypeID = (int)enApplicationType.NewDrivingLicense;
                    _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.userID;
                    _LocalDrivingLicenseApplication.PaidFees = Convert.ToDecimal(lblFees.Text);
                    _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;
                    break;
                case enMode.Update:
                    _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;
                    break;
            }
        }

        private void cbLicenseClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LicenseClassID = cbLicenseClass.SelectedIndex + 1;
        }
    }
}
