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
using UI_LAYER.Global_Classes;
using UI_LAYER.Properties;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;


namespace UI_LAYER.Pepole
{
    public partial class frmAddUpdatePerson : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, clsPerson newPerson);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode : byte { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enGendor { Male = 0, Female = 1 };
        private clsPerson _person;
        private int _PersonID = -1;

        public frmAddUpdatePerson()
        {
            InitializeComponent();

            Mode = enMode.AddNew;
        }
        public frmAddUpdatePerson(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
            Mode = enMode.Update;
        }
        private bool _HandlePersonImage()
        {
            if (_person.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_person.ImagePath != null && File.Exists(_person.ImagePath))
                {

                    try
                    {
                        File.Delete(_person.ImagePath);
                    }
                    catch(IOException)
                    {
                        // We could not delete the file.
                        //log it later   
                    }
                }

                if (pbPersonImage.ImageLocation != null)
                {
                    
                   string SourceImageFile = pbPersonImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile) )
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }
        private void _ResetDefualtValues()
        {
            if (Mode == enMode.AddNew)
            {
                lblModeTitle.Text = "Add New Person";
                _person = new clsPerson();
            }
            else
                lblModeTitle.Text = "Update Person";
            
            //this will initialize the reset the defaule values
            FillCountriesComboBox();
            
            //hide/show the remove linke incase there is no image for the person.
            lblRemoveImage.Visible = (pbPersonImage.ImageLocation != null);

            //set default image for the person.
            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;


            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
        }
        private void FillCountriesComboBox()
        {
            DataTable countriesTable = clsCountries.GetAllCountries();

            foreach (DataRow row in countriesTable.Rows)
            {
                cmbCountries.Items.Add(row["CountryName"]);
            }

            cmbCountries.SelectedIndex = 2;
        }
        private void _LoadData()
        {
            _person = clsPerson.Find(_PersonID);

            if (_person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            //the following code will not be executed if the person was not found
            lblPersonID.Text = _PersonID.ToString();
            txtbxFirstname.Text = _person.FirstName;
            txtbxSecondname.Text = _person.SecondName;
            txtbxThirdname.Text = _person.ThirdName;
            txtbxLastname.Text = _person.LastName;
            txtbxNatonalNo.Text = _person.NationalNo;
            dtpDateOfBirth.Value = _person.DateOfBirth;
            txtAddress.Text = _person.Address;
            txtbxPhone.Text = _person.Phone;
            txtEmail.Text = _person.Email;
            cmbCountries.SelectedIndex = cmbCountries.FindString(_person.CountryInfo.countryName);

            if (_person.Gendor == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            //load person image incase it was set.
            if (_person.ImagePath != "")
            {
                pbPersonImage.ImageLocation = _person.ImagePath;

            }

            //hide/show the remove linke incase there is no image for the person.
            lblRemoveImage.Visible = (_person.ImagePath != "");

        }
        
        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (Mode == enMode.Update)
                _LoadData();
        }
        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            //set default image for the person.
            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;
        }
        private void textBoxName_MouseEnter(object sender, EventArgs e)
        {
            TextBox txtbxName = ((TextBox)sender);

            toolTip1.SetToolTip(txtbxName, "Your " + txtbxName.Tag.ToString());
        }
        private void NameValidating(TextBox txtbxName, bool requird)
        {

            if (String.IsNullOrEmpty(txtbxName.Text.Trim()) && requird == true)
            {
                errorProvider1.SetError(txtbxName, "This Filed Requird Should Fill it");
                return;
            }
            else
            {
                errorProvider1.SetError(txtbxName, null);
            }


            if (String.IsNullOrEmpty(txtbxName.Text.Trim()))
                return;

            if (!clsValidation.ValidateName(txtbxName.Text.Trim()))
            {
                errorProvider1.SetError(txtbxName, "Invalid Name Format!");
            }
            else
            {
                errorProvider1.SetError(txtbxName, null);
            }
        }       
        private void txtbxNatonalNo_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtbxNatonalNo.Text.Trim()))
            {
                errorProvider1.SetError(txtbxNatonalNo, "This Filed Requird Should Fill it");
                return;
            }
            else
            {
                errorProvider1.SetError(txtbxNatonalNo, null);
            }

            if (clsPerson.IsExist(txtbxNatonalNo.Text.Trim()) && _person.NationalNo != txtbxNatonalNo.Text.Trim())
            {
                errorProvider1.SetError(txtbxNatonalNo, "National No. Is Used For Another Person!");
            }
            else
            {
                errorProvider1.SetError(txtbxNatonalNo, null);
            }
        }
        private void Name_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtbxName = ((TextBox)sender);
            
            if (txtbxName.Tag.ToString() == "Secondname" || txtbxName.Tag.ToString() == "Thirdname")
            {
                NameValidating(txtbxName, false);
            }
            else
            {
                NameValidating(txtbxName, true);

            }
        }
        private void txtbxPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxPhone.Text.Trim()))
            {
                errorProvider1.SetError(txtbxPhone, "This Filed Requird Should Fill it");
                return;
            }
            else
            {
                errorProvider1.SetError(txtbxPhone, null);
            }

            if (!clsValidation.ValidatePhoneNumber(txtbxPhone.Text.Trim()))
            {
                errorProvider1.SetError(txtbxPhone, "Invalid Phone Number!");
            }
            else
            {
                errorProvider1.SetError(txtbxPhone, null);

            }
        }
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            //no need to validate the email incase it's empty.
            if (txtEmail.Text.Trim() == "")
            {
                errorProvider1.SetError(txtEmail, null);
                return;

            }

            if (!clsValidation.ValidateEmail(txtEmail.Text.Trim()))
            {
                errorProvider1.SetError(txtEmail, "Invalid Email!");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);

            }
            
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPersonImage.Load(openFileDialog1.FileName);
                lblRemoveImage.Visible = true;
            }
        }
        private void lblRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            lblRemoveImage.Visible = false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (!_HandlePersonImage())
                return;

            if (rbMale.Checked)
                _person.Gendor = (byte)enGendor.Male;
            else
                _person.Gendor = (byte)enGendor.Female;

            if (pbPersonImage.ImageLocation != null)
                _person.ImagePath = pbPersonImage.ImageLocation;
            else
                _person.ImagePath = "";

            clsCountries countrySelected = clsCountries.Find(cmbCountries.Text);

            _person.FirstName   = txtbxFirstname.Text.Trim();
            _person.SecondName  = txtbxSecondname.Text.Trim();
            _person.ThirdName   = txtbxThirdname.Text.Trim();
            _person.LastName    = txtbxLastname.Text.Trim();
            _person.NationalNo  = txtbxNatonalNo.Text.Trim();
            _person.Email       = txtEmail.Text.Trim();
            _person.Phone       = txtbxPhone.Text.Trim();
            _person.Address     = txtAddress.Text.Trim();
            _person.DateOfBirth = dtpDateOfBirth.Value;
            _person.NationalityCountryID = countrySelected.countryID;
            _person.CountryInfo = countrySelected;



            if (_person.Save())
            {
                lblPersonID.Text = _person.PersonID.ToString();
                //change form mode to update.
                Mode = enMode.Update;
                lblModeTitle.Text = "Update Person";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _person);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }       
}