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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        int[,] RouteGraph = null;

        private void Main_Load(object sender, EventArgs e)
        {
            // VERIFY DATA FILES EXISTANCE
            VerifyDataFilesExistance();

            // LOAD PLACES IN MAIN
            RouteGraph = LoadGraph();

            //MAIN LOAD
            LoadPlacesData();
            LoadRoutesData();

            dgvPlaces.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoutes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSuggestedRoute.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        #region FUNCTIONS

        private void VerifyDataFilesExistance() {

            // USERS FILE
            string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_User_File;

            string[] linesUsers = null;

            if (!File.Exists(path))
            { // Create a file to write to   
                using (StreamWriter sw = File.CreateText(path))
                {
                    // ID ; USERNAME ; PASSWORD ; TYPE ; ACTIVE
                    // TYPE 1 = Admin, 2 = User
                    // ACTIVE 1 = Active, 2 = Inactive
                    sw.WriteLine("1;admin;admin;1;1");
                    linesUsers = new string[1];
                    linesUsers[0] = "1;admin;admin;1;1";
                }
            }
            else
            {
                //LOAD DATA IN GLOBAL VARIABLE
                linesUsers = System.IO.File.ReadAllLines(path);
            }//END IF

            for (int i = 0; i < linesUsers.Length; i++)
            {
                string[] splited = linesUsers[i].Split(';');

                GlobalVariables.UsersList.Add( new User() {
                    Id = Convert.ToInt16( splited[0] ),
                    Username = splited[1].ToString(),
                    Password = splited[2].ToString(),
                    Type = Convert.ToInt16(splited[3]),
                    Active = Convert.ToInt16(splited[4])
                });

            }//END FOR

            // USERS FILE
            //path = Directory.GetCurrentDirectory() + "/DB_Place.txt";

        }//END FUNCTION

        #endregion

        private void btnLoginAdmin_Click(object sender, EventArgs e)
        {
            Login nf = new Login();
            if (nf.ShowDialog(this) == DialogResult.OK)
            {
                Administration formAdmin = new Administration();
                formAdmin.ShowDialog(this);

                LoadRoutesData(true);
                LoadPlacesData(true);
            }
            else
            {
                MessageBox.Show("Sin acceso");
            }//END IF
        }

        //DJIKSTRA METHODS
        public static int[,] LoadGraph() {

            //GET NODE QTY
            int nodesQty = 0;
            string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Place_File;

            if (!File.Exists(path))
            { // Create a file to write to   
                
            }
            else
            {
                //LOAD DATA IN GLOBAL VARIABLE
                nodesQty = System.IO.File.ReadAllLines(path).Length;
            }//END IF

            int[,] graph = new int[nodesQty, nodesQty];

            //INITIALIZE IN ZERO
            for (int i = 0; i < nodesQty; i++) {
                for(int j = 0; j < nodesQty; j++)
                {
                    graph[i, j] = 0;
                }//END FOR X1
            }//END FOR X2

            //LOAD GRAPH FROM ROUTES
            path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Route_File;

            string[] lines = null;

            if (!File.Exists(path))
            {
            }
            else
            {
                //LOAD DATA IN GLOBAL VARIABLE
                lines = System.IO.File.ReadAllLines(path);
            }//END IF

            if (lines != null)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] splited = lines[i].Split(';');
                    int x = Convert.ToInt16( splited[1] ) -1;
                    int y = Convert.ToInt16(splited[3]) - 1;
                    int d = Convert.ToInt16( splited[5] );

                    graph[x, y] = d;
                    graph[y, x] = d;

                }//END FOR
            }

            return graph;
        }//END FUNCTION

        public static List<int> DijkstraAlgorithm(int[,] graph, int sourceNode, int destinationNode)
        {
            var n = graph.GetLength(0);

            var distance = new int[n];
            for (int i = 0; i < n; i++)
            {
                distance[i] = int.MaxValue;
            }

            distance[sourceNode] = 0;

            var used = new bool[n];
            var previous = new int?[n];

            while (true)
            {
                var minDistance = int.MaxValue;
                var minNode = 0;
                for (int i = 0; i < n; i++)
                {
                    if (!used[i] && minDistance > distance[i])
                    {
                        minDistance = distance[i];
                        minNode = i;
                    }
                }

                if (minDistance == int.MaxValue)
                {
                    break;
                }

                used[minNode] = true;

                for (int i = 0; i < n; i++)
                {
                    if (graph[minNode, i] > 0)
                    {
                        var shortestToMinNode = distance[minNode];
                        var distanceToNextNode = graph[minNode, i];

                        var totalDistance = shortestToMinNode + distanceToNextNode;

                        if (totalDistance < distance[i])
                        {
                            distance[i] = totalDistance;
                            previous[i] = minNode;
                        }
                    }
                }
            }

            if (distance[destinationNode] == int.MaxValue)
            {
                return null;
            }

            var path = new LinkedList<int>();
            int? currentNode = destinationNode;
            while (currentNode != null)
            {
                path.AddFirst(currentNode.Value);
                currentNode = previous[currentNode.Value];
            }

            return path.ToList();
        }

        public static RouteSuggested GetPath(int[,] graph, int sourceNode, int destinationNode)
        {
            List<Route> routes = new List<Route>();
            double distance = 0;

            string res = string.Empty;

            var path = DijkstraAlgorithm(graph, sourceNode, destinationNode);

            if (path == null)
            {
                //MessageBox.Show("no path");
                res = "no path";
            }
            else
            {
                int pathLength = 0;
                for (int i = 0; i < path.Count - 1; i++)
                {
                    pathLength += graph[path[i], path[i + 1]];
                }

                var formattedPath = string.Join("->", path);
                res = $"{formattedPath} (length {pathLength})";
                //MessageBox.Show($"{formattedPath} (length {pathLength})");


                //ADD PATH TO LIST
                for(int i = 0; i < path.Count - 1; i++)
                {
                    //var myItem = GlobalVariables.RoutesList.Find(item => item.Id == path[i] + 1 );
                    //routes.Add(myItem);

                    routes.Add(new Route()
                    {
                        Id = i + 1,
                        From = GlobalVariables.PlacesList.Find(item => item.Id == path[i] + 1).Name,
                        To = GlobalVariables.PlacesList.Find(item => item.Id == path[i + 1] + 1).Name,
                        DistanceKm = graph[path[i], path[i + 1]]
                    });

                    distance = pathLength;
                }//END FOR

                
            }

            return new RouteSuggested() { RouteList = routes, Distance = distance };
        }

        int routeStart = 0;
        int routeEnd = 0;

        private void btnCalculateRoute_Click(object sender, EventArgs e)
        {
            
            RouteSuggested suggestedRoute = GetPath(RouteGraph, routeStart, routeEnd);
            dgvSuggestedRoute.Rows.Clear();

            //FILL TABLE WITH ROUTES
            for (int i = 0; i < suggestedRoute.RouteList.Count; i++) {
                dgvSuggestedRoute.Rows.Add( i + 1, suggestedRoute.RouteList[i].From, suggestedRoute.RouteList[i].To, suggestedRoute.RouteList[i].DistanceKm, "--");
            }//END FOR

            lblDistance.Text = suggestedRoute.Distance.ToString();
            
            //SHOW TOTAL
        }

        //MAIN LOAD
        private void LoadPlacesData(bool loadForce = false)
        {
            if (GlobalVariables.PlacesList.Count == 0 || loadForce)
            {
                string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Place_File;

                string[] lines = null;

                if (!File.Exists(path))
                { 
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
            //DataTable dtPlaces = new DataTable();
            //dtPlaces.Columns.Add("Código");
            //dtPlaces.Columns.Add("Nombre");
            //dtPlaces.Columns.Add("Latitud");
            //dtPlaces.Columns.Add("Longitud");
            //dtPlaces.Columns.Add("Activo");

            dgvPlaces.Rows.Clear();

            int count = 1;
            foreach (Place place in GlobalVariables.PlacesList)
            {
                //dtPlaces.Rows.Add(place.Id, place.Name, place.Lat, place.Lng, place.Active);    

                if (place.Active == 1)
                {
                    dgvPlaces.Rows.Add(count, place.Name, false, false, place.Id);
                    dgvPlaces.Rows[dgvPlaces.Rows.Count - 1].ReadOnly = true;
                }
                else {
                    DataGridViewRow row = (DataGridViewRow)dgvPlaces.Rows[0].Clone();
                    //DataGridViewRow row = new DataGridViewRow();
                    row.Cells[0].Value = "-";
                    row.Cells[1].Value = place.Name;
                    row.Cells[2].Value = false;
                    row.Cells[3].Value = false;
                    row.Cells[3].ReadOnly = true;
                    row.Cells[4].Value = 5;
                    
                    row.DefaultCellStyle.BackColor = Color.PaleVioletRed;
                    
                    dgvPlaces.Rows.Add(row);
                }
                count++;
            }//END FOREACH

            //dgvPlaces.DataSource = dtPlaces;
            

        }//END FUNCTION

        private void LoadRoutesData(bool loadForce = false)
        {
            if (GlobalVariables.RoutesList.Count == 0 || loadForce )
            {
                string path = Directory.GetCurrentDirectory() + GlobalVariables.DB_Route_File;

                string[] lines = null;

                if (!File.Exists(path))
                {
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
                            From_Id = Convert.ToInt16( splited[1].ToString() ),
                            From = splited[2].ToString(),
                            To_Id = Convert.ToInt16(splited[3]),
                            To = splited[4].ToString(),
                            DistanceKm = Convert.ToInt16(splited[5]),
                            Active = Convert.ToInt16(splited[6])
                        });

                    }//END FOR
                }
            }//END IF

            dgvRoutes.Rows.Clear();

            int count = 1;
            foreach (Route route in GlobalVariables.RoutesList)
            {
                //dtPlaces.Rows.Add(place.Id, place.Name, place.Lat, place.Lng, place.Active);    

                if (route.Active == 1)
                {
                    dgvRoutes.Rows.Add(count, route.From, route.To, route.DistanceKm, route.Id);
                }
                else
                {
                    DataGridViewRow row = (DataGridViewRow)dgvRoutes.Rows[0].Clone();
                    //DataGridViewRow row = new DataGridViewRow();
                    row.Cells[0].Value = "-";
                    row.Cells[1].Value = route.From;
                    row.Cells[2].Value = route.To;
                    row.Cells[3].Value = route.DistanceKm;
                    row.Cells[4].Value = route.Id;

                    row.DefaultCellStyle.BackColor = Color.PaleVioletRed;

                    dgvRoutes.Rows.Add(row);
                }
                count++;
            }//END FOREACH


        }//END FUNCTION

        private void dgvPlaces_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = dgvPlaces.CurrentRow.Index;
            string id = dgvPlaces.Rows[rowindex].Cells["ColumnSelect"].Value.ToString();
            //Check to ensure that the row CheckBox is clicked.
            if (e.ColumnIndex == 2)
            {
                routeStart = Convert.ToInt16(dgvPlaces.Rows[rowindex].Cells["ColumnId"].Value) - 1;
                //Loop and uncheck all other CheckBoxes.
                foreach (DataGridViewRow row in dgvPlaces.Rows)
                {
                    if(id == "-")
                    {
                        row.Cells["ColumnStart"].Value = false;
                    }
                    else if (row.Index == e.RowIndex)
                    {
                        row.Cells["ColumnStart"].Value = !Convert.ToBoolean(row.Cells["ColumnStart"].EditedFormattedValue);
                    }
                    else
                    {
                        row.Cells["ColumnStart"].Value = false;
                    }
                }
            }else if (e.ColumnIndex == 3)
            {
                routeEnd = Convert.ToInt16(dgvPlaces.Rows[rowindex].Cells["ColumnId"].Value) - 1;
                //Loop and uncheck all other CheckBoxes.
                foreach (DataGridViewRow row in dgvPlaces.Rows)
                {
                    if (id == "-")
                    {
                        row.Cells["ColumnEnd"].Value = false;
                    }
                    else if (row.Index == e.RowIndex)
                    {
                        row.Cells["ColumnEnd"].Value = !Convert.ToBoolean(row.Cells["ColumnEnd"].EditedFormattedValue);
                    }
                    else
                    {
                        row.Cells["ColumnEnd"].Value = false;
                    }
                }
            }
        }

    }
}

