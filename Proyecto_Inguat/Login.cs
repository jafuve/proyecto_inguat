using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Inguat
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            string username = tbUsername.Text;
            string password = tbPassword.Text;
            bool authenticated = false;

            for (int i = 0; i < GlobalVariables.UsersList.Count; i++) {
                if( GlobalVariables.UsersList[i].Username == username && GlobalVariables.UsersList[i].Password == password)
                {
                    //LOGIN SUCCESSFULL
                    authenticated = true;
                    
                }//END IF
            }//END FOR

            if (authenticated)
            {
                DialogResult = DialogResult.OK;
            }
            else {
                MessageBox.Show("Los datos ingresados no son correctos, por favor verifícalos e intenta nuevamente");
                tbUsername.Text = tbPassword.Text = string.Empty;
                tbUsername.Focus();
            }//END IF

            this.Cursor = Cursors.Default;
            

        }
    }
}
