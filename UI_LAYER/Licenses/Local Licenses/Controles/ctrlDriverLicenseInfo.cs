using LOGIC_LAYER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_LAYER.Global_Classes;
using UI_LAYER.Properties;

namespace UI_LAYER.Licenses.Local_Licenses.Controles
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private clsLicense _License;
        public clsLicense SelectedLicenseInfo
        { get { return _License; } }
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        public void LoadInfo(int licenseID)
        {
            _License = clsLicense.Find(licenseID);
            if (_License == null)
            {
                MessageBox.Show("Could not find License ID = " + licenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                licenseID = -1;
                return;
            }

            lblLicenseID.Text = _License.LicenseID.ToString();
            lblIsActive.Text = _License.IsActive ? "Yes" : "No";
            lblIsDetained.Text = _License.IsDetained ? "Yes" : "No";
            lblClass.Text = _License.LicenseClassInfo.ClassName;
            lblFullName.Text = _License.DriverInfo.PersonInfo.FullName;
            lblNationalNo.Text = _License.DriverInfo.PersonInfo.NationalNo;
            lblGendor.Text = _License.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblDateOfBirth.Text = clsFormat.DateToShort(_License.DriverInfo.PersonInfo.DateOfBirth);

            lblDriverID.Text = _License.DriverID.ToString();
            lblIssueDate.Text = clsFormat.DateToShort(_License.IssueDate);
            lblExpirationDate.Text = clsFormat.DateToShort(_License.ExpirationDate);
            lblIssueReason.Text = _License.IssueReasonText;
            lblNotes.Text = _License.Notes == "" ? "No Notes" : _License.Notes;

            _LoadPersonImage();
        }

        private void _LoadPersonImage()
        {
            string ImagePath = _License.DriverInfo.PersonInfo.ImagePath;

            try
            {
                pbPersonImage.Load(ImagePath);
            }
            catch
            {
                if (_License.DriverInfo.PersonInfo.Gendor == 0)
                {
                    pbPersonImage.Image = Resources.Male_512;
                    pbGendor.Image = Resources.Man_32;
                }
                else
                {
                    pbPersonImage.Image = Resources.Female_512;
                    pbGendor.Image = Resources.Woman_32;
                }
            }
        }

       
    }
}
