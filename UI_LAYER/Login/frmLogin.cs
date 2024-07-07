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
using static UI_LAYER.Global_Classes.clsGlobal;

namespace UI_LAYER.Login
{
    public partial class frmLogin : Form
    {
        
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            clsUser user = clsUser.GetUserInfoByUsernameAndPassword(username, password);

            if (user == null)
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {

                if (!user.isActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your account is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (chkRememberMe.Checked)
                {
                    clsGlobal.RememberUsernameAndPassword(username, password);
                }
                else
                {
                    clsGlobal.RememberUsernameAndPassword("", "");
                }

                clsGlobal.CurrentUser = user;
                this.Hide();

                new MainForm(this).ShowDialog();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            UserCredentials userCredentials = clsGlobal.GetStoredCredential();
            if (userCredentials != null)
            {
                txtUserName.Text = userCredentials.Username;
                txtPassword.Text = userCredentials.Password;
                chkRememberMe.Checked = true;
            }
            else
                chkRememberMe.Checked = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
