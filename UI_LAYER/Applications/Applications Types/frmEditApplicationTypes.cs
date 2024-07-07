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

namespace UI_LAYER.Applications.Applications_Types
{
    public partial class frmEditApplicationTypes : Form
    {
        private clsApplicationType _ApplicationType;
        private int _ApplicationTypeID = -1;
        public frmEditApplicationTypes(int applicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = applicationTypeID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            
            _ApplicationType.Fees = decimal.Parse(txtFees.Text);
            _ApplicationType.Title = txtTitle.Text;

            if (_ApplicationType.Save())
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        
        }

        private void frmAddUpdateApplicationTypes_Load(object sender, EventArgs e)
        {
            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);

            if (_ApplicationType != null)
            {
                lblApplicationTypeID.Text = _ApplicationType.ID.ToString();
                txtTitle.Text = _ApplicationType.Title.ToString();
                txtFees.Text = _ApplicationType.Fees.ToString();
            }
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title cannot be empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtTitle, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            txtFees.Text = txtFees.Text.Trim();

            if (string.IsNullOrEmpty(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees cannot be empty!");
                return;
            }

            if (!clsValidation.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number.");
                return;
            }
            errorProvider1.SetError(txtFees, null);

        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {

            // Check if the key pressed is a digit (0-9) or the Backspace key
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // If not a digit or Backspace key, ignore the input
                e.Handled = true;
                
            }
        }

        
    }
}
