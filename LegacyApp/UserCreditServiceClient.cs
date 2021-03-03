using System;

namespace LegacyApp
{
    /// <summary>
    /// In the original example, this was a WCF service
    /// </summary>
    public interface IUserCreditService
    {
        decimal GetCreditLimit(string firstName, string surname, DateTime dateOfBirth);
    }

    /// <summary>
    /// In the original example, this was a WCF service
    /// </summary>
    public class UserCreditServiceClient : IUserCreditService, IDisposable
    {
        public void Dispose()
        {

        }

        public decimal GetCreditLimit(string firstName, string surname, DateTime dateOfBirth)
        {
            throw new Exception("Method cannot be invoked directly");
        }
    }
}