using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace IssueTracker.Tests
{
    [TestFixture]
    public class ProjectTests
    {
        [Test]
        public void CreateProject_NotLoggedIn_ShouldThrow()
        {
            var board = new IssueBoard();
            
            Action act = () => board.CreateProject("TEST", "Test project");

            act.ShouldThrow<AccessViolationException>()
                .WithMessage("Not logged in");
        }

        [Test]
        public void CreateProject_LoggedIn_ShouldSetCreatedBy()
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");

            board.CreateProject("P-Key", "Test Project");

            var project = board.Projects.First();
            project.CreatedBy.Should().Be("username");
        }

        [Test]
        public void CreateProject_ShouldAddProjectToBoard()
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");
            board.CreateProject("TEST", "Test project");

            var project = board.Projects.Single();

            project.Key.Should().Be("TEST");
            project.Name.Should().Be("Test project");
        }

        [Test]
        public void CreateProject_WithDuplicateKey_ShouldThrowArgumentException()
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");
            board.CreateProject("TEST", "Test project 1");

            Action act = () => board.CreateProject("TEST", "Test project 2");

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("key");
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("\r\n")]
        [TestCase("T EST")]
        [TestCase(null)]
        public void CreateProject_WithInvalidKey_ShouldThrowArgumentException(string key)
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");

            Action act = () => board.CreateProject(key, "Test project");

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("key");
        }

        [Test]
        public void CreateProject_SholdNotAllowKeyLongerThan8Characters(string key)
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");

            Action act = () => board.CreateProject("verylongkeylol", "Test project");

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("key");
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("\r\n")]
        [TestCase(null)]
        public void CreateProject_WithInvalidName_ShouldThrowArgumentException(string name)
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");

            Action act = () => board.CreateProject("TEST", name);

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("name");
        }
    }
}