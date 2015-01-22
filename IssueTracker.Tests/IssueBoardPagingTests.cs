using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace IssueTracker.Tests
{
    [TestFixture]
    public class IssueBoardPagingTests
    {
        [Test]
        public void PagingIssues_ShouldReturnCorrectIssues()
        {
            var issues1 = new[]
            {
                new Issue("PRJ1", "PRJ1-1", "Issue lorem", string.Empty, new DateTime(2000, 12, 30), "username"),
                new Issue("PRJ1", "PRJ1-2", "issue ipsum", string.Empty, new DateTime(2001, 12, 30), "username"),
                new Issue("PRJ1", "PRJ1-3", "sit amet 1", string.Empty, new DateTime(2002, 12, 30), "username"),
                new Issue("PRJ1", "PRJ1-4", "sit amet 2", string.Empty, new DateTime(2003, 12, 30), "username"),
                new Issue("PRJ1", "PRJ1-5", "sit amet 3", string.Empty, new DateTime(2004, 12, 30), "username")
            };

            var issues2 = new[]
            {
                new Issue("PRJ2", "PRJ2-1", "issue lorem 1", string.Empty, new DateTime(2000, 12, 30), "username"),
                new Issue("PRJ2", "PRJ2-2", "issue ipsum 2", string.Empty, new DateTime(2001, 12, 30), "username"),
                new Issue("PRJ2", "PRJ2-3", "issue ipsum 3", string.Empty, new DateTime(2002, 12, 30), "username"),
                new Issue("PRJ2", "PRJ2-4", "issue ipsum 4", string.Empty, new DateTime(2003, 12, 30), "username")
            };

            var board = new IssueBoard(new[]
            {
                new Project("PRJ1", "Name", "username", 5, issues1),
                new Project("PRJ2", "Name", "username", 4, issues2)
            });

            var page1 = board.Issues
                .Where(issue => issue.ProjectKey.Equals("PRJ1"))
                .OrderBy(issue => issue.Number)
                .Skip(0).Take(2).ToList();

            var page2 = board.Issues
                .Where(issue => issue.ProjectKey.Equals("PRJ1"))
                .OrderBy(issue => issue.Number)
                .Skip(2).Take(2).ToList();
            
            var page3 = board.Issues
                .Where(issue => issue.ProjectKey.Equals("PRJ1"))
                .OrderBy(issue => issue.Number)
                .Skip(4).Take(2).ToList();

            page1.Count.Should().Be(2);
            page2.Count.Should().Be(2);
            page3.Count.Should().Be(1);

            page1[0].Number.Should().Be("PRJ1-1");
            page1[1].Number.Should().Be("PRJ1-2");
            page2[0].Number.Should().Be("PRJ1-3");
            page2[1].Number.Should().Be("PRJ1-4");
            page3[0].Number.Should().Be("PRJ1-5");
        }
    }
}