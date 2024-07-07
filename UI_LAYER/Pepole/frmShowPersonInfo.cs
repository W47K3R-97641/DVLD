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

namespace UI_LAYER.Pepole
{
    public partial class frmShowPersonInfo : Form
    {
        public frmShowPersonInfo(int personID)
        {
            InitializeComponent();

            ctrlPersonInfo1.LoadPersonInfo(personID);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frmAddUpdatePerson = new frmAddUpdatePerson(ctrlPersonInfo1.PersonID);
            
            frmAddUpdatePerson.ShowDialog();

            ctrlPersonInfo1.LoadPersonInfo(ctrlPersonInfo1.PersonID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
