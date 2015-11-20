using Newtonsoft.Json;
using SQLite;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Model
{
    [Table("Notifications")]
    public class Notification
    {
        [Column("Id")]
        [PrimaryKey, AutoIncrement]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [Column("Type")]
        [JsonProperty("type")]
        public int Type { get; set; }

        [Column("Content")]
        [JsonProperty("content")]
        public string Content { get; set; }

        [Column("Date")]
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [Column("Read")]
        [JsonProperty("read")]
        public bool Read { get; set; }

        [JsonProperty("contact")]
        [OneToOne]
        public Contact Contact { get; set; }

        [ForeignKey(typeof(Contact))]
        public string ContacttId { get; set; }

        public string TypeToString()
        {
         	String text = "";
         	
             switch (Type) {
                 case 1:
                     text = "vamo";
                     break;
                 case 2:
                     text = "perai";
                     break;
                 case 3:
                     text = "chegou";
                     break;
                 case 4:
                     text = "maneiro";
                     break;
                 case 5:
                     text = "tocomfome";
                     break;
                 case 6:
                     text = "owkey";
                     break;
                 default:
                     break;
             }
             
             return text;
        }
    }
}
