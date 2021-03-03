using System;

namespace LegacyApp
{
    public class ClientRepository : IClientRepository
    {
        public Client GetById(int clientId)
        {
            throw new Exception("Method cannot be invoked directly");
        }
    }
}