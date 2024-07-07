using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_LAYER.Applications.Applications_Types;
using UI_LAYER.Applications.International_Driving_License;
using UI_LAYER.Applications.Local_Driving_License;
using UI_LAYER.Applications.Release_Detained_License;
using UI_LAYER.Applications.Renew_Local_License;
using UI_LAYER.Applications.ReplaceLostOrDamagedLicense;
using UI_LAYER.Drivers;
using UI_LAYER.Global_Classes;
using UI_LAYER.Licenses.Detain_License;
using UI_LAYER.Login;
using UI_LAYER.Pepole;
using UI_LAYER.Test_Types;
using UI_LAYER.Users;

namespace UI_LAYER
{
    public partial class MainForm : Form
    {
        private frmLogin _frmlogin;
        public MainForm(frmLogin login)
        {
            InitializeComponent();
            _frmlogin = login;
        }
        private void pepoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManagePepole().ShowDialog();   
        }

      
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManageUsers().ShowDialog();

        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmShowUserInfo(clsGlobal.CurrentUser.userID).ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmChangePassword(clsGlobal.CurrentUser.userID).ShowDialog();
        }

        private void signOutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            _SignOut();
        }

        private void _SignOut()
        {
            clsGlobal.CurrentUser = null;
            _frmlogin.Show();
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _frmlogin.Close();
        }

        

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManageApplicationsTypes().ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmListTestTypes().ShowDialog();
        }

        private void localDrivingLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmListLocalDrivingLicesnseApplications().ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmManageDrivers().ShowDialog();
        }

        

        private void newDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmAddUpdateLocalDrivingLicesnseApplication().ShowDialog();
        }

        private void internationalLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmListInternationalLicesnseApplications().ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmRenewLocalDrivingLicenseApplication().ShowDialog();
        }

        private void replacementForLostOrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmReplaceLostOrDamagedLicense().ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmListDetainedLicenses().ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmDetainLicense().ShowDialog();
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {new frmReleaseDetainedLicenseApplication().ShowDialog();

        }
    }
}
