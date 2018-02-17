using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TradingBot.Data.Entities;

namespace TradingBot.Services
{
    public class UserService: BaseService
    {
        private const int PasswordSaltLength = 16;
        private const string PasswordHashFormat = "System.Security.Cryptography.SHA1";
           
        public UserService()
        {
        }

        private string CreateSalt(int size)
        {
            var provider = new RNGCryptoServiceProvider();

            var data = new byte[size];
            provider.GetBytes(data);

            return Convert.ToBase64String(data);
        }

        private string CreatePasswordHash(string password, string salt, string format)
        {
            var passwordBytes = Encoding.Unicode.GetBytes(password);
            var saltBytes = Convert.FromBase64String(salt);
            var totalBytes = new byte[saltBytes.Length + passwordBytes.Length];

            Buffer.BlockCopy(saltBytes, 0, totalBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, totalBytes, saltBytes.Length, passwordBytes.Length);

	        var hashAlgorithm = SHA512.Create("SHA512");
            var hash = hashAlgorithm.ComputeHash(totalBytes);

            return Convert.ToBase64String(hash);
        }


        private string CreatePasswordHashAndSalt(string password, out string passwordSalt)
        {
            passwordSalt = CreateSalt(PasswordSaltLength);
            return CreatePasswordHash(password, passwordSalt, PasswordHashFormat);
        }

        public User GetUser(string username)
        {
            var key = (username ?? "").Trim().ToLowerInvariant();
            var row = Context.Users.Query().FirstOrDefault(m => m.Username.ToLower() == key);
            return row;
        }

        public User RegisterUser(string username, string password)
        {
            var name = (username ?? "").Trim();

            var user = GetUser(name);
            if (user != null)
                return null;

            string passwordSalt;

            var passwordHash = CreatePasswordHashAndSalt(password, out passwordSalt);

            user = new User
            {
                Username = name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            Context.Users.Add(user);
            Context.SaveChanges();

            return user;
        }

        public User AuthenticateUser(string username, string password)
        {
            var user = GetUser(username);

            if (user == null)
                return null;

            var passwordHash = CreatePasswordHash(password, user.PasswordSalt, PasswordHashFormat);

            if (!user.PasswordHash.Equals(passwordHash))
            {
                return null;
            }

            return user;
        }

        //public void CreateOrUpdateUser(string username, string password)
        //{
        //    username = (username ?? "").Trim();

        //    var user = GetUser(username);
        //    var isNew = false;
        //    if (user == null)
        //    {
        //        user = new User();
        //        isNew = true;
        //    }

        //    user.Username = username;
        //    user.PasswordHash = password;

        //    if (isNew)
        //        dbContext.Users.Add(user);

        //    dbContext.SaveChanges();
        //}

    }
}