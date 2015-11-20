using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Model
{
    public class Registration
    {
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
