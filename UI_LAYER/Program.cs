using LOGIC_LAYER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_LAYER.Applications.Applications_Types;
using UI_LAYER.Applications.International_Driving_License;
using UI_LAYER.Applications.Local_Driving_License;
using UI_LAYER.Applications.Release_Detained_License;
using UI_LAYER.Drivers;
using UI_LAYER.Licenses.Local_Licenses;
using UI_LAYER.Login;
using UI_LAYER.Pepole;
using UI_LAYER.Test;
using UI_LAYER.Users;

namespace UI_LAYER
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
    }
}
