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
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

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

        private void Test_Load(object sender, EventArgs e)
        {
            //var graph = new[,]
            //   {
            //        // 0   1   2   3   4   5   6   7   8   9  10  11
            //        { 0,  0,  0,  0,  0,  0, 10,  0, 12,  0,  0,  0 }, // 0
            //        { 0,  0,  0,  0, 20,  0,  0, 26,  0,  5,  0,  6 }, // 1
            //        { 0,  0,  0,  0,  0,  0,  0, 15, 14,  0,  0,  9 }, // 2
            //        { 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  7,  0 }, // 3
            //        { 0, 20,  0,  0,  0,  5, 17,  0,  0,  0,  0, 11 }, // 4
            //        { 0,  0,  0,  0,  5,  0,  6,  0,  3,  0,  0, 33 }, // 5
            //        {10,  0,  0,  0, 17,  6,  0,  0,  0,  0,  0,  0 }, // 6
            //        { 0, 26, 15,  0,  0,  0,  0,  0,  0,  3,  0, 20 }, // 7
            //        {12,  0, 14,  0,  0,  3,  0,  0,  0,  0,  0,  0 }, // 8
            //        { 0,  5,  0,  0,  0,  0,  0,  3,  0,  0,  0,  0 }, // 9
            //        { 0,  0,  0,  7,  0,  0,  0,  0,  0,  0,  0,  0 }, // 10
            //        { 0,  6,  9,  0, 11, 33,  0, 20,  0,  0,  0,  0 }, // 11
            //    };

            var graph = new[,]
               {
                    // 0   1   2   3   4   5   6   7   8   9  10  11
                    { 0,  328,  185,  0 }, // 0
                    { 328,  0,  0,  265 }, // 1
                    { 185,  0,  0,  82 }, // 2
                    { 0,  265,  82,  0 }, // 3
                };

            PrintPath(graph, 0, 3);
            //PrintPath(graph, 0, 9);
            //PrintPath(graph, 0, 2);
            //PrintPath(graph, 0, 10);
            //PrintPath(graph, 0, 11);
            //PrintPath(graph, 0, 1);
        }

        public static void PrintPath(int[,] graph, int sourceNode, int destinationNode)
        {
            Console.Write(
                "Shortest path [{0} -> {1}]: ",
                sourceNode,
                destinationNode);

            var path = DijkstraAlgorithm(graph, sourceNode, destinationNode);

            if (path == null)
            {
                Console.WriteLine("no path");
                MessageBox.Show("no path");
            }
            else
            {
                int pathLength = 0;
                for (int i = 0; i < path.Count - 1; i++)
                {
                    pathLength += graph[path[i], path[i + 1]];
                }

                var formattedPath = string.Join("->", path);
                Console.WriteLine("{0} (length {1})", formattedPath, pathLength);
                MessageBox.Show($"{formattedPath} (length {pathLength})");
            }
        }
    }
}
