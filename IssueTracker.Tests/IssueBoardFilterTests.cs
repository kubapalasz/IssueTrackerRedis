using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace IssueTracker.Tests
{
    [TestFixture]
    public class IssueBoardFilterTests
    {
        [Test]
        public void GetAllIssues_ShouldReturnQueryable()
        {
            var board = new IssueBoard();
            board.CreateUser("username", "password");
            board.Login("username", "password");
            board.CreateProject("P-Key", "Test Project");
            board.CreateIssue("P-Key", "Old issue", string.Empty, new DateTime(2000, 12, 30));
            board.CreateIssue("P-Key", "Future issue", string.Empty, new DateTime(2130, 12, 30));
            board.CreateIssue("P-Key", "Current issue", string.Empty, DateTime.Now.AddDays(1));

            var issues = board.Issues
                .OrderByDescending(issue => issue.DueDate)
                .ToList();

            issues[0].Title.Should().Be("Future issue");
            issues[1].Title.Should().Be("Current issue");
            issues[2].Title.Should().Be("Old issue");
        }

        [Test]
        public void SearchIssueByName_ShouldReturnIssuesWithMatchingName()
        {
            var issues1 = new[]
            {
                new Issue("PRJ1", "PRJ1-1", "Issue lorem", string.Empty, new DateTime(2000, 12, 30), "username"),
                new Issue("PRJ1", "PRJ1-2", "issue ipsum", string.Empty, new DateTime(2001, 12, 30), "username"),
                new Issue("PRJ1", "PRJ1-3", "sit amet", string.Empty, new DateTime(2002, 12, 30), "username")
            };

            var issues2 = new[]
            {
                new Issue("PRJ2", "PRJ2-1", "issue ipsum", string.Empty, new DateTime(2002, 12, 30), "username"),
                new Issue("PRJ2", "PRJ2-2", "Issue lorem", string.Empty, new DateTime(2000, 12, 30), "username")
            };

            var board = new IssueBoard(new[]
            {
                new Project("PRJ1", "Name", "username", 3, issues1),
                new Project("PRJ2", "Name", "username", 2, issues2)
            });

            var foundIssues = board.Issues
                .Where(issue => issue.Title.Equals("issue ipsum"))
                .OrderByDescending(issue => issue.DueDate)
                .ToList();

            foundIssues[0].Number.Should().Be("PRJ2-1");
            foundIssues[1].Number.Should().Be("PRJ1-2");
        }

        [Test]
        public void SearchIssueByNameAndProjectKey_ShouldReturnIssuesWithMatchingName()
        {
            var issues1 = new[]
            {
                new Issue("PRJ1", "PRJ1-1", "Issue lorem", string.Empty, new DateTime(2000, 12, 30), "username"),
                new Issue("PRJ1", "PRJ1-2", "issue ipsum", string.Empty, new DateTime(2001, 12, 30), "username"),
                new Issue("PRJ1", "PRJ1-3", "sit amet", string.Empty, new DateTime(2002, 12, 30), "username")
            };

            var issues2 = new[]
            {
                new Issue("PRJ2", "PRJ2-1", "Issue lorem 2", string.Empty, new DateTime(2000, 12, 30), "username"),
                new Issue("PRJ2", "PRJ2-2", "issue ipsum", string.Empty, new DateTime(2001, 12, 30), "username")
            };

            var board = new IssueBoard(new[]
            {
                new Project("PRJ1", "Name", "username", 3, issues1),
                new Project("PRJ2", "Name", "username", 2, issues2)
            });

            var foundIssue = board.Issues
                .Where(issue => issue.ProjectKey.Equals("PRJ2") && issue.Title.Equals("issue ipsum"));

            foundIssue.Single().Number.Should().Be("PRJ2-2");
        }

        [Test]
        public void FilterIssues_ByIssueDescription_ShouldReturnMatchingIssues()
        {
            var issues1 = new[]
            {
                new Issue("PRJ1", "PRJ1-1", "Issue lorem", "test 1", new DateTime(2000, 12, 30), "username"),
                new Issue("PRJ1", "PRJ1-2", "issue ipsum", "lorem ipsum", new DateTime(2001, 12, 30), "username"),
                new Issue("PRJ1", "PRJ1-3", "sit amet", "test 3", new DateTime(2002, 12, 30), "username")
            };

            var issues2 = new[]
            {
                new Issue("PRJ2", "PRJ2-1", "Issue lorem 2", "test 1", new DateTime(2000, 12, 30), "username"),
                new Issue("PRJ2", "PRJ2-2", "issue ipsum", "test 2", new DateTime(2001, 12, 30), "username")
            };

            var board = new IssueBoard(new[]
            {
                new Project("PRJ1", "Name", "username", 3, issues1),
                new Project("PRJ2", "Name", "username", 2, issues2)
            });

            var foundIssues = board.Issues
                .Where(i => i.Description.Contains("ipsum"));

            foundIssues.Single().Number.Should().Be("PRJ1-2");
        }
    }
}