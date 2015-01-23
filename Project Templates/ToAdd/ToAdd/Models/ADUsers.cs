using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace DGII_PFD.Models
{
    public class ADUsers
    {
        public static ADUsers Instance { get { return SingletonHolder.Instance; } }
        public Boolean Completed{ get; private set; }
        private IOrderedEnumerable<KeyValuePair<string, string>> usr;
        private Dictionary<string, string> dbDisplayNames;
        private Dictionary<string, Principal> dbusers;
        private IOrderedEnumerable<Principal> results;
        private ADUsers()
        {
            dbDisplayNames = new Dictionary<string, string>();
            dbusers = new Dictionary<string, Principal>();
            Refresh();
            Completed = true;
        }

        public IOrderedEnumerable<KeyValuePair<string, string>> GetUsers()
        {
            return usr;
        }

        public IOrderedEnumerable<Principal> GetResults()
        {
            return results;
        }

        public Principal GetUser(string NombreUsuario)
        {
            return dbusers[NombreUsuario];
        }

        public string GetDisplayName(string DomainName)
        {
            return dbDisplayNames[DomainName];
        }

        private void Refresh(List<string> toExclude = null)
        {
            dbDisplayNames = new Dictionary<string, string>();
            dbusers = new Dictionary<string, Principal>();
            List<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
            PrincipalContext context = new PrincipalContext(ContextType.Domain);
            UserPrincipal template = new UserPrincipal(context);
            template.Enabled = true;
            PrincipalSearcher searcher = new PrincipalSearcher(template);
            results = searcher.FindAll().OrderBy(r => r.SamAccountName);
            foreach (Principal result in results.Where(r => !String.IsNullOrEmpty(r.UserPrincipalName) && !String.IsNullOrEmpty(r.Description)))
            {
                if (toExclude == null)
                {
                    users.Add(new KeyValuePair<string, string>(result.SamAccountName, result.DisplayName));
                }
                else if (!toExclude.Contains(string.Format("DGII\\{0}", result.SamAccountName)))
                {
                    users.Add(new KeyValuePair<string, string>(result.SamAccountName, result.DisplayName));
                }
                dbDisplayNames.Add(string.Format("DGII\\{0}", result.SamAccountName), result.DisplayName);
                dbusers.Add(string.Format("DGII\\{0}", result.SamAccountName), result);
            }

            usr = users.OrderBy(t => t.Key);
        }


        public void ReInstanciate(List<string> toExclude = null)
        {
            Refresh(toExclude);
        }


        private class SingletonHolder
        {
            static SingletonHolder() { }
            internal static readonly ADUsers Instance = new ADUsers();
        }
    }
}