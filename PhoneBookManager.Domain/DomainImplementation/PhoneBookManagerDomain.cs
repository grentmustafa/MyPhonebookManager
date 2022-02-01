using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MyPhoneBookManager.Domain.Interfaces;
using MyPhoneBookManager.DTO;
using MyPhoneBookManager.Entitites.Models;
using MyPhoneBookManager.Repository.Interfaces;

namespace PhoneBookManager.Domain.DomainImplementation
{
    public class PhoneBookManagerDomain : IPhoneBookManagerDomain
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPhoneRecordsRepository _phoneRecordsRepository;

        public PhoneBookManagerDomain(IMapper mapper, IUserRepository userRepository, IPhoneRecordsRepository phoneRecordsRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _phoneRecordsRepository = phoneRecordsRepository;
        }


        public UsersOutDTO GetPhoneBookRecordByUserId(long id)
        {
            var user = this._userRepository.GetById(id);
            if (user == null)
            {
                return new UsersOutDTO();
            }
            var phoneNumber = this._phoneRecordsRepository.GetNumbersByUserId(id);
            user.PhoneNumbers = phoneNumber;
            var response = this._mapper.Map<User, UsersOutDTO>(user);
            return response;

        }

        public IEnumerable<UsersOutDTO> GetAllPhoneBookRecords()
        {
            var userFromRepository = this._userRepository.GetAllUsers()?.ToList();
            if (userFromRepository == null)
            {
                return new List<UsersOutDTO>();
            }
            userFromRepository.ForEach(x => x.PhoneNumbers = this._phoneRecordsRepository.GetNumbersByUserId(x.ID));
            var response = this._mapper.Map<IEnumerable<User>, IEnumerable<UsersOutDTO>>(userFromRepository);
            return response;
        }

        public IEnumerable<UsersOutDTO> GetAllPhoneBookRecordsSorted(bool asc, bool sortByFirstName)
        {
            var users = this.GetAllPhoneBookRecords().ToList();
            var response = asc
                ? users.OrderBy(a => sortByFirstName ? a.FirstName : a.LastName)
                : users.OrderByDescending(a => sortByFirstName ? a.FirstName : a.LastName);
            return response;
        }

        public UsersOutDTO AddPhoneBookRecord(UsersInDTO userToAdd)
        {
            User user = this._userRepository.GetByFullName(userToAdd.FirstName + " " + userToAdd.LastName);
            if (user == null)
            {
                user = _userRepository.AddUser(this._mapper.Map<UsersInDTO, User>(userToAdd));
                if (user != null && user.ID > 0)
                {
                    InsertPhoneNumbers(userToAdd, user);
                }
            }
            else
            {
                InsertPhoneNumbers(userToAdd, user);
            }
            return this._mapper.Map<User, UsersOutDTO>(user);
        }

        private void InsertPhoneNumbers(UsersInDTO userToAdd, User user)
        {

            foreach (var phoneNumber in userToAdd.PhoneNumbers)
            {
                var phoneNumberToAdd = this._mapper.Map<PhoneNumberInDTO, PhoneNumberRecord>(phoneNumber);
                phoneNumberToAdd.UserID = user.ID;
                this._phoneRecordsRepository.AddPhoneNumberRecord(phoneNumberToAdd);
            }

            user.PhoneNumbers = this._phoneRecordsRepository.GetNumbersByUserId(user.ID);
        }

        public UsersOutDTO DeletePhoneBookRecord(long id)
        {

            var phoneNumbersToDelete = this._phoneRecordsRepository.DeleteAllNumbersRecordByUserId(id);
            if (!phoneNumbersToDelete.Any())
            {
                return new UsersOutDTO();
            }
            else
            {
                var user = this._userRepository.DeleteUser(id);
                user.PhoneNumbers = phoneNumbersToDelete;
                return this._mapper.Map<User, UsersOutDTO>(user);
            }
        }

        public PhoneNumberOutDTO DeletePhoneNumberOfUser(long phoneNumberId)
        {
            var phoneNumberDeleted = this._phoneRecordsRepository.DeleteNumberRecord(phoneNumberId);
            return this._mapper.Map<PhoneNumberRecord, PhoneNumberOutDTO>(phoneNumberDeleted);
        }

        public PhoneNumberOutDTO UpdatePhoneNumber(long id, PhoneNumberInDTO phoneNumberToUpdate)
        {
            var phoneNumberRecordUpdated = this._phoneRecordsRepository.UpdatePhoneNumberRecord(id, phoneNumberToUpdate.PhoneType,
                phoneNumberToUpdate.PhoneNumber);
            return this._mapper.Map<PhoneNumberRecord, PhoneNumberOutDTO>(phoneNumberRecordUpdated);
        }

        public UsersOutDTO UpdatePhoneBookRecord(long id, PhoneRecordUpdateDTO recordToUpdate)
        {
            var userUpdated = this._userRepository.UpdateUser(id, recordToUpdate.FirstName, recordToUpdate.LastName);
            if (userUpdated != null && userUpdated.ID > 0)
            {
                foreach (var phoneNumber in recordToUpdate.PhoneNumber)
                {
                    var getPhoneNumberById = this._phoneRecordsRepository.GetById(phoneNumber.ID);
                    if (getPhoneNumberById != null && getPhoneNumberById.UserID == id)
                    {
                        var phoneNumberRecord = this._phoneRecordsRepository.UpdatePhoneNumberRecord(phoneNumber.ID,
                            phoneNumber.PhoneType, phoneNumber.PhoneNumber);
                    }
                }

                userUpdated.PhoneNumbers = this._phoneRecordsRepository.GetNumbersByUserId(id);
                return this._mapper.Map<User, UsersOutDTO>(userUpdated);
            }

            return new UsersOutDTO();

        }
    }
}
