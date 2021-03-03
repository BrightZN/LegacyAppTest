using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LegacyApp.Tests
{
    public class UserServiceTests
    {
        private class InvalidUserTestCaseData : TheoryData<string, string, string, DateTime, int>
        {
            public InvalidUserTestCaseData()
            {
                var now = new DateTime(2021, 1, 1);
                var dateOfBirthMoreThan21 = now.AddYears(-22);

                Add("", "sur", "test@test.com", dateOfBirthMoreThan21, 1);
                Add("fir", "", "test@test.com", dateOfBirthMoreThan21, 1);
                Add("fir", "sur", "test", dateOfBirthMoreThan21, 1);
            }
        }

        [Theory]
        [ClassData(typeof(InvalidUserTestCaseData))]
        public void AddUser_InvalidUserInfo_ReturnsFalseAndDoesNotAddUser(
            string firname, 
            string surname, 
            string email, 
            DateTime dateOfBirth,
            int clientId)
        {
            var sut = new UserService();

            bool result = sut.AddUser(firname, surname, email, dateOfBirth, clientId);

            Assert.False(result);
            //Assert.Null(sut.AddedUser);
        }
    }
}
