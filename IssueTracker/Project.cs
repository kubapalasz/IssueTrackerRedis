using System;
using System.Collections.Generic;
using System.Linq;

namespace IssueTracker
{
    public class Project
    {
        private const int MaxKeyLength = 8;
        private readonly IList<Issue> _issues = new List<Issue>();
        private int _counter;

        public string Key { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<Issue> Issues { get { return _issues; } }
        public string CreatedBy { get; private set; }

        internal Project(string key, string name, string createdBy, int counter, IEnumerable<Issue> issues)
            : this(key, name, createdBy)
        {
            _counter = counter;
            _issues = new List<Issue>(issues);
        }

        public Project(string key, string name, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Project key cannot be empty", "key");

            if (key.Length > MaxKeyLength)
                throw new ArgumentException(string.Format("Project key cannot be longer than {0} characters", MaxKeyLength), "key");

            if (key.Contains(" "))
                throw new ArgumentException("Project key cannot contain spaces", "key");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be empty", "name");

            Name = name;
            Key = key;
            CreatedBy = createdBy;
        }

        public void CreateIssue(string title, string description, DateTime dueDate, string createdBy)
        {
            var number = string.Format("{0}-{1}", Key, ++_counter);

            _issues.Add(new Issue(Key, number, title, description, dueDate, createdBy));
        }
    }
}