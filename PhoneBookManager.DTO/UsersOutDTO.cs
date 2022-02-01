using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhoneBookManager.DTO
{
    public class UsersOutDTO
    {
        public long ID { get; set; }
        public string FirstName { get; set; }    
        public  string LastName { get; set; }
        public IEnumerable<PhoneNumberOutDTO> PhoneNumber { get; set; }
    }
}
