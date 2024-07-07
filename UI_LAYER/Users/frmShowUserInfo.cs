﻿using System;
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
    public partial class frmShowUserInfo : Form
    {
        public frmShowUserInfo(int userID)
        {
            InitializeComponent();

            ctrlUserCard1.LoadUserInfo(userID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
