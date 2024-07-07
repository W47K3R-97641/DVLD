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

namespace UI_LAYER.Test
{
    public partial class frmTakeTest : Form
    {
        private int _AppointmentID;
        private clsTestTypes.enTestType _TestType;

        
        private clsTest _Test;


        public frmTakeTest(int AppointmentID, clsTestTypes.enTestType TestType)
        {
            InitializeComponent();
            _AppointmentID = AppointmentID;
            _TestType = TestType;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!clsUtil.ConfirmOperationMsgBx("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.", "Confirm"))
            {
                return;
            }
            _Test.TestAppointmentID = _AppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.Notes = txtNotes.Text.Trim();
            _Test.CreatedByUserID = clsGlobal.CurrentUser.userID;

            if (_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTestInfo1.TestTypeID = _TestType;
            ctrlScheduleTestInfo1.LoadInfo(_AppointmentID);

            if (ctrlScheduleTestInfo1.TestAppointmentID == -1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;

            int _TestID = ctrlScheduleTestInfo1.TestID;
            if (_TestID != -1)
            {
                _Test = clsTest.Find(_TestID);

                if (_Test.TestResult)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;
                
                txtNotes.Text = _Test.Notes;

                lblUserMessage.Visible = true;
                
                rbFail.Enabled = false;
                rbPass.Enabled = false;
                btnSave.Enabled = false;
            }

            else
                _Test = new clsTest();
        }
    }
}
