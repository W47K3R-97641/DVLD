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
    public partial class frmAddUpdateUser : Form
    {
        public enum enMode : byte { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        private int _UserID = -1;
        clsUser _User;

        public frmAddUpdateUser()
        {
            InitializeComponent();

            Mode = enMode.AddNew;
        }
        public frmAddUpdateUser(int userID)
        {
            InitializeComponent();
            Mode = enMode.Update;
            _UserID = userID;
        }

        private void _ResetDefualtValues()
        {
            if (Mode == enMode.Update)
            {
                lblModeTitle.Text = "Update User";
                tabLoginInfo.Enabled = true;
                this.Text = "Update User";
                btnSave.Enabled = true;
            }
            else
            {
                _User=new clsUser();
                lblModeTitle.Text = "Add New User";
                tabLoginInfo.Enabled = false;
                this.Text = "Add New User";
            }
        }
        private void _LoadData()
        {
            
            _User = clsUser.GetUserById(_UserID);
            if (_User == null)
            {
                MessageBox.Show("No User with ID = " + _User, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            lblUserID.Text = _User.userID.ToString();
            txtUserName.Text        = _User.username;
            txtPassword.Text        = _User.password;
            txtConfirmPassword.Text = _User.password;
            chkIsActive.Checked = _User.isActive;

            ctrlPersonInfoWithFilter1.LoadPersonInfo(_User.personID);
            
            ctrlPersonInfoWithFilter1.FilterEnabled = false;
        }
        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (Mode == enMode.Update)
                _LoadData();
        }  
        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {

            if (Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                TabsControl.SelectedTab = TabsControl.TabPages["tabLoginInfo"];
                return;
            }

            if (ctrlPersonInfoWithFilter1.selectedPersonInfo != null)
            {
                if (clsUser.IsExistPersonID(ctrlPersonInfoWithFilter1.selectedPersonInfo.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }
                else
                {
                    btnSave.Enabled = true;

                    tabLoginInfo.Enabled = true;

                    TabsControl.SelectedTab = TabsControl.TabPages["tabLoginInfo"];
                }
                
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void ctrlPersonInfoWithFilter1_OnPersonSelectedChange(int obj)
        {
            tabLoginInfo.Enabled = ctrlPersonInfoWithFilter1.selectedPersonInfo != null;
        }
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            txtUserName.Text = txtUserName.Text.Trim();
            if (String.IsNullOrEmpty(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "Username  cannot be blank");
                e.Cancel = true;
                return;

            }
            if (!clsValidation.ValidateUsernameFormat(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "Username Invalid  EX: exemple_username");
                e.Cancel = true;
                return;
            }

            if (Mode == enMode.AddNew && clsUser.IsExistUser(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "Username is used by another user");
                e.Cancel = true;
                return;
            }

            if (Mode == enMode.Update && IsUsernameChanged(txtUserName.Text) && clsUser.IsExistUser(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "Username is used by another user");
                e.Cancel = true;
                return;
            }

            errorProvider1.SetError(txtUserName, null);
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            txtPassword.Text = txtPassword.Text.Trim();

            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
                e.Cancel = true;
                return;
            }

            if (!clsValidation.ValidatePassword(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, @"Password requirment:
                                                       At least 8 characters long
                                                       Contains at least one uppercase letter
                                                       Contains at least one lowercase letter
                                                       Contains at least one digit
                                                       Contains at least one special character from the set: @$!%*?&");

                e.Cancel = true;
                return;
            }


            errorProvider1.SetError(txtPassword, null);
        }
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            txtConfirmPassword.Text = txtConfirmPassword.Text.Trim();
            
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
                e.Cancel = true;
                return;
            }

            errorProvider1.SetError(txtConfirmPassword,null);

        }
        private bool IsUsernameChanged(string newUsername)
        {
            return newUsername != _User.username;
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

            _User.username = txtUserName.Text;
            _User.password = txtPassword.Text;
            _User.isActive = chkIsActive.Checked;
            _User.personID = ctrlPersonInfoWithFilter1.selectedPersonInfo.PersonID;

            if (_User.Save())
            {
                lblUserID.Text = _User.userID.ToString();
                Mode = enMode.Update;
                
                _ResetDefualtValues();
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

       
    }
}
