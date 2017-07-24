namespace OutlookCerti
{
    using System;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    internal class Program
    {
        private static void Main(String[] args)
        {
            var certPath = "cer.pfx";
            var cerPassword = "password";

            var ResourceExchange = "https://outlook.office365.com";

            var authority = "https://login.microsoftonline.com/65147614-9a4d-4481-9d97-f3ee8eda2129/oauth2/token";

            var authenticationContext = new AuthenticationContext(authority, false);
            if (authenticationContext.TokenCache != null)
            {
                authenticationContext.TokenCache.Clear();
            }

            var certBytes = File.ReadAllBytes(certPath);
            var cert = new X509Certificate2(certBytes, cerPassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            var cac = new ClientAssertionCertificate("da8cf645-c01f-44ca-a0da-2676d4fba2cc", cert);
            var authenticationResult = authenticationContext.AcquireTokenAsync(ResourceExchange, cac)
                .Result;
        }
    }
}