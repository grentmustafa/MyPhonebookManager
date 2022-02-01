using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhoneBookManager.Entitites.Models
{
    public class PhoneNumberRecord
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public PhoneType PhoneType { get; set; }
        public string PhoneNumber { get; set; }
    }
}
