using System;

namespace LegacyApp.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var userService = new UserService();

            var added = userService.AddUser(
                firname: "Ryan",
                surname: "East",
                email: "ryan.east@test.com",
                dateOfBirth: new DateTime(1995, 1, 1),
                1);

            System.Console.WriteLine($"Adding Ryan East was {(added ? "successful" : "unsuccessful")}");
        }
    }
}
