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
using UI_LAYER.Test.Test_Types;

namespace UI_LAYER.Test_Types
{
    public partial class frmListTestTypes : Form
    {
        private DataTable _dtAllTestTypes;
        public frmListTestTypes()
        {
            InitializeComponent();
        }



        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
        private void _RefershTestTypesList()
        {
            _dtAllTestTypes = clsTestTypes.GetAllTestTypes();

            dgvTestTypes.DataSource = _dtAllTestTypes;

            lblRecordsCount.Text = dgvTestTypes.RowCount.ToString();
            
        }
        private void _SetColumnsSizes()
        {
            dgvTestTypes.Columns[0].HeaderText = "ID";
            dgvTestTypes.Columns[0].Width = 120;

            dgvTestTypes.Columns[1].HeaderText = "Title";
            dgvTestTypes.Columns[1].Width = 200;

            dgvTestTypes.Columns[2].HeaderText = "Description";
            dgvTestTypes.Columns[2].Width = 400;

            dgvTestTypes.Columns[3].HeaderText = "Fees";
            dgvTestTypes.Columns[3].Width = 100;
        }

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _RefershTestTypesList();
            _SetColumnsSizes();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int testTypeID = (int)dgvTestTypes.CurrentRow.Cells[0].Value;

            new frmEditTestType(testTypeID).ShowDialog();

            _RefershTestTypesList();
        }
    }
}
