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

namespace UI_LAYER.Users
{
    public partial class ctrlUserCard : UserControl
    {
        private clsUser _user;
        
        public ctrlUserCard()
        {
            InitializeComponent();

           
        }

        private void ctrlUserCard_Load(object sender, EventArgs e)
        {
            
        }
        private void _FillUserInfo()
        {
            lblUserID.Text = _user.userID.ToString();
            lblUserName.Text = _user.username;
            
            if (_user.isActive)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";

            ctrlPersonInfo1.LoadPersonInfo(_user.personID);
        }

        public void LoadUserInfo(int userId)
        {
            _user = clsUser.GetUserById(userId);

            if ( _user == null )
            {
                _ResetPersonInfo();
                MessageBox.Show("No User with UserID = " + userId.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



           _FillUserInfo();
        }

        private void _ResetPersonInfo()
        {

            ctrlPersonInfo1.ResetPersonInfo();
            lblUserID.Text = "[???]";
            lblUserName.Text = "[???]";
            lblIsActive.Text = "[???]";
        }
    }
}
