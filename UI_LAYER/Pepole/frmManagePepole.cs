using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LOGIC_LAYER;
using UI_LAYER.Global_Classes;

namespace UI_LAYER.Pepole
{
    public partial class frmManagePepole : Form
    {
      private static DataTable _dtAllPeople = clsPerson.GetAllPepole();
            
       
      private static DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");

        public frmManagePepole()
        {
            InitializeComponent();
        }

        private void _RefershPepoleList()
        {
            _dtAllPeople = clsPerson.GetAllPepole();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
            dgvPeople.DataSource = _dtPeople;
            
            lblRecords.Text = _dtAllPeople.Rows.Count.ToString();
        }

        private void frmManagePepole_Load(object sender, EventArgs e)
        {
           

            _RefershPepoleList();
            _SetColumnsSizes();
            cbFilterBy.SelectedIndex = 0;
        }
        
        private void _SetColumnsSizes()
        {
            if (dgvPeople.Rows.Count > 0)
            {

                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 110;

                dgvPeople.Columns[1].HeaderText = "National No.";
                dgvPeople.Columns[1].Width = 120;


                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 120;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 140;


                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 120;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 120;

                dgvPeople.Columns[6].HeaderText = "Gendor";
                dgvPeople.Columns[6].Width = 120;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 140;

                dgvPeople.Columns[8].HeaderText = "Nationality";
                dgvPeople.Columns[8].Width = 120;


                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;


                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 170;
            }
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filter = "";

            switch(cbFilterBy.Text)
            {
                case "Person ID":
                    filter = "PersonID";
                    break;
                case "National No.":
                    filter = "NationalNo";
                    break;
                case "First Name":
                    filter = "FirstName";
                    break;
                case  "Second Name":
                    filter = "SecondName";
                    break;
                case "Third Name":
                    filter = "ThirdName";
                    break;
                case "Nationality":
                    filter = "CountryName";
                    break;
                case "Gendor":
                    filter = "GendorCaption";
                    break;
                case "Phone":
                    filter = "Phone";
                    break;
                case "Email":
                    filter = "Email";
                    break;
                default:
                    filter = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || filter == "None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblRecords.Text = dgvPeople.Rows.Count.ToString();
                return;
            }

            if (filter == "PersonID")
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", filter, txtFilterValue.Text.Trim());
            }
            else
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", filter, txtFilterValue.Text.Trim());
            }

            lblRecords.Text = dgvPeople.Rows.Count.ToString();
            
        }

        private void btnAddPepole_Click(object sender, EventArgs e)
        {
            _ShowAddNewPersonfrm();
        }
        private void frmManagePepole_DataBack(object sender, clsPerson newPerson)
        {

            
            DataRow newRow = _dtPeople.NewRow();
            
            newRow["PersonID"]      = newPerson.PersonID;
            newRow["FirstName"]     = newPerson.FirstName;
            newRow["SecondName"]    = newPerson.SecondName;
            newRow["ThirdName"]     = newPerson.ThirdName;
            newRow["LastName"]      = newPerson.LastName;
            newRow["GendorCaption"] = newPerson.Gendor == 1 ? "M" : "F";
            newRow["DateOfBirth"]   = newPerson.DateOfBirth;
            newRow["CountryName"]   = newPerson.CountryInfo.countryName;
            newRow["Email"]         = newPerson.Email;
            newRow["Phone"]         = newPerson.Phone;
            newRow["NationalNo"]    = newPerson.NationalNo;

            _dtPeople.Rows.Add(newRow);
        }

        private void dgvPeople_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            _ShowPersonDetailsfrm();
        }

        private void muIt_ShowDetails_Click(object sender, EventArgs e)
        {
            _ShowPersonDetailsfrm();
        }

        private void _ShowPersonDetailsfrm()
        {
            frmShowPersonInfo frmShowPersonInfo = new frmShowPersonInfo(((int)dgvPeople.CurrentRow.Cells[0].Value));
            frmShowPersonInfo.ShowDialog();

            _RefershPepoleList();
        }

        private void muIt_AddNewPerson_Click(object sender, EventArgs e)
        {
            _ShowAddNewPersonfrm();
        }

        private void _ShowAddNewPersonfrm()
        {
            frmAddUpdatePerson frmAddUpdatePerson = new frmAddUpdatePerson();
            frmAddUpdatePerson.DataBack += frmManagePepole_DataBack;
            frmAddUpdatePerson.ShowDialog();
        }

        private void muIt_Edit_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frmAddUpdatePerson = new frmAddUpdatePerson(((int)dgvPeople.CurrentRow.Cells[0].Value));

            frmAddUpdatePerson.ShowDialog();
        }

        private void muIt_Delete_Click(object sender, EventArgs e)
        {

            if (clsUtil.ConfirmOperationMsgBx($"Are you sure you want to delete Person [{dgvPeople.CurrentRow.Cells[0].Value}]", "Confirm Delete"))
            {
                //Perform Delele and refresh
                if (clsPerson.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefershPepoleList();
                }

                else
                    MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void muIt_SendEmail_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void muIt_PhoneCall_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        
    }
}
