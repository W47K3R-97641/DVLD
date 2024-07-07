using LOGIC_LAYER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static UI_LAYER.Pepole.frmAddUpdatePerson;
using UI_LAYER.Properties;

namespace UI_LAYER.Pepole
{
    public partial class ctrlPersonInfo : UserControl
    {
        private clsPerson _person;

        private int _personID = -1;

        public int PersonID
        {
           get { return _personID; }
        }

        public clsPerson SelectedPersonInfo
        { get { return _person; } }

        public ctrlPersonInfo()
        {
            InitializeComponent();
        }

        public void ResetPersonInfo()
        {
            _personID = -1;
            lblPersonID.Text = "[????]";
            lblNationaNo.Text = "[????]";
            lblName.Text = "[????]";
            pbGendor.Image = Resources.Man_32;
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbPerson.Image = Resources.Male_512;

        }

        private void _FillPersonInfo()
        {
            _personID = _person.PersonID;

            lblName.Text      = _person.FullName;
            lblEmail.Text     = _person.Email.ToString();
            lblPhone.Text     = _person.Phone.ToString();
            lblAddress.Text   = _person.Address.ToString();
            lblPersonID.Text  = _person.PersonID.ToString();
            lblCountry.Text   = _person.CountryInfo.countryName.ToString();
            lblNationaNo.Text = _person.NationalNo.ToString();
            lblDateOfBirth.Text = _person.DateOfBirth.ToString();
            lblGendor.Text = _person.Gendor == 0 ? "Male" : "Female";
            pbGendor.Image = _person.Gendor == 0 ? Resources.Man_32 : Resources.Woman_32;
            _LoadPersonImage();

        }

        private void _LoadPersonImage()
        {
            pbPerson.ImageLocation = null;

            if (_person.ImagePath == "")
            {
                pbPerson.Image = _person.Gendor == 0 ? Resources.Male_512 : Resources.Female_512;
                return;
            }

            if (File.Exists(_person.ImagePath))
                pbPerson.ImageLocation = _person.ImagePath;
            else
                MessageBox.Show("Could not find this image: = " + _person.ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public bool LoadPersonInfo(int personID)
        {
            _person = clsPerson.Find(personID);

            if (_person == null)
            {
                ResetPersonInfo();
               
                MessageBox.Show("No Person with PersonID = " + personID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            _FillPersonInfo();
            return true;
        }

        public bool LoadPersonInfo(string NationalNo)
        {
            _person = clsPerson.Find(NationalNo);
            if (_person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with National No. = " + NationalNo.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            _FillPersonInfo();
            return true;
        }

        private void ctrlPersonInfo_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
