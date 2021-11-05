using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Inguat
{
    /// <summary>
    /// CLASS QUE CONTIENE TODAS LAS VARIABLES
    /// </summary>
    public class GlobalVariables
    {
        public static List<User> UsersList = new List<User>();
        public static List<Place> PlacesList = new List<Place>();
        public static List<Route> RoutesList = new List<Route>();
        public static List<Stadistics> StadisticsList = new List<Stadistics>();
        public static int Visitors = 0;

        //FILE NAMES
        public static string DB_User_File = "/DB_User.txt";
        public static string DB_Place_File = "/DB_Place.txt";
        public static string DB_Route_File = "/DB_Route.txt";
        public static string DB_Stadistics_File = "/DB_Stadistics.txt";
    }//END CLASS

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Type { get; set; }
        public int Active { get; set; }
    }//END CLASS

    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int Active { get; set; }
    }//END CLASS

    public class Route
    {
        public int Id { get; set; }
        public int From_Id { get; set; } //Id from Place Class
        public string From { get; set; } //Id from Place Class
        public int To_Id { get; set; } // Id from Place Class
        public string To { get; set; } // Id from Place Class
        public double DistanceKm { get; set; }
        public int Active { get; set; }
    }//END CLASS

    public class RouteSuggested
    {
        public List<Route> RouteList { get; set; }
        public double Distance { get; set; }
    }//END CLASS

    public class Stadistics
    {
        public int Type { get; set; }
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
    }
}
