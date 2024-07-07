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
using UI_LAYER.Licenses.International_Licenses;
using static UI_LAYER.Applications.Local_Driving_License.frmListLocalDrivingLicesnseApplications;

namespace UI_LAYER.Applications.International_Driving_License
{
    public partial class frmListInternationalLicesnseApplications : Form
    {
        public enum enFilterInterDrivingLicenseAppBy : byte
        {
            None = 0,
            InternationalLicenseID = 1,
            ApplicationID = 2,
            DriverID = 3,
            IssuedUsingLocalLicenseID = 4,
            IsActive = 5
        }
        private DataTable _dtInternationalLicenseApplications;

        private enFilterInterDrivingLicenseAppBy _filterBy = enFilterInterDrivingLicenseAppBy.None;
        private string FilterBy
        {
            get { return Enum.GetName(typeof(enFilterInterDrivingLicenseAppBy), _filterBy); }
        }

        public frmListInternationalLicesnseApplications()
        {
            InitializeComponent();
        }

        private void _RefershInternationalLicenseApplicationsList()
        {
            _dtInternationalLicenseApplications = clsInternationalLicense.GetAllInternationalLicenses();
            dgvInternationalLicenses.DataSource = _dtInternationalLicenseApplications;
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.RowCount.ToString();
        }

        private void frmListInternationalLicesnseApplications_Load(object sender, EventArgs e)
        {
            _RefershInternationalLicenseApplicationsList();
            _SetColumnsSizes();
            cbFilterBy.SelectedIndex = (byte)enFilterInterDrivingLicenseAppBy.None;
            
        }

        private void _SetColumnsSizes()
        {
            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 160;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 150;

                dgvInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns[2].Width = 130;

                dgvInternationalLicenses.Columns[3].HeaderText = "L.License ID";
                dgvInternationalLicenses.Columns[3].Width = 130;

                dgvInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[4].Width = 180;

                dgvInternationalLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[5].Width = 180;

                dgvInternationalLicenses.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[6].Width = 120;

            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filterBy = (enFilterInterDrivingLicenseAppBy)cbFilterBy.SelectedIndex;

            txtFilterValue.Visible = _filterBy != enFilterInterDrivingLicenseAppBy.IsActive && _filterBy != enFilterInterDrivingLicenseAppBy.None;

            cbIsReleased.Visible = _filterBy == enFilterInterDrivingLicenseAppBy.IsActive;

            if (cbIsReleased.Visible )
                cbIsReleased.SelectedIndex = 0;

            _ClearFilter();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = txtFilterValue.Text.Trim();

            if (txtFilterValue.Text == "")
            {
                _ClearFilter();
                return;
            }

            _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, txtFilterValue.Text);
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.RowCount.ToString();
        }
        private void _ClearFilter()
        {
            _dtInternationalLicenseApplications.DefaultView.RowFilter = "";
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.RowCount.ToString();
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIsReleased.SelectedIndex == 0)
            {
                _ClearFilter();
                return;
            }

            if (cbIsReleased.SelectedIndex == 1)
            {
                _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, 1);
            }
            else
            {
                _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, 0);
            }

            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.RowCount.ToString();
        }

        private void btnNewApplication_Click(object sender, EventArgs e)
        {
            new frmNewInternationalLicenseApplication().ShowDialog();
        }
    }
}
