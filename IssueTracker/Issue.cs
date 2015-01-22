using System;

namespace IssueTracker
{
    public class Issue
    {
        public string ProjectKey { get; private set; }

        public string Number { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public DateTime DueDate { get; private set; }

        public string CreatedBy { get; private set; }

        public string AssignedTo { get; private set; }

        internal Issue(string projectKey, string number, string title, string description, DateTime dueDate, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Invalid issue title", "title");

            ProjectKey = projectKey;
            Number = number;
            Title = title;
            Description = description;
            DueDate = dueDate;
            CreatedBy = createdBy;
        }

        public void AssignTo(string username)
        {
            AssignedTo = username;
        }
    }
}