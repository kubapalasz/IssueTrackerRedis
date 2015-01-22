using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IssueTracker
{
    public class IssueBoard
    {
        private const int MinPasswordLength = 6;

        private readonly IList<Project> _projects = new List<Project>();
        private readonly IList<User> _users = new List<User>();

        public User CurrentUser { get; private set; }

        public IEnumerable<Project> Projects
        {
            get { return _projects; }
        }

        public IQueryable<Issue> Issues
        {
            get { return _projects.SelectMany(p => p.Issues).AsQueryable(); }
        }

        public IssueBoard() { }

        internal IssueBoard(IEnumerable<Project> projects)
        {
            _projects = new List<Project>(projects);
        }

        public void CreateIssue(string projectKey, string title, string description, DateTime dueDate)
        {
            if (CurrentUser == null)
                throw new AccessViolationException("Not logged in");

            var project = _projects.FirstOrDefault(p => p.Key.Equals(projectKey, StringComparison.InvariantCultureIgnoreCase));
            if (project == null)
                throw new ArgumentException("Project with this key does not exist", "projectKey");

            project.CreateIssue(title, description, dueDate, CurrentUser.Username);
        }

        public void CreateProject(string key, string name)
        {
            if (CurrentUser == null)
                throw new AccessViolationException("Not logged in");

            if (_projects.Any(p => p.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)))
                throw new ArgumentException("Project with this key already exists", "key");

            _projects.Add(new Project(key, name, CurrentUser.Username));
        }

        public User CreateUser(string username, string password)
        {
            if (_users.Any(user => user.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)))
                throw new ArgumentException("Username already in use", "username");
            
            if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
                throw new ArgumentException("Password is too simple", "password");

            var newUser = new User(username, GetMD5(password));
            _users.Add(newUser);

            return newUser;
        }

        public void Login(string username, string password)
        {
            var user = _users.SingleOrDefault(u => u.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));

            if (user == null || user.PasswordHash != GetMD5(password))
                throw new InvalidOperationException("Unknown user or wrong password");

            CurrentUser = user;
        }

        private string GetMD5(string password)
        {
            var bytes = MD5.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(password));

            var hashBuilder = new StringBuilder();
            foreach (var @byte in bytes)
            {
                hashBuilder.Append(@byte.ToString("x2"));
            }

            return hashBuilder.ToString();
        }

        public void Logout()
        {
            if (CurrentUser == null)
                throw new InvalidOperationException("No one is logged in yet!");

            CurrentUser = null;
        }
    }
}