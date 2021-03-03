using System;

namespace LegacyApp
{
    /// <summary>
    /// This class cannot be modified
    /// </summary>
    public static class UserDataAccess
    {
        public static void AddUser(User _)
        {
            throw new Exception("Method cannot be invoked directly");
        }
    }
}