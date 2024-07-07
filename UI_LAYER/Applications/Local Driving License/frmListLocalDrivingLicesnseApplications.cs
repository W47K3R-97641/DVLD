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
using UI_LAYER.Licenses;
using UI_LAYER.Licenses.Local_Licenses;
using UI_LAYER.Test;

namespace UI_LAYER.Applications.Local_Driving_License
{
    public partial class frmListLocalDrivingLicesnseApplications : Form
    {
        DataTable _dtAllLocalDrivingLicesnseApplications;
        public enum enFilterDrivingLicenseAppBy : byte
        {
            None = 0,
            LocalDrivingLicenseApplicationID, 
            NationalNo,
            FullName,
            Status
        }
        private enFilterDrivingLicenseAppBy _filterBy = enFilterDrivingLicenseAppBy.None;
        private string FilterBy
        {
            get { return Enum.GetName(typeof(enFilterDrivingLicenseAppBy), _filterBy); }
        }
        public frmListLocalDrivingLicesnseApplications()
        {
            InitializeComponent();
        }
        private void _RefershDrivingLicesnseApplicationsList()
        {
            _dtAllLocalDrivingLicesnseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();

            dgvLocalDrivingLicenseApplications.DataSource = _dtAllLocalDrivingLicesnseApplications;

            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }
        private void frmListLocalDrivingLicesnseApplications_Load(object sender, EventArgs e)
        {
            _RefershDrivingLicesnseApplicationsList();
            _SetColumnsSizes();
            cbFilterBy.SelectedIndex = (byte)enFilterDrivingLicenseAppBy.None;
        }
        private void _SetColumnsSizes()
        {
            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0)
            {

                dgvLocalDrivingLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingLicenseApplications.Columns[0].Width = 120;

                dgvLocalDrivingLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplications.Columns[1].Width = 200;

                dgvLocalDrivingLicenseApplications.Columns[2].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplications.Columns[2].Width = 150;

                dgvLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplications.Columns[3].Width = 250;

                dgvLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplications.Columns[4].Width = 130;

                dgvLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplications.Columns[5].Width = 80;
            }
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filterBy = (enFilterDrivingLicenseAppBy)cbFilterBy.SelectedIndex;

            txtFilterValue.Visible = _filterBy != enFilterDrivingLicenseAppBy.None && _filterBy != enFilterDrivingLicenseAppBy.Status;

            cmbStatus.Visible = _filterBy == enFilterDrivingLicenseAppBy.Status;
            
            if (cmbStatus.Visible)
            {
                cmbStatus.SelectedIndex = 0;
            }

            _dtAllLocalDrivingLicesnseApplications.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();

        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = txtFilterValue.Text.Trim();

            if (txtFilterValue.Text == "")
            {
                _dtAllLocalDrivingLicesnseApplications.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
                return;
            }

