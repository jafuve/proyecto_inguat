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

        public static string GetPath(int[,] graph, int sourceNode, int destinationNode)
        {
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
            }

            return res;
        }

        private void btnCalculateRoute_Click(object sender, EventArgs e)
        {
            string MinPath = GetPath(RouteGraph, 0, 3);
            MessageBox.Show("Min " + MinPath);
        }
    }
}
