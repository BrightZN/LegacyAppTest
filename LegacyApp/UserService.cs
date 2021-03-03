using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            if (string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname))
            {
                return false;
            }

            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }
            //p1
            var now = Now();
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day) age--;

            if (age < 21)
            {
                return false;
            }
            //p2
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

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

                using (var userCreditService = new UserCreditServiceClient())
                {
                    var creditLimit =
                        userCreditService.GetCreditLimit(user.FirstName, user.Surname, user.DateOfBirth);

                    user.CreditLimit = creditLimit * 2;
                }
            }
            else
            {
                user.HasCreditLimit = true;

                using (var userCreditService = new UserCreditServiceClient())
                {
                    var creditLimit =
                        userCreditService.GetCreditLimit(user.FirstName, user.Surname, user.DateOfBirth);

                    user.CreditLimit = creditLimit;
                }
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }
            //p3
            UserDataAccess.AddUser(user);

            return true;
        }

        protected virtual DateTime Now() => DateTime.Now;
    }
}