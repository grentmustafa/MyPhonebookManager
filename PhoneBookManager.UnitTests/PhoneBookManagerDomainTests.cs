using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MyPhoneBookManager.Domain.Interfaces;
using MyPhoneBookManager.Domain.MappingProfile;
using MyPhoneBookManager.DTO;
using MyPhoneBookManager.Entitites.Models;
using MyPhoneBookManager.Repository.Interfaces;
using NSubstitute;
using NSubstitute.Extensions;
using NSubstitute.ReturnsExtensions;
using PhoneBookManager.Domain.DomainImplementation;
using Xunit;

namespace PhoneBookManager.UnitTests
{
    public class PhoneBookManagerDomainTests
    {
        private readonly IPhoneBookManagerDomain _domain;
        private readonly IMapper _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfile>(); }).CreateMapper();
        private readonly IPhoneRecordsRepository _phoneRecordsRepository = Substitute.For<IPhoneRecordsRepository>();
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly IFixture _fixture = new Fixture();

        public PhoneBookManagerDomainTests()
        {
            _domain = new PhoneBookManagerDomain(_mapper, _userRepository, _phoneRecordsRepository);
        }
        [Fact]
        public void GetPhoneRecord_ShouldReturnUser_ByID()
        {
            //Arrange
            const int userId = 1;
            _userRepository.GetById(userId).Returns(new User() { ID = 1, FirstName = "Grent", LastName = "Mustafa" });
            _phoneRecordsRepository.GetNumbersByUserId(userId).Returns(new List<PhoneNumberRecord>()
            {
                new PhoneNumberRecord() {ID = 1, UserID = userId, PhoneType = PhoneType.Home, PhoneNumber = "+355695103594"}
            });
            //Act
            var result = _domain.GetPhoneBookRecordByUserId(userId);

            //Assert
            result.Should().NotBeNull();
            result.ID.Should().Be(userId);
            result.PhoneNumber.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetPhoneRecord_ShouldNotFoundUser_ByID()
        {
            //Arrange
            const int userId = 234;
            _userRepository.GetById(userId).Returns(new User());
            _phoneRecordsRepository.GetNumbersByUserId(userId).Returns(new List<PhoneNumberRecord>());
            //Act
            var result = _domain.GetPhoneBookRecordByUserId(userId);

            //Assert
            result.ID.Should().Be(0);
            result.FirstName.Should().BeNull();
            result.LastName.Should().BeNull();
            result.PhoneNumber.Should().BeNullOrEmpty();
        }

        [Fact]
        public void GetPhoneRecord_ShouldFoundUserButNoPhoneNumbers_ByID()
        {
            //Arrange
            const int userId = 1;
            _userRepository.GetById(userId).Returns(new User() { ID = 1, FirstName = "Grent", LastName = "Mustafa" });
            _phoneRecordsRepository.GetNumbersByUserId(userId).Returns(new List<PhoneNumberRecord>());
            //Act
            var result = _domain.GetPhoneBookRecordByUserId(userId);

            //Assert
            result.ID.Should().Be(userId);
            result.FirstName.Should().Be("Grent");
            result.LastName.Should().Be("Mustafa");
            result.PhoneNumber.Should().BeNullOrEmpty();
        }
        [Fact]
        public void GetPhoneRecord_ShouldNotFoundAnyRecords_AllRecords()
        {
            //Arrange
            _userRepository.GetAllUsers().ReturnsNull();
            //Act
            var result = _domain.GetAllPhoneBookRecords();
            //Assert
            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public void GetPhoneRecord_ShouldReturnNonEmpty_AllRecords()
        {
            //Arrange

            _userRepository.GetAllUsers().
                Returns(new List<User>()
                {
                    new User() { ID = 1, FirstName = "Grent", LastName = "Mustafa"} ,
                    new User() { ID = 2, FirstName = "Test", LastName = "Test"} ,
                    new User() { ID = 3, FirstName = "Test2", LastName = "Test2"} ,

                });
            _phoneRecordsRepository.GetNumbersByUserId(1).Returns(new List<PhoneNumberRecord>()
            {
                new PhoneNumberRecord() {ID = 1, UserID = 1, PhoneType = PhoneType.Home, PhoneNumber = "+355695103594"},
                new PhoneNumberRecord() {ID = 2, UserID = 1, PhoneType = PhoneType.Home, PhoneNumber = "+6896454654654"},

            });
            _phoneRecordsRepository.GetNumbersByUserId(2).Returns(new List<PhoneNumberRecord>()
            {
                new PhoneNumberRecord() {ID = 3, UserID = 2, PhoneType = PhoneType.Cellphone, PhoneNumber = "+46546545645"},

            });
            //Act
            var result = _domain.GetAllPhoneBookRecords();

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(3);
            result.First().PhoneNumber.Count().Should().Be(2);
            result.Last().PhoneNumber.Should().BeNullOrEmpty();
        }
        [Fact]
        public void AddPhoneRecord_ShouldAddRecord_NewRecord()
        {
            //Arrange
            const int userId = 1;
            const string firstName = "Grent";
            const string lastname = "Mustafa";
            var userToAdd = _fixture.Build<UsersInDTO>().
                With(c=>c.FirstName,firstName).
                With(c=>c.LastName,lastname).
                With(c=>c.PhoneNumbers, new List<PhoneNumberInDTO>() {new PhoneNumberInDTO()
                {
                    PhoneType = 1,
                    PhoneNumber = "+355695103594"
                }}).Create();

            var user = _fixture.Build<User>()
                .With(c => c.ID, userId)
                .With(c => c.FirstName, firstName)
                .With(c => c.LastName, lastname)
                .Create();


            _userRepository.GetByFullName(userToAdd.FirstName + " " + userToAdd.LastName).ReturnsNull();
            _userRepository.AddUser(Arg.Any<User>()).Returns(user);
            _phoneRecordsRepository.AddPhoneNumberRecord(Arg.Any<PhoneNumberRecord>()).Returns(true);
            _phoneRecordsRepository.GetNumbersByUserId(userId).Returns(new List<PhoneNumberRecord>(){
                new PhoneNumberRecord() {ID = 1, UserID = userId, PhoneType = PhoneType.Work, PhoneNumber = "+355695103594"},

            });
            //Act
            var result = _domain.AddPhoneBookRecord(userToAdd);

            //Assert
            result.Should().NotBeNull();
            result.ID.Should().Be(userId);
            result.FirstName.Should().Be(firstName);
            result.LastName.Should().Be(lastname);
            result.PhoneNumber.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void DeletePhoneRecord_ShoulDeleteRecord_ExistingRecord()
        {
            //Arrange
            const int userId = 1;
            const string firstName = "Grent";
            const string lastname = "Mustafa";
            const string phoneNumber = "Mustafa";

            _phoneRecordsRepository.DeleteAllNumbersRecordByUserId(userId).Returns(new List<PhoneNumberRecord>(){
                new PhoneNumberRecord() {ID = 1, UserID = userId, PhoneType = PhoneType.Work, PhoneNumber = phoneNumber},

            });
            _userRepository.DeleteUser(userId).Returns(new User(){ ID = userId, FirstName = firstName, LastName = lastname });
            //Act
            var result = _domain.DeletePhoneBookRecord(userId);

            //Assert
            result.Should().NotBeNull();
            result.ID.Should().Be(userId);
            result.FirstName.Should().Be(firstName);
            result.LastName.Should().Be(lastname);
            result.PhoneNumber.Should().NotBeNullOrEmpty();
            result.PhoneNumber.First().PhoneNumber.Should().Be(phoneNumber);

        }
    }
}