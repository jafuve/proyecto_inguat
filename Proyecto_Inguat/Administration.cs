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
            //load main settings
            LoadMainSettings();

            //LOAD DATA IN GRIDVIEWS
            LoadMainData();

            //LOAD STADISTICS
            LoadStadistics();
        }

        private void LoadMainData()
        {
            LoadUsersData();


            DataTable dtUserActive = new DataTable();
            dtUserActive.Columns.Add("Id");
            dtUserActive.Columns.Add("State");

            dtUserActive.Rows.Add("1", "Habilitada");
            dtUserActive.Rows.Add("0", "Inhabilitada");

            cbUserActive.DataSource = dtUserActive;
            cbUserActive.DisplayMember = "State";
            cbUserActive.ValueMember = "Id";

            LoadPlacesData();

            LoadRoutesData();

        }//END FUNCTION

        private void LoadPlacesData()
        {
            if (GlobalVariables.PlacesList.Count == 0)
            {
                string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Place_File;

                string[] lines = null;

                if (!File.Exists(path))
                { // Create a file to write to   
                    //using (StreamWriter sw = File.CreateText(path))
                    //{
                    //    // ID ; USERNAME ; PASSWORD ; TYPE ; ACTIVE
                    //    // TYPE 1 = Admin, 2 = User
                    //    // ACTIVE 1 = Active, 2 = Inactive
                    //    sw.WriteLine("1;admin;admin;1;1");
                    //    lines = new string[1];
                    //    lines[0] = "1;admin;admin;1;1";
                    //}
                }
                else
                {
                    //LOAD DATA IN GLOBAL VARIABLE
                    lines = System.IO.File.ReadAllLines(path);
                }//END IF

                GlobalVariables.PlacesList = new List<Place>();

                if (lines != null)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] splited = lines[i].Split(';');

                        GlobalVariables.PlacesList.Add(new Place()
                        {
                            Id = Convert.ToInt16(splited[0]),
                            Name = splited[1].ToString(),
                            Lat = Convert.ToDouble(splited[2]),
                            Lng = Convert.ToDouble(splited[3]),
                            Active = Convert.ToInt16(splited[4])
                        });

                    }//END FOR
                }
            }//END IF

            //PLACES
            DataTable dtPlaces = new DataTable();
            dtPlaces.Columns.Add("Código");
            dtPlaces.Columns.Add("Nombre");
            dtPlaces.Columns.Add("Latitud");
            dtPlaces.Columns.Add("Longitud");
            dtPlaces.Columns.Add("Activo");

            foreach (Place place in GlobalVariables.PlacesList)
            {
                dtPlaces.Rows.Add(place.Id, place.Name, place.Lat, place.Lng, place.Active);
            }//END FOREACH

            dgvPlaces.DataSource = dtPlaces;

            cbRouteFrom.DataSource = dtPlaces;
            cbRouteFrom.BindingContext = new BindingContext();
            cbRouteFrom.DisplayMember = "Nombre";
            cbRouteFrom.ValueMember = "Código";

            cbRouteTo.DataSource = dtPlaces;
            cbRouteTo.BindingContext = new BindingContext();
            cbRouteTo.DisplayMember = "Nombre";
            cbRouteTo.ValueMember = "Código";

        }//END FUNCTION

        private void LoadRoutesData()
        {
            if (GlobalVariables.RoutesList.Count == 0)
            {
                string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Route_File;

                string[] lines = null;

                if (!File.Exists(path))
                { // Create a file to write to   
                    //using (StreamWriter sw = File.CreateText(path))
                    //{
                    //    // ID ; USERNAME ; PASSWORD ; TYPE ; ACTIVE
                    //    // TYPE 1 = Admin, 2 = User
                    //    // ACTIVE 1 = Active, 2 = Inactive
                    //    sw.WriteLine("1;admin;admin;1;1");
                    //    lines = new string[1];
                    //    lines[0] = "1;admin;admin;1;1";
                    //}
                }
                else
                {
                    //LOAD DATA IN GLOBAL VARIABLE
                    lines = System.IO.File.ReadAllLines(path);
                }//END IF

                GlobalVariables.RoutesList = new List<Route>();

                if (lines != null)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] splited = lines[i].Split(';');

                        GlobalVariables.RoutesList.Add(new Route()
                        {
                            Id = Convert.ToInt16(splited[0]),
                            From_Id = Convert.ToInt16(splited[1]),
                            From = splited[2],
                            To_Id = Convert.ToInt16(splited[3]),
                            To = splited[4],
                            DistanceKm = Convert.ToDouble(splited[5])
                        });

                    }//END FOR
                }
            }//END IF

            //ROUTES
            DataTable dtRoutes = new DataTable();
            dtRoutes.Columns.Add("Código");
            dtRoutes.Columns.Add("DesdeID");
            dtRoutes.Columns.Add("Desde");
            dtRoutes.Columns.Add("HastaID");
            dtRoutes.Columns.Add("Hasta");
            dtRoutes.Columns.Add("DistanciaKm");
            dtRoutes.Columns.Add("Activo");

            foreach (Route route in GlobalVariables.RoutesList)
            {
                dtRoutes.Rows.Add(route.Id, route.From_Id, route.From, route.To_Id, route.To, route.DistanceKm, route.Active);
            }//END FOREACH

            dgvRoutes.DataSource = dtRoutes;

        }//END FUNCTION

        private void LoadUsersData()
        {
            if (GlobalVariables.PlacesList.Count == 0)
            {
                string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_User_File;

                string[] lines = null;

                if (!File.Exists(path))
                { // Create a file to write to   
                    //using (StreamWriter sw = File.CreateText(path))
                    //{
                    //    // ID ; USERNAME ; PASSWORD ; TYPE ; ACTIVE
                    //    // TYPE 1 = Admin, 2 = User
                    //    // ACTIVE 1 = Active, 2 = Inactive
                    //    sw.WriteLine("1;admin;admin;1;1");
                    //    lines = new string[1];
                    //    lines[0] = "1;admin;admin;1;1";
                    //}
                }
                else
                {
                    //LOAD DATA IN GLOBAL VARIABLE
                    lines = System.IO.File.ReadAllLines(path);
                }//END IF

                GlobalVariables.UsersList = new List<User>();

                if (lines != null)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] splited = lines[i].Split(';');

                        GlobalVariables.UsersList.Add(new User()
                        {
                            Id = Convert.ToInt16(splited[0]),
                            Username = splited[1].ToString(),
                            Password = splited[2].ToString(),
                            Type = Convert.ToInt16(splited[3]),
                            Active = Convert.ToInt16(splited[4])
                        });

                    }//END FOR
                }
            }//END IF

            DataTable dtUsers = new DataTable();
            dtUsers.Columns.Add("Código");
            dtUsers.Columns.Add("Usuario");
            dtUsers.Columns.Add("Contraseña");
            dtUsers.Columns.Add("Tipo");
            dtUsers.Columns.Add("Activo");

            foreach (User user in GlobalVariables.UsersList)
            {
                dtUsers.Rows.Add(user.Id, user.Username, user.Password, user.Type, user.Active);
            }//END FOREACH

            dgvUsers.DataSource = dtUsers;

        }//END FUNCTION

        private void btnPlaceSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //VALIDATION
            string err = string.Empty;

            if (tbPlaceName.Text.Trim().Length == 0)
            {
                err += "- Ingresa el nombre del lugar";
            }//EN IF

            if (err.Trim().Length > 0) {
                MessageBox.Show(err, "ERROR DE VALIDACION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }//END IF

            if (placeEditId == -1)
            {

                try
                {
                    string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Place_File;

                    if (!File.Exists(path))
                    { // Create a file to write to   
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            // ID ; NAME ; LAT ; LNG ; ACTIVE
                            sw.WriteLine($"1;{ tbPlaceName.Text.Trim()};{ tbPlaceLat.Text.Trim() };{ tbPlaceLng.Text.Trim() };{ cbPlaceActive.SelectedValue }");
                            GlobalVariables.PlacesList.Add(new Place() { Id = 1, Name = tbPlaceName.Text, Lat = Convert.ToInt16(tbPlaceLat.Text), Lng = Convert.ToInt16(tbPlaceLng.Text), Active = Convert.ToInt16(cbPlaceActive.SelectedValue) });
                        }
                    }
                    else
                    {
                        int lines = System.IO.File.ReadAllLines(path).Length + 1;
                        // id; name; lat; lng
                        StreamWriter file = new StreamWriter(path, append: true);
                        file.WriteLine($"{ lines };{ tbPlaceName.Text.Trim()};{ tbPlaceLat.Text.Trim() };{ tbPlaceLng.Text.Trim() };{ cbPlaceActive.SelectedValue }");
                        file.Close();

                        GlobalVariables.PlacesList.Add(new Place() { Id = lines, Name = tbPlaceName.Text, Lat = Convert.ToInt16(tbPlaceLat.Text), Lng = Convert.ToInt16(tbPlaceLng.Text), Active = Convert.ToInt16(cbPlaceActive.SelectedValue) });
                    }//END IF

                    MessageBox.Show("Lugar creado con éxito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    tbPlaceName.Text = string.Empty;
                    tbPlaceLat.Text = tbPlaceLng.Text = "0";
                    //REALOAD DATA
                    LoadPlacesData();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else {
                //edit
                dgvPlaces.Rows[placeEditIdx].Cells["Activo"].Value = cbPlaceActive.SelectedValue.ToString();

                //
                string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Place_File;

                StreamWriter file = new StreamWriter(path, append: false);

                for (int i = 0; i < dgvPlaces.Rows.Count-1; i++) {
                    file.WriteLine($"{ dgvPlaces.Rows[i].Cells[0].Value };{ dgvPlaces.Rows[i].Cells[1].Value };{ dgvPlaces.Rows[i].Cells[2].Value };{ dgvPlaces.Rows[i].Cells[3].Value };{ dgvPlaces.Rows[i].Cells[4].Value }");
                }//END FOR
                file.Close();

                placeEditId = -1;

                MessageBox.Show("Lugar modificado con éxito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
            }//END IF

        }//END FUNCTION

        private void btnRouteSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //VALIDATION
            string err = string.Empty;

            if (tbRouteDistanceKm.Text.Trim().Length == 0)
            {
                err += "- Ingresa la distancia entre lugares.";
            }//EN IF

            if (err.Trim().Length > 0)
            {
                MessageBox.Show(err, "ERROR DE VALIDACION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }//END IF

            if (routeEditId == -1)
            {
                try
                {
                    string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Route_File;

                    if (!File.Exists(path))
                    { // Create a file to write to   
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            // ID ; FROM_ID ; FROM; TO_ID ; TO; DISTANCE ; ACTIVE
                            sw.WriteLine($"1;{ cbRouteFrom.SelectedValue.ToString().Trim()};{ cbRouteFrom.Text.ToString().Trim()};{ cbRouteTo.SelectedValue.ToString().Trim() };{ cbRouteTo.Text.ToString().Trim()};{ tbRouteDistanceKm.Text.Trim() };{ cbRouteActive.SelectedValue.ToString().Trim()}");
                            GlobalVariables.RoutesList.Add(new Route() { Id = 1, From_Id = Convert.ToInt16(cbRouteFrom.SelectedValue), From = cbRouteFrom.Text, To_Id = Convert.ToInt16(cbRouteTo.SelectedValue), To = cbRouteTo.Text, DistanceKm = Convert.ToDouble(tbRouteDistanceKm.Text), Active = Convert.ToInt16(cbRouteActive.SelectedValue.ToString().Trim()) });
                        }
                    }
                    else
                    {
                        int lines = System.IO.File.ReadAllLines(path).Length + 1;
                        // id; name; lat; lng
                        StreamWriter file = new StreamWriter(path, append: true);
                        file.WriteLine($"{ lines };{ cbRouteFrom.SelectedValue.ToString().Trim()};{ cbRouteFrom.Text.ToString().Trim()};{ cbRouteTo.SelectedValue.ToString().Trim() };{ cbRouteTo.Text.ToString().Trim()};{ tbRouteDistanceKm.Text.Trim() };{ cbRouteActive.SelectedValue.ToString().Trim()}");
                        file.Close();

                        GlobalVariables.RoutesList.Add(new Route() { Id = lines, From_Id = Convert.ToInt16(cbRouteFrom.SelectedValue), From = cbRouteFrom.Text, To_Id = Convert.ToInt16(cbRouteTo.SelectedValue), To = cbRouteTo.Text, DistanceKm = Convert.ToDouble(tbRouteDistanceKm.Text), Active = Convert.ToInt16(cbRouteActive.SelectedValue.ToString().Trim()) });
                    }//END IF



                    MessageBox.Show("Ruta creada con éxito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    tbRouteDistanceKm.Text = "0";
                    //REALOAD DATA
                    LoadRoutesData();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else {
                //edit
                dgvRoutes.Rows[routeEditIdx].Cells["Activo"].Value = cbRouteActive.SelectedValue.ToString();

                //
                string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Route_File;

                StreamWriter file = new StreamWriter(path, append: false);

                for (int i = 0; i < dgvRoutes.Rows.Count - 1; i++)
                {
                    file.WriteLine($"{ dgvRoutes.Rows[i].Cells[0].Value };{ dgvRoutes.Rows[i].Cells[1].Value };{ dgvRoutes.Rows[i].Cells[2].Value };{ dgvRoutes.Rows[i].Cells[3].Value };{ dgvRoutes.Rows[i].Cells[4].Value };{ dgvRoutes.Rows[i].Cells[5].Value };{ dgvRoutes.Rows[i].Cells[6].Value }");
                }//END FOR
                file.Close();

                routeEditId = -1;

                MessageBox.Show("Ruta modificado con éxito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
            }//END IF
        }//END FUNCTION

        private void btnUserSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //VALIDATION
            string err = string.Empty;

            if (tbUserUsername.Text.Trim().Length == 0)
            {
                err += "- Ingresa el nombre de usuario.";
            }//EN IF

            if (err.Trim().Length > 0)
            {
                MessageBox.Show(err, "ERROR DE VALIDACION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }//END IF

            try
            {
                string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_User_File;

                if (!File.Exists(path))
                { // Create a file to write to   
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        // ID ; USERNAME ; PASSWORD; TYPE ; ACTIVE
                        sw.WriteLine($"1;{ tbUserUsername.Text.Trim()};{ tbUserPassword.Text.Trim()};1;{ cbUserActive.SelectedValue.ToString().Trim()}");
                        GlobalVariables.UsersList.Add(new User() { Id = 1, Username = tbUserUsername.Text.Trim(), Password = tbUserPassword.Text, Type = 1, Active = Convert.ToInt16(cbUserActive.SelectedValue) });
                    }
                }
                else
                {
                    int lines = System.IO.File.ReadAllLines(path).Length + 1;
                    // id; name; lat; lng
                    StreamWriter file = new StreamWriter(path, append: true);
                    file.WriteLine($"{ lines };{ tbUserUsername.Text.Trim()};{ tbUserPassword.Text.Trim()};1;{ cbUserActive.SelectedValue.ToString().Trim()}");
                    file.Close();

                    GlobalVariables.UsersList.Add(new User() { Id = lines, Username = tbUserUsername.Text.Trim(), Password = tbUserPassword.Text, Type = 1, Active = Convert.ToInt16(cbUserActive.SelectedValue) });
                }//END IF



                MessageBox.Show("Usuario creado con éxito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                tbUserUsername.Text = tbUserPassword.Text = string.Empty;
                //REALOAD DATA
                LoadUsersData();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }//END FUNCTION

        private void LoadMainSettings() {
            //LOAD ACTIVE COMBOBOXES
            DataTable cbActive = new DataTable();
            cbActive.Columns.Add("Id");
            cbActive.Columns.Add("State");

            cbActive.Rows.Add("1", "Habilitada");
            cbActive.Rows.Add("0", "Inhabilitada");

            //COMBOBO PLACES
            cbPlaceActive.DataSource = cbActive;
            cbPlaceActive.BindingContext = new BindingContext();
            cbPlaceActive.DisplayMember = "State";
            cbPlaceActive.ValueMember = "Id";

            //COMBOBOX ROUTES
            cbRouteActive.DataSource = cbActive;
            cbRouteActive.BindingContext = new BindingContext();
            cbRouteActive.DisplayMember = "State";
            cbRouteActive.ValueMember = "Id";

            //COMBOBOX USERS
            cbUserActive.DataSource = cbActive;
            cbUserActive.BindingContext = new BindingContext();
            cbUserActive.DisplayMember = "State";
            cbUserActive.ValueMember = "Id";

            //DGV SETTINGS
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlaces.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoutes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        }//END FUNCTION


        int placeEditId = -1, placeEditIdx = -1;
        int routeEditId = -1, routeEditIdx = -1;

        private void btnNewPlace_Click(object sender, EventArgs e)
        {
            tbPlaceName.Text = string.Empty;
            tbPlaceLat.Text = tbPlaceLng.Text = "0";
            cbPlaceActive.SelectedValue = "1";
            placeEditId = placeEditIdx = -1;
        }

        private void btnNewRoute_Click(object sender, EventArgs e)
        {
            routeEditId = routeEditIdx = -1;
            tbRouteDistanceKm.Text = "0";
        }

        private void btnNewUser_Click(object sender, EventArgs e)
        {
            tbUserPassword.Text = tbUserUsername.Text = string.Empty;
        }

        private void dgvRoutes_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = routeEditIdx = dgvRoutes.CurrentRow.Index;
            routeEditId = Convert.ToInt16(dgvRoutes.Rows[rowindex].Cells["Código"].Value);
            cbRouteFrom.SelectedValue = Convert.ToInt16(dgvRoutes.Rows[rowindex].Cells["DesdeId"].Value);
            cbRouteTo.SelectedValue = Convert.ToInt16(dgvRoutes.Rows[rowindex].Cells["HastaId"].Value);
            tbRouteDistanceKm.Text = dgvRoutes.Rows[rowindex].Cells["DistanciaKm"].Value.ToString();
            cbRouteActive.SelectedValue = dgvRoutes.Rows[rowindex].Cells["Activo"].Value.ToString();
        }

        private void dgvPlaces_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = placeEditIdx = dgvPlaces.CurrentRow.Index;
            placeEditId = Convert.ToInt16(dgvPlaces.Rows[rowindex].Cells["Código"].Value);
            tbPlaceName.Text = dgvPlaces.Rows[rowindex].Cells["Nombre"].Value.ToString();
            cbPlaceActive.SelectedValue = dgvPlaces.Rows[rowindex].Cells["Activo"].Value.ToString();
        }

        private void LoadStadistics() {

            //FILL LIST WITH DATA
            #region LOAD DATA
            string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Stadistics_File;

            string[] lines = null;

            if (!File.Exists(path))
            { // Create a file to write to   
              
            }
            else
            {
                //LOAD DATA IN GLOBAL VARIABLE
                lines = System.IO.File.ReadAllLines(path);
            }//END IF

            GlobalVariables.StadisticsList = new List<Stadistics>();

            if (lines != null)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] splited = lines[i].Split(';');

                    if ( Convert.ToInt16( splited[0] ) == 1)
                    {
                        GlobalVariables.Visitors++;
                    }
                    else
                    {
                        GlobalVariables.StadisticsList.Add(new Stadistics()
                        {
                            Type = Convert.ToInt16(splited[0]),
                            PlaceId = Convert.ToInt16(splited[1]),
                            PlaceName = splited[2].ToString()
                        });
                    }//END IF

                }//END FOR
            }
            #endregion

            #region SHOW DATA

            var list = GlobalVariables.StadisticsList
                        .GroupBy(x => x.PlaceName)
                        .Select(y => new { PlaceId = y.Key, Visits = y.Count() })
                        .OrderByDescending(z => z.Visits)
                        .ToList();

            tbStadisticsVisitors.Text = GlobalVariables.Visitors.ToString();

            dgvPlacesFavorites.DataSource = list;

            #endregion

        }//END FUNCTION
    }
}
