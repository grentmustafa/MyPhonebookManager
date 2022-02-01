using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhoneBookManager.DTO
{
    public class UsersInDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<PhoneNumberInDTO> PhoneNumbers { get; set; }
    }
}
