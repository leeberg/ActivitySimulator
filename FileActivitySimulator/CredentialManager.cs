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
        public NetworkCredential UserCredential { get; set; }
        public string userName { get; set; }
        public string userDomain { get; set; }
        public string userPassword { get; set; }



        public NetworkCredential SetupUserCreds(string username, string password, string domain)
        {
            // pass strings into a Network Cred Object

            NetworkCredential newcred = new NetworkCredential(username, password, domain);
            //UserCredential = newcred;
            return newcred;

            

        }
        
        

    }
    
}
