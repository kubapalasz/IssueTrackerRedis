using System;
using FluentAssertions;
using NUnit.Framework;

namespace IssueTracker.Tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void CreateUser_ShouldStorePasswordInMD5()
        {
            var board = new IssueBoard();
            var user = board.CreateUser("username", "password");

            user.Username.Should().Be("username");
            user.PasswordHash.Should().Be("5f4dcc3b5aa765d61d8327deb882cf99");
        }

        [Test]
        public void CreateUser_UsernameExists_ShouldThrow()
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");

            Action act = () => board.CreateUser("username", "password1");

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("username");
        }

        [Test]
        public void CreateUsersWithDifferentPasswords_ShouldHaveDifferentHashes()
        {
            var board = new IssueBoard();
            var user1 = board.CreateUser("username1", "password1");
            var user2 = board.CreateUser("username2", "password2");

            user1.PasswordHash.Should().NotBe(user2.PasswordHash);
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("\r\n")]
        [TestCase("T EST")]
        [TestCase(null)]
        public void CreateUser_UsernameInvalid_ShouldThrow(string username)
        {
            var board = new IssueBoard();

            Action act = () => board.CreateUser(username, "password1");

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("username");
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        [TestCase("11111")]
        public void CreateUser_PasswordIsTooSimple_ShouldThrow(string password)
        {
            var board = new IssueBoard();

            Action act = () => board.CreateUser("username", password);

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("password");
        }

        [Test]
        public void LoginUser_ShouldSetCurrentUserOnBoard()
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");

            board.CurrentUser.Username.Should().Be("username");
        }

        [Test]
        public void LogoutUser_ShouldResetCurrentUserToNull()
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");
            board.Logout();

            board.CurrentUser.Should().BeNull();
        }

        [Test]
        public void LoginUser_WithWrongPassword_ShouldThrow()
        {
            var board = new IssueBoard();
            board.CreateUser("username1", "password1");
            board.CreateUser("username2", "password2");

            Action act = () => board.Login("username1", "wrong_password");

            act.ShouldThrow<InvalidOperationException>()
                .WithMessage("Unknown user or wrong password");
        }

        [Test]
        public void LoginUser_UserDoesNotExist_ShouldThrow()
        {
            var board = new IssueBoard();
            board.CreateUser("username1", "password1");
            board.CreateUser("username2", "password2");

            Action act = () => board.Login("wrong_user", "password1");

            act.ShouldThrow<InvalidOperationException>()
                .WithMessage("Unknown user or wrong password");
        }

        [Test]
        public void LogoutUser_NotLoggedIn_ShouldThrow()
        {
            var board = new IssueBoard();

            Action act = () => board.Logout();

            act.ShouldThrow<InvalidOperationException>()
                .WithMessage("No one is logged in yet!");
        }
    }
}