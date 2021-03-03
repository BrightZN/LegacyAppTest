using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            if (NamesAreBlank(firname, surname) || CheckEmail(email))
            {
                return false;
            }

            if (YoungerThan21(dateOfBirth))
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
            ApplyCreditLimit(client, user);

            if (CheckCredit(user))
            {
                return false;
            }
            //p3
            AddUser(user);

            return true;
        }

        private static bool CheckCredit(User user)
        {
            return user.HasCreditLimit && user.CreditLimit < 500;
        }

        private void ApplyCreditLimit(Client client, User user)
        {
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