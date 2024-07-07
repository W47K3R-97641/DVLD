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

namespace UI_LAYER.Applications.Applications_Types
{
    public partial class frmManageApplicationsTypes : Form
    {
        private DataTable _dtAllAppTypes;

        public frmManageApplicationsTypes()
        {
            InitializeComponent();
        }

        private void _RefershApplicationTypesList()
        {
            _dtAllAppTypes = clsApplicationType.GetAllApplicationTypes();

            dgvApplicationTypes.DataSource = _dtAllAppTypes;

            lblRecordsCount.Text = dgvApplicationTypes.Rows.Count.ToString();
        }

        private void frmManageApplicationsTypes_Load(object sender, EventArgs e)
        {
            _RefershApplicationTypesList();
            _SetColumnsSizes();
        }

        private void _SetColumnsSizes()
        {
            dgvApplicationTypes.Columns[0].HeaderText = "ID";
            dgvApplicationTypes.Columns[0].Width = 110;

            dgvApplicationTypes.Columns[1].HeaderText = "Title";
            dgvApplicationTypes.Columns[1].Width = 400;

            dgvApplicationTypes.Columns[2].HeaderText = "Fees";
            dgvApplicationTypes.Columns[2].Width = 120;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int appID = (int)dgvApplicationTypes.CurrentRow.Cells[0].Value;

            new frmEditApplicationTypes(appID).ShowDialog();

            _RefershApplicationTypesList();
        }
    }
}
