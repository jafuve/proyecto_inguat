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
            //USERS
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

            //PLACES
            DataTable dtPlaces = new DataTable();
            dtPlaces.Columns.Add("Código");
            dtPlaces.Columns.Add("Nombre");
            dtPlaces.Columns.Add("Latitud");
            dtPlaces.Columns.Add("Longitud");

            foreach (Place place in GlobalVariables.PlacesList)
            {
                dtUsers.Rows.Add(place.Id, place.Name, place.Lat, place.Lng);
            }//END FOREACH

            dgvPlaces.DataSource = dtPlaces;

            //ROUTES
            DataTable dtRoutes = new DataTable();
            dtRoutes.Columns.Add("Código");
            dtRoutes.Columns.Add("Desde");
            dtRoutes.Columns.Add("Hasta");
            dtRoutes.Columns.Add("DistanciaKm");

            foreach (Route route in GlobalVariables.RoutesList)
            {
                dtRoutes.Rows.Add(route.Id, route.From, route.To, route.DistanceKm);
            }//END FOREACH

            dgvRoutes.DataSource = dtRoutes;

        }//END FUNCTION
    }
}
