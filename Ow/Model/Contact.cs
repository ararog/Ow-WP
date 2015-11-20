using Newtonsoft.Json;
using SQLite;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Model
{
    [Table("Contacts")]
    public class Contact
    {
        [Column("Id")]
        [PrimaryKey]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Column("FirstName")]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [Column("LastName")]
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [Column("CountryCode")]
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [Column("PhoneNumber")]
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
