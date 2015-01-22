using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace IssueTracker.Tests
{
    [TestFixture]
    public class IssueTests
    {
        [Test]
        public void AssignIssue_ToExistingUser_ShouldUpdateAssignedTo()
        {
            var board = new IssueBoard();
            board.CreateUser("username1", "password1");
            board.CreateUser("username2", "password2");
            board.Login("username1", "password1");

            board.CreateProject("TEST", "Test Project");
            board.CreateIssue("TEST", "Test issue", string.Empty, DateTime.Today);
            
            var issue = board.Issues.First();
            issue.AssignTo("username2");

            issue.AssignedTo.Should().Be("username2");
        }
    }
}