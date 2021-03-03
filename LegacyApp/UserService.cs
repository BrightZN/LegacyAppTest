using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;

        public UserService()
        {
            _clientRepository = new ClientRepository();
        }

        public UserService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            if (NamesAreBlank(firname, surname) || CheckEmail(email))
                return false;

            if (YoungerThan21(dateOfBirth))
                return false;

            var client = _clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firname,
                Surname = surname
            };

            ApplyCreditLimit(user);

            if (CheckCredit(user))
                return false;

            //_userDataAccess.AddUser(user)
            AddUser(user);

            return true;
        }

        private static bool CheckCredit(User user)
        {
            return user.HasCreditLimit && user.CreditLimit < 500;
        }

        private void ApplyCreditLimit(User user)
        {
            var client = user.Client;

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
        }

        private bool YoungerThan21(DateTime dateOfBirth)
        {
            var now = Now();
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day) age--;

            bool youngerThan21 = age < 21;
            return youngerThan21;
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

        private bool NamesAreBlank(string firname, string surname)
        {
            return string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname);
        }
        private bool CheckEmail(string email)
        {
            return !email.Contains("@") && !email.Contains(".");
        }

        
        protected virtual void AddUser(User user) => UserDataAccess.AddUser(user);// _userDataAccess.AddUser(user)
    }
}