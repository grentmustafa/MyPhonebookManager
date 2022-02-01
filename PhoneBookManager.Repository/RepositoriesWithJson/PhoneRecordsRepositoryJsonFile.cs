using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyPhoneBookManager.Entitites.Models;
using MyPhoneBookManager.Repository.Interfaces;


namespace MyPhoneBookManager.Repository.RepositoriesWithJson
{
    public class PhoneRecordsRepositoryJsonFile : IPhoneRecordsRepository
    {
        private readonly string phoneRecordsPath;
        public PhoneRecordsRepositoryJsonFile()
        {
            phoneRecordsPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\filesToRead\\phoneNumberRecords.json";
        }
        public PhoneNumberRecord GetById(long id)
        {
            var phoneNumberRecords = JsonHelper.ReadAndDeserializeFromFile<PhoneNumberRecord>(phoneRecordsPath);
            return phoneNumberRecords != null ? phoneNumberRecords.FirstOrDefault(x => x.ID == id) : null;
        }

        public IEnumerable<PhoneNumberRecord> GetNumbersByUserId(long userId)
        {
            var phoneNumberRecords = JsonHelper.ReadAndDeserializeFromFile<PhoneNumberRecord>(phoneRecordsPath);
            return phoneNumberRecords != null ? phoneNumberRecords.Where(x => x.UserID == userId) : null;
        }

        public bool AddPhoneNumberRecord(PhoneNumberRecord phoneNumberRecordToAdd)
        {
            bool isInserted = false;
            try
            {
                var responseFromFile = JsonHelper.ReadAndDeserializeFromFile<PhoneNumberRecord>(phoneRecordsPath);
                var phoneRecords = responseFromFile == null ? new List<PhoneNumberRecord>() : responseFromFile.ToList();
                phoneNumberRecordToAdd.ID = phoneRecords.Count() + 1;
                phoneRecords.Add(phoneNumberRecordToAdd);
                JsonHelper.WriteJsonToText<PhoneNumberRecord>(phoneRecordsPath, phoneRecords);
                isInserted = true;
            }
            catch
            {
                isInserted = false;
            }

            return isInserted;
        }

        public PhoneNumberRecord UpdatePhoneNumberRecord(long id, int phoneType, string phoneNumber)
        {
            PhoneNumberRecord phoneNumberRecordToUpdate = new PhoneNumberRecord();
            try
            {
                var responseFromFile = JsonHelper.ReadAndDeserializeFromFile<PhoneNumberRecord>(phoneRecordsPath);
                if (responseFromFile == null)
                {
                    return phoneNumberRecordToUpdate;
                }
                var phoneNumberRecords = responseFromFile.ToList();
                phoneNumberRecordToUpdate = phoneNumberRecords.FirstOrDefault(x => x.ID == id);
                if (phoneNumberRecordToUpdate == null)
                {
                    phoneNumberRecordToUpdate = new PhoneNumberRecord();
                    return phoneNumberRecordToUpdate;
                }
                phoneNumberRecordToUpdate.PhoneType = (PhoneType)phoneType;
                phoneNumberRecordToUpdate.PhoneNumber = phoneNumber;
                JsonHelper.WriteJsonToText<PhoneNumberRecord>(phoneRecordsPath, phoneNumberRecords);

            }
            catch
            {
                phoneNumberRecordToUpdate = null;
            }
            return phoneNumberRecordToUpdate;
        }

        public PhoneNumberRecord DeleteNumberRecord(long id)
        {
            PhoneNumberRecord phoneNumberRecordToDelete = new PhoneNumberRecord();
            try
            {
                var responseFromFile = JsonHelper.ReadAndDeserializeFromFile<PhoneNumberRecord>(phoneRecordsPath);
                if (responseFromFile == null)
                {
                    return phoneNumberRecordToDelete;
                }
                var phoneNumberRecords = responseFromFile.ToList();
                 phoneNumberRecordToDelete = phoneNumberRecords.FirstOrDefault(x => x.ID == id);
                if (phoneNumberRecordToDelete == null)
                {
                    phoneNumberRecordToDelete=new PhoneNumberRecord();
                    return phoneNumberRecordToDelete;
                }

                phoneNumberRecords.Remove(phoneNumberRecordToDelete);
                JsonHelper.WriteJsonToText<PhoneNumberRecord>(phoneRecordsPath, phoneNumberRecords);
            }
            catch
            {
                phoneNumberRecordToDelete = null;
            }
            return phoneNumberRecordToDelete;
        }


        public IEnumerable<PhoneNumberRecord> DeleteAllNumbersRecordByUserId(long userId)
        {
            List<PhoneNumberRecord> phoneNumberRecordsToRemove = new List<PhoneNumberRecord>();
            try
            {
                var responseFromFile = JsonHelper.ReadAndDeserializeFromFile<PhoneNumberRecord>(phoneRecordsPath);
                if (responseFromFile == null)
                {
                    return phoneNumberRecordsToRemove;
                }
                var phoneNumberRecords = responseFromFile.ToList();
                phoneNumberRecordsToRemove = phoneNumberRecords.Where(x => x.UserID == userId).ToList();
                if (!phoneNumberRecordsToRemove.Any())
                {
                    return phoneNumberRecordsToRemove;
                }
                phoneNumberRecords = phoneNumberRecords.Except(phoneNumberRecordsToRemove).ToList();
                JsonHelper.WriteJsonToText<PhoneNumberRecord>(phoneRecordsPath, phoneNumberRecords);

            }
            catch
            {
                phoneNumberRecordsToRemove = new List<PhoneNumberRecord>();
            }
            return phoneNumberRecordsToRemove;
        }
    }
}
