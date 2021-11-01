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
    public partial class Administration : Form
    {
        public Administration()
        {
            InitializeComponent();
        }

        private void Administration_Load(object sender, EventArgs e)
        {
            //MAIN SETTINGS
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //LOAD DATA IN GRIDVIEWS
            LoadMainData();
        }

        private void LoadMainData()
        {
            DataTable dtUsers = new DataTable();
            dtUsers.Columns.Add("Código");
            dtUsers.Columns.Add("Usuario");
            dtUsers.Columns.Add("Contraseña");
            dtUsers.Columns.Add("Tipo");
            dtUsers.Columns.Add("Activo");

            foreach(User user in GlobalVariables.UsersList)
            {
                dtUsers.Rows.Add(user.Id, user.Username, user.Password, user.Type, user.Active);
            }//END FOREACH

            dgvUsers.DataSource = dtUsers;

        }//END FUNCTION
    }
}
