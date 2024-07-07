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
using UI_LAYER.Properties;
using static LOGIC_LAYER.clsTestTypes;

namespace UI_LAYER.Test
{
    public partial class frmListTestAppointments : Form
    {
        private int _LocalDrivingLicenseApplicationID;
        
        private enTestType _TestType = enTestType.VisionTest;
        private DataTable _dtLicenseTestAppointments;
        public frmListTestAppointments(int localDrivingLicenseApplicationID, enTestType testType)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            _TestType = testType;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();
            _RefershTestAppointmentsList();
            _SetColumnsSizes();

        }

        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestType)
            {
                case enTestType.VisionTest:
                    lblTitle.Text = "Vision Test Appointments";
                    this.Text = lblTitle.Text;
                    pbTestTypeImage.Image = Resources.Vision_512;
                    break;
                case enTestType.WrittenTest:
                    lblTitle.Text = "Written Test Appointments";
                    this.Text = lblTitle.Text;
                    pbTestTypeImage.Image = Resources.Written_Test_512;
                    break;
                case enTestType.StreetTest:
                    lblTitle.Text = "Street Test Appointments";
                    this.Text = lblTitle.Text;
                    pbTestTypeImage.Image = Resources.driving_test_512;
                    break;
            }
        }

        private void _RefershTestAppointmentsList()
        {
            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
            _dtLicenseTestAppointments = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestType);

            dgvLicenseTestAppointments.DataSource = _dtLicenseTestAppointments;
            lblRecordsCount.Text = dgvLicenseTestAppointments.Rows.Count.ToString();
        }

        private void _SetColumnsSizes()
        {
            if (dgvLicenseTestAppointments.Rows.Count > 0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width = 150;

                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width = 200;

                dgvLicenseTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width = 150;

                dgvLicenseTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvLicenseTestAppointments.Columns[3].Width = 100;
            }
        }

      

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;

            new frmTakeTest(TestAppointmentID, _TestType).ShowDialog();

            _RefershTestAppointmentsList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;

            new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestType, TestAppointmentID).ShowDialog();

            _RefershTestAppointmentsList();
        }

        private void btnAddNewAppointment_Click_1(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);

            if (localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestType))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestType);

            if (LastTest == null)
            {
                new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestType).ShowDialog();
                _RefershTestAppointmentsList();
                return;
            }

            //if person already passed the test s/he cannot retak it.
            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            new frmScheduleTest(LastTest.TestAppointmentInfo.LocalDrivingLicenseAppID, _TestType)
                .ShowDialog();

            _RefershTestAppointmentsList();
            //---

        }
    }
}
