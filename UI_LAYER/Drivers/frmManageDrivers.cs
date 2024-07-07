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
using UI_LAYER.Licenses;
using UI_LAYER.Licenses.International_Licenses;
using UI_LAYER.Pepole;

namespace UI_LAYER.Drivers
{
    public partial class frmManageDrivers : Form
    {

        private enum enFilterDriversBy : byte
        {
            None = 0,
            DriverID,
            PersonID,
            NationalNo,
            FullName
        }
    
        private enFilterDriversBy _filterBy = enFilterDriversBy.None;

        private string FilterBy
        {
            get { return Enum.GetName(typeof(enFilterDriversBy), _filterBy); }
        }

        private DataTable _dtAllDrivers = clsDriver.GetAllDrivers();

        public frmManageDrivers()
        {
            InitializeComponent();
        }

        private void _RefershDriversList()
        {
            _dtAllDrivers = clsDriver.GetAllDrivers();

            dgvDrivers.DataSource = _dtAllDrivers;

            lblRecords.Text = dgvDrivers.RowCount.ToString();
        }

        private void frmManageDrivers_Load(object sender, EventArgs e)
        {

            _RefershDriversList();
            _SetColumnsSizes();
            cbFilterBy.SelectedIndex = (byte)enFilterDriversBy.None;
        }

        private void _SetColumnsSizes()
        {
            if (dgvDrivers.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].HeaderText = "DriverInfo ID";
                dgvDrivers.Columns[0].Width = 120;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 120;

                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[2].Width = 140;

                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 320;

                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[4].Width = 170;

                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvDrivers.Columns[5].Width = 150;
            }

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filterBy = (enFilterDriversBy)cbFilterBy.SelectedIndex;

            txtFilterValue.Visible = _filterBy != enFilterDriversBy.None;
            
            txtFilterValue.Text = "";

            _dtAllDrivers.DefaultView.RowFilter = "";

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = txtFilterValue.Text.Trim();
            
            if (string.IsNullOrEmpty(txtFilterValue.Text) || _filterBy == enFilterDriversBy.None)
            {
                _dtAllDrivers.DefaultView.RowFilter = "";
                lblRecords.Text = dgvDrivers.Rows.Count.ToString();
                return;
            }

            if (_filterBy == enFilterDriversBy.PersonID || _filterBy == enFilterDriversBy.DriverID)
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, txtFilterValue.Text);
            else
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] Like '{1}%'", FilterBy, txtFilterValue.Text);

            lblRecords.Text = dgvDrivers.Rows.Count.ToString();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = (int)dgvDrivers.CurrentRow.Cells[1].Value;

            new frmShowPersonInfo(personID).ShowDialog();

            frmManageDrivers_Load(null, null);
        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Yet");


            
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = (int)dgvDrivers.CurrentRow.Cells[1].Value;

            new frmShowPersonLicenseHistory(personID).ShowDialog();
        }
    }
}
