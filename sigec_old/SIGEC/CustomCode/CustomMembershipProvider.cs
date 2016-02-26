using SIGEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace SIGEC.CustomCode
{
    public class CustomMembershipProvider : SimpleMembershipProvider
    {
        private SIGECContext db = new SIGECContext();

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return base.GetUser(username, userIsOnline);
        }

        public override bool ValidateUser(string username, string password)
        {
            var userID = WebSecurity.GetUserId(username);
            
            return GlobalHelpers.CheckUser(userID, username, password);
        }
    }
}