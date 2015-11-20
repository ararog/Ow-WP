using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace Ow.Model
{
    public class Country
    {
        [JsonProperty("code")]
        public String Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dial_code")]
        public string DialCode { get; set; }

        [JsonIgnore]
        public BitmapImage Image
        {
            get
            {
                BitmapImage bitmapImage = new BitmapImage();

                Uri imageUri = new Uri(@"Assets/Flags/flag_" + 
                    Code.ToLower() + ".png", UriKind.Relative);

                StreamResourceInfo streamInfo = Application.GetResourceStream(imageUri);

                if (streamInfo != null)
                {
                    bitmapImage.SetSource(streamInfo.Stream);

                    return bitmapImage;
                }

                return null;
            }
        }
    }
}
