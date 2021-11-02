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
                            Lat = Convert.ToDouble( splited[2] ),
                            Lng = Convert.ToDouble(splited[3])
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

            foreach (Place place in GlobalVariables.PlacesList)
            {
                dtPlaces.Rows.Add(place.Id, place.Name, place.Lat, place.Lng);
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
                            DistanceKm = Convert.ToDouble (splited[5])
                        });

                    }//END FOR
                }
            }//END IF

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

            DataTable dtRouteActive = new DataTable();
            dtRouteActive.Columns.Add("Id");
            dtRouteActive.Columns.Add("State");

            dtRouteActive.Rows.Add("1", "Habilitada");
            dtRouteActive.Rows.Add("0", "Inhabilitada");

            cbRouteActive.DataSource = dtRouteActive;
            cbRouteActive.DisplayMember = "State";
            cbRouteActive.ValueMember = "Id";

        }//END FUNCTION

        private void btnPlaceSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //VALIDATION
            string err = string.Empty;

            if(tbPlaceName.Text.Trim().Length == 0)
            {
                err += "- Ingresa el nombre del lugar";
            }//EN IF

            if (err.Trim().Length > 0) {
                MessageBox.Show(err, "ERROR DE VALIDACION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }//END IF

            try {
                string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Place_File;

                if (!File.Exists(path))
                { // Create a file to write to   
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        // ID ; USERNAME ; PASSWORD ; TYPE ; ACTIVE
                        // TYPE 1 = Admin, 2 = User
                        // ACTIVE 1 = Active, 2 = Inactive
                        sw.WriteLine($"1;{ tbPlaceName.Text.Trim()};{ tbPlaceLat.Text.Trim() };{ tbPlaceLng.Text.Trim() }");
                        GlobalVariables.PlacesList.Add(new Place() { Id = 1, Name = tbPlaceName.Text, Lat = Convert.ToInt16( tbPlaceLat.Text ), Lng = Convert.ToInt16( tbPlaceLng.Text ) });
                    }
                }
                else {
                    int lines = System.IO.File.ReadAllLines(path).Length + 1;
                    // id; name; lat; lng
                    StreamWriter file = new StreamWriter(path, append: true);
                    file.WriteLine($"{ lines };{ tbPlaceName.Text.Trim()};{ tbPlaceLat.Text.Trim() };{ tbPlaceLng.Text.Trim() }");
                    file.Close();

                    GlobalVariables.PlacesList.Add(new Place() { Id = lines, Name = tbPlaceName.Text, Lat = Convert.ToInt16(tbPlaceLat.Text), Lng = Convert.ToInt16(tbPlaceLng.Text) });
                }//END IF

                

                MessageBox.Show("Lugar creado con éxito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

                tbPlaceName.Text = string.Empty;
                tbPlaceLat.Text = tbPlaceLng.Text = "0";
                //REALOAD DATA
                LoadPlacesData();
            }catch(Exception x)
            {
                MessageBox.Show(x.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

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

            try
            {
                string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Route_File;

                if (!File.Exists(path))
                { // Create a file to write to   
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        // ID ; FROM_ID ; FROM; TO_ID ; TO; DISTANCE ; ACTIVE
                        sw.WriteLine($"1;{ cbRouteFrom.SelectedValue.ToString().Trim()};{ cbRouteFrom.Text.ToString().Trim()};{ cbRouteTo.SelectedValue.ToString().Trim() };{ cbRouteTo.Text.ToString().Trim()};{ tbRouteDistanceKm.Text.Trim() };{ cbRouteActive.SelectedValue.ToString().Trim()}");
                        GlobalVariables.RoutesList.Add(new Route() { Id = 1, From_Id = Convert.ToInt16( cbRouteFrom.SelectedValue ), From = cbRouteFrom.Text, To_Id = Convert.ToInt16( cbRouteTo.SelectedValue ), To = cbRouteTo.Text, DistanceKm = Convert.ToDouble( tbRouteDistanceKm.Text ), Active = Convert.ToInt16( cbRouteActive.SelectedValue.ToString().Trim()) });
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
        }//END FUNCTION
    }
}
