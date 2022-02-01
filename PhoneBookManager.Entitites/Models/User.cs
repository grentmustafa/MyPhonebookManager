using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyPhoneBookManager.Entitites.Models
{
    public class User
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set;}
        [JsonIgnore]
        public IEnumerable<PhoneNumberRecord> PhoneNumbers { get; set; }
    }
}
