using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using System.DirectoryServices.Protocols;
using System.Net;

namespace siapv_backend.Services
{
    public class LdapService
    {
        private readonly string _ldapDomain = "10.0.74.10"; // Replace with your domain
        private readonly string _ldapContainer = "OU=Usuarios,DC=aisem,DC=gob,DC=bo"; // Replace with your LDAP container
        public bool AuthenticateAsync(string username, string password)
        {
            try
            {
                using (var ldapConnection = new LdapConnection(
                   new LdapDirectoryIdentifier(_ldapDomain, 389)))
                {
                    var networkCredential = new NetworkCredential($"AISEM\\{username}", password);
                    ldapConnection.Credential = networkCredential;
                    ldapConnection.AuthType = AuthType.Basic;
                    ldapConnection.SessionOptions.ProtocolVersion = 3;
                    ldapConnection.SessionOptions.SecureSocketLayer = false;

                    ldapConnection.Bind();
                    return true;
                }
            }
            catch (LdapException error)
            {
                Console.WriteLine(error);
                return false;
            }
        }
    }
}