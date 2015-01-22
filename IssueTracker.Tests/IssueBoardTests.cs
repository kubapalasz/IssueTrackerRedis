using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace IssueTracker.Tests
{
    [TestFixture]
    public class IssueBoardTests
    {
        [Test]
        public void CreateIssue_NotLoggedIn_ShouldThrow()
        {
            var board = new IssueBoard();

            Action act = () => board.CreateIssue("TEST", "Test issue", string.Empty, DateTime.Today);

            act.ShouldThrow<AccessViolationException>()
                .WithMessage("Not logged in");
        }

        [Test]
        public void CreateIssue_LoggedIn_ShouldSetCreatedBy()
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");

            board.CreateProject("P-Key", "Test Project");
            board.CreateIssue("P-Key", "Test issue", string.Empty, DateTime.Today);

            var issue = board.Issues.First();
            issue.CreatedBy.Should().Be("username");
        }

        [Test]
        public void CreateIssue_ShouldHaveReadableIncrementingNumbersAssigned()
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");

            board.CreateProject("P-Key", "Test Project");
            board.CreateIssue("P-Key", "Title", string.Empty, new DateTime(2000, 12, 22));
            board.CreateIssue("P-Key", "Title", string.Empty, new DateTime(2000, 12, 21));
            board.CreateIssue("P-Key", "Title", string.Empty, new DateTime(2000, 12, 20));

            var issues = board.Issues.ToList();
            issues[0].Number.Should().Be("P-Key-1");
            issues[1].Number.Should().Be("P-Key-2");
            issues[2].Number.Should().Be("P-Key-3");
        }

        [Test]
        public void CreateIssue_WithNonExistingProjectKey_ShouldThrowException()
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");

            Action act = () => board.CreateIssue("P-Key", "Title", string.Empty, new DateTime(2000, 12, 22));

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("projectKey");
        }

        [Test]
        public void CreateIssues_ForMultipleProjects_ShouldHaveReadableIncrementingNumbersAssigned()
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");
            board.CreateProject("PRJ1", "Test Project");
            board.CreateProject("PRJ2", "Test Project");
            board.CreateIssue("PRJ1", "Title", string.Empty, new DateTime(2000, 12, 22));
            board.CreateIssue("PRJ1", "Title", string.Empty, new DateTime(2000, 12, 21));
            board.CreateIssue("PRJ2", "Title", string.Empty, new DateTime(2000, 12, 20));
            board.CreateIssue("PRJ2", "Title", string.Empty, new DateTime(2000, 12, 19));
            board.CreateIssue("PRJ1", "Title", string.Empty, new DateTime(2000, 12, 18));

            var issues = board.Issues
                .OrderByDescending(issue => issue.DueDate)
                .ToList();
            
            issues[0].Number.Should().Be("PRJ1-1");
            issues[1].Number.Should().Be("PRJ1-2");

            issues[2].Number.Should().Be("PRJ2-1");
            issues[3].Number.Should().Be("PRJ2-2");

            issues[4].Number.Should().Be("PRJ1-3");
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("\r\n")]
        [TestCase(null)]
        public void CreateIssue_WithInvalidTitle_ShouldThrowArgumentException(string title)
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");
            board.CreateProject("P-Key", "Test Project");

            Action act = () => board.CreateIssue("P-Key", title, string.Empty, DateTime.Now);

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("title");
        }
    }
}