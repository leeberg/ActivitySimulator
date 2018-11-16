using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileActivitySimulator
{
    public class CredentialManager
    {
        public NetworkCredential UserCredential;
        public string userName;
        public string userDomain;

        public void SetupUserCreds(string username, string password, string domain)
        {
            userName = username;
            userDomain = domain;

            NetworkCredential newcred = new NetworkCredential(username, password, domain);
            UserCredential = newcred;

        }
        
        

    }
    
}
