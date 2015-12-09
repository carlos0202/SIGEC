using System;
using System.Diagnostics;

namespace TheWorld.Services{

    public class DebugMailService : IMailServices
    {
        bool IMailServices.SendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending mail: to{to}, Subject: {subject}");
			
			return true;
        }
    }
}