            if (_filterBy == enFilterDrivingLicenseAppBy.LocalDrivingLicenseApplicationID)
            {
                _dtAllLocalDrivingLicesnseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, txtFilterValue.Text);
            }
            else
            {
                _dtAllLocalDrivingLicesnseApplications.DefaultView.RowFilter = string.Format("[{0}] Like '{1}%'", FilterBy, txtFilterValue.Text);
            }

            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
        }
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStatus.SelectedIndex == 0)
            {
                _dtAllLocalDrivingLicesnseApplications.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();

                return;
            }
            
            enApplicationStatus enApplicationStatus = (enApplicationStatus)cmbStatus.SelectedIndex;

            string filterStrung = Enum.GetName(typeof(enApplicationStatus), enApplicationStatus);

            _dtAllLocalDrivingLicesnseApplications.DefaultView.RowFilter = string.Format("[{0}] Like '{1}'", FilterBy, filterStrung);

            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            new frmAddUpdateLocalDrivingLicesnseApplication().ShowDialog();
            _RefershDrivingLicesnseApplicationsList();
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_filterBy == enFilterDrivingLicenseAppBy.LocalDrivingLicenseApplicationID)
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            new frmAddUpdateLocalDrivingLicesnseApplication(LocalApplicationID).ShowDialog();
            _RefershDrivingLicesnseApplicationsList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            new frmLocalDrivingLicenseApplicationInfo(LocalApplicationID).ShowDialog();
            _RefershDrivingLicesnseApplicationsList();

        }

        private void CancelApplicaitonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsUtil.ConfirmOperationMsgBx("Are you sure do want to cancel this application?", "Confirm"))
                return;

            int LocalApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;


            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalApplicationID);
            
            if (LocalDrivingLicenseApplication != null && LocalDrivingLicenseApplication.Cancel())
            {
                MessageBox.Show("Application Cancelled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //refresh the form again.
                frmListLocalDrivingLicesnseApplications_Load(null, null);
            }
            else
            {
                MessageBox.Show("Could not cancel applicatoin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                    clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID
                                                    (LocalDrivingLicenseApplicationID);

            int TotalPassedTests = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[5].Value;
            
            bool LicenseExists = LocalDrivingLicenseApplication.IsLicenseIssued();
            
            showLicenseToolStripMenuItem.Enabled = LicenseExists;
            
            editToolStripMenuItem.Enabled = !LicenseExists && (LocalDrivingLicenseApplication.ApplicationStatus == enApplicationStatus.New);
            
            //Enabled only if person passed all tests and Does not have license. 
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExists;
            
            //Enable/Disable Cancel Menue Item
            //We only canel the applications with status=new.
            CancelApplicaitonToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == enApplicationStatus.New);
            
            //Enable/Disable Delete Menue Item
            //We only allow delete incase the application status is new not complete or Cancelled.
            DeleteApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == enApplicationStatus.New);
            
            _HandleScheduleTest(LocalDrivingLicenseApplication);
        }

        private void _HandleScheduleTest(clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication)
        {
            var isPassedTests = new
            {
                VisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.VisionTest),
                WrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.WrittenTest),
                StreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestType.StreetTest),
            };

            ScheduleTestsMenue.Enabled = (!isPassedTests.VisionTest || !isPassedTests.WrittenTest || !isPassedTests.StreetTest) && (LocalDrivingLicenseApplication.ApplicationStatus == enApplicationStatus.New);

            if (ScheduleTestsMenue.Enabled == false)
                return;

            //To Allow Schdule vision test, Person must not passed the same test before.
            scheduleVisionTestToolStripMenuItem.Enabled  = !isPassedTests.VisionTest;
            
            //To Allow Schdule written test, Person must pass the vision test and must not passed the same test before.
            scheduleWrittenTestToolStripMenuItem.Enabled = isPassedTests.VisionTest && !isPassedTests.WrittenTest;
            
            //To Allow Schdule steet test, Person must pass the vision * written tests, and must not passed the same test before.
            scheduleStreetTestToolStripMenuItem.Enabled  = isPassedTests.VisionTest && isPassedTests.WrittenTest && !isPassedTests.StreetTest;
            
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestType.VisionTest);
        }
        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestType.WrittenTest);
        }
        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestTypes.enTestType.StreetTest);
        }
        private void _ScheduleTest(clsTestTypes.enTestType TestType)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            new frmListTestAppointments(LocalDrivingLicenseApplicationID,TestType).ShowDialog();

            _RefershDrivingLicesnseApplicationsList();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            new frmIssueDriverLicenseFirstTime(LocalDrivingLicenseApplicationID).ShowDialog();

            _RefershDrivingLicesnseApplicationsList();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication app = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);
           
            new frmShowLicenseInfo(app.GetActiveLicenseID()).ShowDialog();
        }

        private void DeleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!clsUtil.ConfirmOperationMsgBx("Are you sure do want to delete this application?", "Confirm"))
                return;

            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //refresh the form again.
                    frmListLocalDrivingLicesnseApplications_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Could not delete applicatoin, other data depends on it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication app = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);

            new frmShowPersonLicenseHistory(app.ApplicantPersonID).ShowDialog();
            
        }
    }
}
