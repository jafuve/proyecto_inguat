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

        private void Main_Load(object sender, EventArgs e)
        {
            // VERIFY DATA FILES EXISTANCE
            VerifyDataFilesExistance();

            // LOAD PLACES IN MAIN

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
    }
}
