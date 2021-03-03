using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            if (NamesAreBlank(firname,surname) || CheckEmail(email)) {
                return false;
            }
            var now = Now();
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day) age--;

            if (age < 21)
            {
                return false;
            }
            //p2
            var client = GetClientById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firname,
                Surname = surname
            };

            if (client.Name == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Name == "ImportantClient")
            {
                user.HasCreditLimit = true;

                var creditLimit = GetCreditLimit(user);

                user.CreditLimit = creditLimit * 2;
            }
            else
            {
                user.HasCreditLimit = true;

                var creditLimit = GetCreditLimit(user);

                user.CreditLimit = creditLimit;
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }
            //p3
            AddUser(user);

            return true;
        }

        protected virtual decimal GetCreditLimit(User user)
        {
            decimal creditLimit;
            using (var userCreditService = new UserCreditServiceClient())
            {
                creditLimit =
                    userCreditService.GetCreditLimit(user.FirstName, user.Surname, user.DateOfBirth);
            }

            return creditLimit;
        }

        protected virtual DateTime Now() => DateTime.Now;

        protected virtual Client GetClientById(int clientId)
        {
            var clientRepository = new ClientRepository();
            return clientRepository.GetById(clientId);
        }
        private bool NamesAreBlank(string firname, string surname)
        {
            return string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname);
        }
        private bool CheckEmail(string email)
        {
            return !email.Contains("@") && !email.Contains(".");
        }
        protected virtual void AddUser(User user) => UserDataAccess.AddUser(user);
    }
}