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
using UI_LAYER.Licenses.Local_Licenses;

namespace UI_LAYER.Applications.Local_Driving_License
{
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {
        private int LicenseID;
        private clsLocalDrivingLicenseApplication _localLicenseApplication;
        

        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }
        private void _FillLocalDrivingLicenseApplicationInfo()
        {

            LicenseID = _localLicenseApplication.GetActiveLicenseID();

            //incase there is license enable the show link.
            llShowLicenceInfo.Enabled = (LicenseID != -1);
            
            lblAppliedFor.Text = clsLicenseClass.Find(_localLicenseApplication.LicenseClassID).ClassName;
            lblLocalDrivingLicenseApplicationID.Text = _localLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblPassedTests.Text = _localLicenseApplication.GetPassedTestCount() + "/3";

            ctrlApplicationInfoShow1.LoadApplicationInfo(_localLicenseApplication.ApplicationID);
        }

        private void _ResetLocalDrivingLicenseApplicationInfo()
        {

            ctrlApplicationInfoShow1.ResetApplicationInfo();
            lblLocalDrivingLicenseApplicationID.Text = "[????]";
            lblAppliedFor.Text = "[????]";
            lblPassedTests.Text = "[????]";
        }
        public void LoadApplicationInfoByLocalDrivingAppID(int LocalDrivingLicenseApplicationID)
        {
            _localLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);

            if (_localLicenseApplication == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();

                MessageBox.Show("No Application with ApplicationID = " + LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }

        private void llShowLicenceInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmShowLicenseInfo(LicenseID).ShowDialog();
        }

       
    }
}
