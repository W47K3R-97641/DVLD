using LOGIC_LAYER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_LAYER.Global_Classes;
using UI_LAYER.Pepole;

namespace UI_LAYER.Users
{
    public partial class frmManageUsers : Form
    {
        private static DataTable _dtAllUsers;

        private enum enFilterCoulmn : byte 
        {
            None     = 0,
            UserID,
            UserName,
            PersonID,
            FullName,
            IsActive,
        };
        private enFilterCoulmn _enFilterCoulmn = enFilterCoulmn.None;

        private string FilterBy
        {
            get { return Enum.GetName(typeof(enFilterCoulmn), _enFilterCoulmn); }
        }

        private enum enIsActive : byte
        {
            All = 0,
            Yes = 1,
            No = 2,
        }

       

        public frmManageUsers()
        {
            InitializeComponent();
        }
        private void _RefershPepoleList()
        {
            _dtAllUsers = clsUser.GetAllUsers();
            
            dgvUsers.DataSource = _dtAllUsers;

            lblRecords.Text = dgvUsers.Rows.Count.ToString();
        }
        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            _RefershPepoleList();
            _SetColumnsSizes();
            cmbfilter.SelectedIndex = (byte)enFilterCoulmn.None;
        }

        private void _SetColumnsSizes()
        {
            dgvUsers.Columns[0].HeaderText = "User ID";
            dgvUsers.Columns[0].Width = 110;

            dgvUsers.Columns[1].HeaderText = "Person ID";
            dgvUsers.Columns[1].Width = 120;

            dgvUsers.Columns[2].HeaderText = "Full Name";
            dgvUsers.Columns[2].Width = 350;

            dgvUsers.Columns[3].HeaderText = "UserName";
            dgvUsers.Columns[3].Width = 120;

            dgvUsers.Columns[4].HeaderText = "Is Active";
            dgvUsers.Columns[4].Width = 120;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void cmbfilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _enFilterCoulmn = ((enFilterCoulmn)cmbfilter.SelectedIndex);
            
            _dtAllUsers.DefaultView.RowFilter = "";

            lblRecords.Text = _dtAllUsers.Rows.Count.ToString();

            if (_enFilterCoulmn == enFilterCoulmn.IsActive)
            {
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
                cbIsActive.Focus();
                txtFilterValue.Visible = false;
            }
            else
            {
                cbIsActive.Visible = false;
                txtFilterValue.Visible = (_enFilterCoulmn != enFilterCoulmn.None);
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }
        
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = txtFilterValue.Text.Trim();

            if (txtFilterValue.Text == string.Empty)
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecords.Text = dgvUsers.Rows.Count.ToString();
                return;
            }

            if (_enFilterCoulmn == enFilterCoulmn.UserID || _enFilterCoulmn ==  enFilterCoulmn.PersonID)
            {
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, txtFilterValue.Text);
            }
            else
            {
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterBy, txtFilterValue.Text);

            }
        }
        private void btnAddPepole_Click(object sender, EventArgs e)
        {
            new frmAddUpdateUser().ShowDialog();
            frmManageUsers_Load(null, null);
        }
        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            enIsActive isActiveFilterCoulmn = (enIsActive)cbIsActive.SelectedIndex;

            switch(isActiveFilterCoulmn) 
            {
                case enIsActive.All:
                    _dtAllUsers.DefaultView.RowFilter = "";
                    break;
                case enIsActive.Yes:
                    _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, true);
                    break;
                case enIsActive.No:
                    _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, false);
                    break;
            }

            lblRecords.Text = dgvUsers.Rows.Count.ToString();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int userID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            new frmShowUserInfo(userID).ShowDialog();
        }
        private void addNewUsertoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btnAddPepole.PerformClick();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int userID = (int)dgvUsers.CurrentRow.Cells[0].Value;

            new frmAddUpdateUser(userID).ShowDialog();

            _RefershPepoleList();
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsUtil.ConfirmOperationMsgBx($"Are you sure you want to delete User [{dgvUsers.CurrentRow.Cells[0].Value}]", "Confirm Delete"))
            {
                //Perform Delele and refresh
                if(clsUser.DeleteUser((int)dgvUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("User Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefershPepoleList();
                }

                else
                    MessageBox.Show("User was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Show User Info
            showDetailsToolStripMenuItem_Click(null, null);
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ChangePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int userID = (int)dgvUsers.CurrentRow.Cells[0].Value;

            new frmChangePassword(userID).ShowDialog();
        }
    }
}
