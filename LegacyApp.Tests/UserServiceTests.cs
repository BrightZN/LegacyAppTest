using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LegacyApp.Tests
{
    public class UserServiceTests
    {
        private readonly TestableClientRepository _clientRepository = new TestableClientRepository();

        private readonly TestableUserService _sut;

        public UserServiceTests()
        {
            _sut = new TestableUserService(
                _clientRepository);
        }

        private class InvalidUserTestCaseData 
            : TheoryData<string, string, string, DateTime, int, DateTime, Client, decimal, bool>
        {
            public InvalidUserTestCaseData()
            {
                var now = new DateTime(2021, 1, 1);
                var dateOfBirthMoreThan21 = now.AddYears(-22);
                var dateOfBirthLessThan21 = now.AddYears(-15);

                var client = new Client
                {
                    Id = 1,
                    Name = "Boring Client"
                };

                Add("", "sur", "test@test.com", dateOfBirthMoreThan21, 1, now, client,0.00M,false);
                Add("fir", "", "test@test.com", dateOfBirthMoreThan21, 1, now, client, 0.00M,false);
                Add("fir", "sur", "test", dateOfBirthMoreThan21, 1, now, client, 0.00M, false);
                Add("fir", "sur", "test@test.com", dateOfBirthLessThan21, 1, now, client, 0.00M, false);
                Add("fir", "sur", "test@test.com", dateOfBirthMoreThan21, 1, now, client, 0.00M, false);
                var client2 = new Client
                {
                    Id = 2,
                    Name = "ImportantClient"
                };
                Add("fir", "sur", "test@test.com", dateOfBirthMoreThan21, 2, now, client2, 0.00M, false);

                Add("fir", "sur", "test@test.com", dateOfBirthMoreThan21, 1, now, client, 501.00M, true);
                Add("fir", "sur", "test@test.com", dateOfBirthMoreThan21, 2, now, client2, 251.00M, true);
                var client3 = new Client
                {
                    Id = 3,
                    Name = "VeryImportantClient"
                };
                Add("fir", "sur", "test@test.com", dateOfBirthMoreThan21, 3, now, client3, 0.00M, true);
            }
        }

        [Theory]
        [ClassData(typeof(InvalidUserTestCaseData))]
        public void AddUser_WithProvidedUserInfo_ReturnsExpectedResultAndAddedUserCheckPasses(
            string firname, 
            string surname, 
            string email, 
            DateTime dateOfBirth,
            int clientId,
            DateTime now,
            Client client,
            Decimal creditLimit,
            bool expected)
        {
            _sut.NowValue = now;

            _clientRepository.Client = client;

            _sut.CreditLimit = creditLimit;
            bool result = _sut.AddUser(firname, surname, email, dateOfBirth, clientId);

            Assert.Equal(expected,result);
            Assert.Equal(expected,_sut.User != null);
        }

        private class TestableClientRepository : IClientRepository
        {
            public Client Client { get; set; }

            public Client GetById(int clientId) => Client;
        }

        private class TestableUserService : UserService
        {
            public TestableUserService(IClientRepository clientRepository)
                : base(clientRepository)
            {

            }

            public DateTime NowValue { get; set; }
            public decimal CreditLimit { get; set; }
            public User User { get; set; }

            protected override DateTime Now() => NowValue;

            protected override decimal GetCreditLimit(User user) => CreditLimit;

            protected override void AddUser(User user) => User = user;
        }
    }
}
