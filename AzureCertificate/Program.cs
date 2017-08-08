namespace AzureCertificate
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.Azure;
    using Microsoft.WindowsAzure.Management.Compute;
    using Microsoft.WindowsAzure.Management.Compute.Models;

    internal class Program
    {
        private static void Main(String[] args)
        {
            var subscription = " "; //oc
            var credentials = new CertificateCloudCredentials(subscription, GetCertificate());


            using (var computeManagementClient = new ComputeManagementClient(credentials, new Uri("https://management.core.windows.net")))
            {
                //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://management.core.windows.net/" + subscription + "/services/hostedservices");
                //request.ClientCertificates.Add(GetCertificate());
                //request.Headers.Add("x-ms-version", "2015-04-01");
                //request.ContentType = "application/xml";
                //var response = request.GetResponse();
                //var responseStream = response.GetResponseStream();


                var services = computeManagementClient.HostedServices.List();
                foreach (var item in services)
                {
                    var serviceDetail = computeManagementClient.HostedServices.GetDetailed(item.ServiceName);

                    var deployments = new List<HostedServiceGetDetailedResponse.Deployment>();
                    foreach (var deployment in serviceDetail.Deployments)
                    {
                        var tempDeployment = new HostedServiceGetDetailedResponse.Deployment
                        {
                            DeploymentSlot = (DeploymentSlot) Enum.Parse(typeof(DeploymentSlot), deployment.DeploymentSlot.ToString()), RoleInstances = new List<RoleInstance>()
                        };
                        foreach (var roleInstance in deployment.RoleInstances)
                        {
                        }
                        tempDeployment.Roles = new List<Role>();
                        foreach (var role in deployment.Roles)
                        {
                        }
                        tempDeployment.Status = (DeploymentStatus) Enum.Parse(typeof(DeploymentStatus), deployment.Status.ToString());
                        deployments.Add(tempDeployment);
                    }
                }
            }
        }


        private static X509Certificate2 GetCertificate()
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            var thumbprint = "Test";
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false)[0];
            return certificates;
        }
    }
}