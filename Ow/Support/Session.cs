using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ow.Model;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketRails;

namespace Ow.Support
{
    public class Session
    {
        private IsolatedStorageSettings preferences;

        public User User { get; set; }
        public bool Mute { get; set; }

        public Session()
        {
            preferences = IsolatedStorageSettings.ApplicationSettings;
        }

        public void LoadData()
        {
            try 
            { 
                string data = (String) preferences["user"];
                User = JsonConvert.DeserializeObject<User>(data);
            }
            catch(Exception e)
            {
                User = null;
            }
        }

        public void SaveData()
        {
            try
            {
                string data = JsonConvert.SerializeObject(User);
                preferences["user"] = data;
                preferences.Save();
            }
            catch (Exception e)
            {

            }
        }
    }
}
