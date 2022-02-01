using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhoneBookManager.Entitites.Models;


namespace MyPhoneBookManager.Repository.Interfaces
{
    public interface IPhoneRecordsRepository
    {
        PhoneNumberRecord GetById(long id);
        IEnumerable<PhoneNumberRecord> GetNumbersByUserId(long userId);
        bool AddPhoneNumberRecord(PhoneNumberRecord phoneNumberRecordToAdd);
        PhoneNumberRecord UpdatePhoneNumberRecord(long id, int phoneType, string phoneNumber);
        PhoneNumberRecord DeleteNumberRecord(long id);
        IEnumerable<PhoneNumberRecord> DeleteAllNumbersRecordByUserId(long userId);
    }
}
