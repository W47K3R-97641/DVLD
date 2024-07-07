using LOGIC_LAYER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_LAYER.Global_Classes;
using UI_LAYER.Pepole;

namespace UI_LAYER
{
    public partial class ctrlPersonInfoWithFilter : UserControl
    {

        // Define a custom event handler delegate with parameters
        public event Action<int> OnPersonSelectedChange;
        // Create a protected method to raise the event with a parameter
        protected virtual void PersonSelectedChange(int PersonID)
        {
            Action<int> handler = OnPersonSelectedChange;
            if (handler != null)
            {
                handler(PersonID); // Raise the event with the parameter
            }
        }

        private enum enFindBy : byte { NationalNo = 0, PersonID = 1 };
        private enFindBy _findBy;



        public clsPerson selectedPersonInfo
        {
            get { return ctrlPersonInfo1.SelectedPersonInfo; }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }

        public ctrlPersonInfoWithFilter()
        {
            InitializeComponent();
        }


        public bool LoadPersonInfo(int personID)
        {
            if (ctrlPersonInfo1.LoadPersonInfo(personID))
            {
                txtbxFindByValue.Text = personID.ToString();
                cmbFindBy.SelectedIndex = 1;
                return true;
            }
            return false;
            
        }


        private void cmbFindBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            _findBy = (enFindBy)cmbFindBy.SelectedIndex;
        }

       

        private void ctrlPersonInfoWithFilter_Load(object sender, EventArgs e)
        {
            cmbFindBy.SelectedIndex = ((int)enFindBy.NationalNo);
            _findBy = enFindBy.NationalNo;
            txtbxFindByValue.Focus();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
               
            FindNow();
        }

        private void FindNow()
        {
            if (txtbxFindByValue.Text == "") return;

            bool IsPersonSelected = false;
            switch (_findBy)
            {
                case enFindBy.NationalNo:
                    IsPersonSelected = ctrlPersonInfo1.LoadPersonInfo(txtbxFindByValue.Text.Trim());
                    break;
                case enFindBy.PersonID:
                    IsPersonSelected = ctrlPersonInfo1.LoadPersonInfo(int.Parse(txtbxFindByValue.Text));
                    break;
            }

            
            
            if (OnPersonSelectedChange != null && FilterEnabled)
                PersonSelectedChange(ctrlPersonInfo1.PersonID);
            
        }
            
               

        private void txtbxFindByValue_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtbxFindByValue.Text.Trim()))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txtbxFindByValue, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtbxFindByValue, null);
            }
        }

        private void txtbxFindByValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter) 
            {
                btnFind.PerformClick();
            }


            if (_findBy == enFindBy.PersonID) 
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frmAddUpdatePerson = new frmAddUpdatePerson();
            frmAddUpdatePerson.DataBack += DataBackEvent;
            frmAddUpdatePerson.ShowDialog();
        }

        public void DataBackEvent(object sender, clsPerson newPerson)
        {
            cmbFindBy.SelectedIndex = 1;

            txtbxFindByValue.Text = newPerson.PersonID.ToString();

            errorProvider1.SetError(txtbxFindByValue, null);

            bool IsPersonAdded = ctrlPersonInfo1.LoadPersonInfo(newPerson.PersonID);

            
            if (OnPersonSelectedChange != null)
                PersonSelectedChange(ctrlPersonInfo1.PersonID);
            


        }

        public void FilterFocus()
        {
            txtbxFindByValue.Focus();
        }
           
    }

}
;