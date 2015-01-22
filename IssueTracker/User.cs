using System;

namespace IssueTracker
{
    public class User
    {
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }

        public User(string username, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", "username");

            if (username.Contains(" "))
                throw new ArgumentException("Username cannot contain spaces", "username");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty", "passwordHash");

            Username = username;
            PasswordHash = passwordHash;
        }
    }
}