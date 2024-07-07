using LOGIC_LAYER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;
using static UI_LAYER.Global_Classes.clsGlobal;

using static UI_LAYER.Global_Classes.clsUtil;

namespace UI_LAYER.Global_Classes
{
    internal class clsGlobal
    {
        public static clsUser CurrentUser { get; set; }

        

        public class UserCredentials
        {
            public  string Username { get; set; }
            public  string Password { get; set; }
        }

        

        

        private static UserCredentials GetCurrentUserCredentials(string text)
        {
            string[] strings = text.Split(new string[] { "//" }, StringSplitOptions.None);
            return new UserCredentials { Username = strings[0], Password = strings[1] };
        }


        public static bool RememberUsernameAndPassword(string username, string password)
        {
            try
            {
                string currentDir = System.IO.Directory.GetCurrentDirectory();

                // Define the path to the text file where you want to save the data
                string filePath = currentDir + "\\data.txt";

                if (File.Exists(filePath) && username == "")
                {
                    File.Delete(filePath);
                    return true;
                }

                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    string dataToSave = username + "//" + password;
                    
                    
                    streamWriter.WriteLine(dataToSave);


                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static UserCredentials GetStoredCredential()
        {
            try
            {
                string currentDir = System.IO.Directory.GetCurrentDirectory();

                // Define the path to the text file where you want to save the data
                string filePath = currentDir + "\\data.txt";

                if (File.Exists(filePath))
                {
                    StreamReader streamReader = new StreamReader(filePath);

                    using (StreamReader reader = new StreamReader(filePath))
                    {

                        UserCredentials userCredentials = GetCurrentUserCredentials(reader.ReadLine());
                        
                        return userCredentials;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }

        }
    }
}
