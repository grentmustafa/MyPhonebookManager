using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhoneBookManager.DTO;

namespace MyPhoneBookManager.Domain.Interfaces
{
    public interface IPhoneBookManagerDomain
    {
        UsersOutDTO GetPhoneBookRecordByUserId(long id);
        IEnumerable<UsersOutDTO> GetAllPhoneBookRecords();
        IEnumerable<UsersOutDTO> GetAllPhoneBookRecordsSorted(bool asc, bool sortByFirstName);
        UsersOutDTO AddPhoneBookRecord(UsersInDTO userToAdd);
        UsersOutDTO DeletePhoneBookRecord(long id);
        PhoneNumberOutDTO DeletePhoneNumberOfUser(long phoneNumberId);
        PhoneNumberOutDTO UpdatePhoneNumber(long id, PhoneNumberInDTO phoneNumberToUpdate);
        UsersOutDTO UpdatePhoneBookRecord(long id, PhoneRecordUpdateDTO recordToUpdate);

    }
}
