using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class NicOperations : AmazonEc2ClientValidator
    {
        public NicOperations(string accessKey, string secretKey, string regionSystemName) 
            : base(accessKey, secretKey, regionSystemName)
        {
        }

        public InstanceNetworkInterfaceSpecification CreateNetworkInterfaceSpecification(int deviceIndex, SecurityGroup securityGroup, Subnet subnet, NetworkInterface networkInterface, bool associateePblicIp)
        {
            return CreateNetworkInterfaceSpecification(deviceIndex, securityGroup.GroupId, subnet.SubnetId, networkInterface.PrivateIpAddress, associateePblicIp);
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
    }
}
