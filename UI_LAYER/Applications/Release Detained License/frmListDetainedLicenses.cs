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
using UI_LAYER.Licenses.Detain_License;
using UI_LAYER.Licenses.Local_Licenses;
using UI_LAYER.Pepole;
using static UI_LAYER.Applications.Local_Driving_License.frmListLocalDrivingLicesnseApplications;

namespace UI_LAYER.Applications.Release_Detained_License
{
    public partial class frmListDetainedLicenses : Form
    {
        private DataTable _dtDetainedLicenses;

        public enum enFilterDetaineLicenseBy
        {
            None,
            DetainID,
            IsReleased,
            NationalNo,
            FullName,
            ReleaseApplicationId,
        }

        private enFilterDetaineLicenseBy _filterBy = enFilterDetaineLicenseBy.None;
        private string FilterBy
        {
            get { return Enum.GetName(typeof(enFilterDetaineLicenseBy), _filterBy); }
        }
        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            _RefershDetainedLIcensesList();
            _SetColumnsSizes();
            cbFilterBy.SelectedIndex = 0;
        }

        private void _RefershDetainedLIcensesList()
        {
            _dtDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();

            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;
            lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }

        private void _SetColumnsSizes()
        {
            if (dgvDetainedLicenses.Rows.Count > 0)
            {
                dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvDetainedLicenses.Columns[0].Width = 90;

                dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicenses.Columns[1].Width = 90;

                dgvDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvDetainedLicenses.Columns[2].Width = 160;

                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[3].Width = 110;

                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].Width = 110;

                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[5].Width = 160;

                dgvDetainedLicenses.Columns[6].HeaderText = "N.No.";
                dgvDetainedLicenses.Columns[6].Width = 90;

                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns[7].Width = 330;

                dgvDetainedLicenses.Columns[8].HeaderText = "Rlease App.ID";
                dgvDetainedLicenses.Columns[8].Width = 150;

            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filterBy = ( enFilterDetaineLicenseBy )cbFilterBy.SelectedIndex;

            cbIsReleased.Visible = _filterBy == enFilterDetaineLicenseBy.IsReleased;

            if (cbIsReleased.Visible)
            {
                cbIsReleased.SelectedIndex = 0;
                return;
            }

            txtFilterValue.Visible = _filterBy != enFilterDetaineLicenseBy.IsReleased && _filterBy != enFilterDetaineLicenseBy.None;

            _dtDetainedLicenses.DefaultView.RowFilter = "";
            lblTotalRecords.Text = dgvDetainedLicenses.RowCount.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = txtFilterValue.Text.Trim();

            if (txtFilterValue.Text == "")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                lblTotalRecords.Text = dgvDetainedLicenses.RowCount.ToString();
                return;
            }

            if (_filterBy != enFilterDetaineLicenseBy.FullName && _filterBy != enFilterDetaineLicenseBy.NationalNo)
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, txtFilterValue.Text);
            else
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] Like '{1}%'", FilterBy, txtFilterValue.Text);

            lblTotalRecords.Text = dgvDetainedLicenses.RowCount.ToString();

        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch(cbIsReleased.SelectedIndex)
            {
                case 1:
                    _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, true);
                    break;
                case 2:
                    _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, false);
                    break;
                default:
                    _dtDetainedLicenses.DefaultView.RowFilter = "";
                    break;
            }

            lblTotalRecords.Text = dgvDetainedLicenses.RowCount.ToString();
        }

        private void PesonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;

            clsLicense license = clsLicense.Find(licenseID);

            if (license != null)
                new frmShowPersonInfo(license.DriverInfo.PersonID).ShowDialog();
            
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;

            new frmShowLicenseInfo(licenseID).ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;

            clsLicense license = clsLicense.Find(licenseID);

            if (license != null)
                new frmShowPersonLicenseHistory(license.DriverInfo.PersonID).ShowDialog();
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            bool IsReleased = (bool)dgvDetainedLicenses.CurrentRow.Cells[3].Value;

            releaseDetainedLicenseToolStripMenuItem.Enabled = !IsReleased;
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            new frmDetainLicense().ShowDialog();

            _RefershDetainedLIcensesList();
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            new frmReleaseDetainedLicenseApplication().ShowDialog();

            _RefershDetainedLIcensesList();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int licenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;

            new frmReleaseDetainedLicenseApplication(licenseID).ShowDialog();

            _RefershDetainedLIcensesList();
        }
    }
}
