using System.Collections.Generic;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class NicOperations : AmazonEc2ClientProvider
    {
        public NicOperations(string accessKey, string secretKey, string regionSystemName) 
            : base(accessKey, secretKey, regionSystemName)
        {
        }

        public InstanceNetworkInterfaceSpecification CreateNetworkInterfaceSpecification(int deviceIndex, string securityGroupID, string subnetID, string privateIpAddress, bool associateePblicIp)
        {
            return new InstanceNetworkInterfaceSpecification
            {
                DeviceIndex = deviceIndex,
                SubnetId = subnetID,
                Groups = new List<string>() { securityGroupID },
                DeleteOnTermination = true,
                AssociatePublicIpAddress = associateePblicIp,
                PrivateIpAddress = privateIpAddress
            };
        }

        public NetworkInterface CreateNetworkInterface(string description, string subnetId, List<string> securityGroups, string privateIpAddress)
        {
            var createRequest = new CreateNetworkInterfaceRequest()
            {
                Description = description,
                Groups = securityGroups,
                PrivateIpAddress = privateIpAddress,
                SubnetId = subnetId
            };
            var createResponse = amazonEc2Client.CreateNetworkInterface(createRequest);
            return createResponse.NetworkInterface;
        }

        public void DeleteNetworkInterface(string networkInterfaceId)
        {
            var deleteRequest = new DeleteNetworkInterfaceRequest()
            {
                NetworkInterfaceId = networkInterfaceId
            };
            var deleteResponse = amazonEc2Client.DeleteNetworkInterface(deleteRequest);
        }

        public List<NetworkInterface> GetNetworkInterfaes(List<string> networkInterfaceIds)
        {
            var getrequest = new DescribeNetworkInterfacesRequest()
            {
                NetworkInterfaceIds = networkInterfaceIds
            };
            var getResponse = amazonEc2Client.DescribeNetworkInterfaces(getrequest);
            return getResponse.NetworkInterfaces;
        }
    }
}