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

namespace UI_LAYER.Users
{
    public partial class frmChangePassword : Form
    {

        private int _userID = -1;
        private clsUser _user;
        public frmChangePassword(int userID)
        {
            InitializeComponent();
            _userID = userID;
            
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();
            _user = clsUser.GetUserById(_userID);
            if ( _user == null )
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Could not Find User with id = " + _userID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;
            }


        }

        private void _ResetDefualtValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if ( txtCurrentPassword.Text != _user.password ) 
            {
                errorProvider1.SetError(txtCurrentPassword, "Current password is wrong!");
                return;
            }

            errorProvider1.SetError(txtCurrentPassword, null);
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if ( string.IsNullOrEmpty(txtNewPassword.Text) )
            {
                errorProvider1.SetError(txtNewPassword, "New Password cannot be blank");
                return;
            }

            if (!clsValidation.ValidatePassword(txtNewPassword.Text) )
            {
                errorProvider1.SetError(txtNewPassword, @"Password requirment:
                                                       At least 8 characters long
                                                       Contains at least one uppercase letter
                                                       Contains at least one lowercase letter
                                                       Contains at least one digit
                                                       Contains at least one special character from the set: @$!%*?&");

                return;
            }

            errorProvider1.SetError(txtNewPassword, null);

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if ( txtNewPassword.Text != txtConfirmPassword.Text )
            {
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match New Password!");
                return;
            }

            errorProvider1.SetError(txtConfirmPassword, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _user.password = txtNewPassword.Text;

            if (_user.Save())
            {
                MessageBox.Show("Password Changed Successfully.",
                  "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ResetDefualtValues(); 
            }
            else
            {
                MessageBox.Show("An Error Occured, Password did not change.",
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
