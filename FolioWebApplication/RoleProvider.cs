using FolioLibrary;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace FolioWebApplication
{
    public class RoleProvider : System.Web.Security.RoleProvider
    {
        private readonly/* static*/ FolioServiceClient folioServiceClient = new FolioServiceClient();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);
        private string accessToken;

        public RoleProvider()
        {
            using (var fsc = new FolioServiceClient())
            {
                fsc.Authenticate();
                accessToken = fsc.AccessToken;
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames) => throw new NotImplementedException();

        public override string ApplicationName
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override void CreateRole(string roleName) => throw new NotImplementedException();

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) => throw new NotImplementedException();

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) => throw new NotImplementedException();

        public override string[] GetAllRoles() => throw new NotImplementedException();

        public override string[] GetRolesForUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));
            //lock (this)
            //{
            //    var u = folioServiceClient.Users($"username == \"{Regex.Replace(userName, @"^.+\\", "", RegexOptions.Compiled)}\"").SingleOrDefault();
            //    return u != null ? folioServiceClient.PermissionsUsers($"userId == \"{u["id"]}\"").Single()["permissions"].Select(jt => jt.ToString()).ToArray() : new string[] { };
            //}

            //using (var fsc = new FolioServiceClient(accessToken: accessToken))
            //{
            //    var u = fsc.Users($"username == \"{Regex.Replace(userName, @"^.+\\", "", RegexOptions.Compiled)}\"").SingleOrDefault();
            //    return u != null ? fsc.PermissionsUsers($"userId == \"{u["id"]}\"").Single()["permissions"].Select(jt => jt.ToString()/*.Replace("juniper-basic", "all")*/.Replace("juniper-admin-perms-sorted", "all").Replace("administrator", "all")).ToArray() : new string[] { };
            //}

            using (var fsc = new FolioServiceContext(accessToken: accessToken))
            {
                var u = fsc.User2s($"username == \"{Regex.Replace(userName, @"^.+\\", "", RegexOptions.Compiled)}\"").SingleOrDefault();
                return u != null ? new[] { $"department:{u.StaffDepartment}" } : new string[] { };
            }
        }

        public override string[] GetUsersInRole(string roleName) => throw new NotImplementedException();

        public override bool IsUserInRole(string username, string roleName) => throw new NotImplementedException();

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) => throw new NotImplementedException();

        public override bool RoleExists(string roleName) => throw new NotImplementedException();
    }
}
