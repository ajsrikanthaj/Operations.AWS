using System.Collections.Generic;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class SecurityGroupOperations : AmazonEc2ClientProvider
    {
        public SecurityGroupOperations(string accessKey, string secretKey, string regionSystemName)
            : base(accessKey, secretKey, regionSystemName)
        {
        }

        public List<SecurityGroup> GetSecurityGroups(string vpcId = null)
        {
            Filter vpcFilter = null;
            if (vpcId != null)
            {
                vpcFilter = new Filter
                {
                    Name = "vpc-id",
                    Values = new List<string>() { vpcId }
                };
            }
            DescribeSecurityGroupsRequest securityGroupRequest = new DescribeSecurityGroupsRequest();
            if (vpcFilter != null) securityGroupRequest.Filters.Add(vpcFilter);
            DescribeSecurityGroupsResponse securityGroupResponse = amazonEc2Client.DescribeSecurityGroups(securityGroupRequest);

            return securityGroupResponse.SecurityGroups;
        }

        public string CreateSecurityGroup(string secGroupName, string description, string vpcId = null)
        {
            var secGroupRequest = new CreateSecurityGroupRequest()
            {
                GroupName = secGroupName,
                Description = description
            };
            if (vpcId != null) secGroupRequest.VpcId = vpcId;

            var csgResponse = amazonEc2Client.CreateSecurityGroup(secGroupRequest);
            return csgResponse.GroupId;
        }

        public void DeleteSecurityGroup(string secGroupId)
        {
            var deleterequest = new DeleteSecurityGroupRequest()
            {
                GroupId = secGroupId
            };
            var deleteResponse = amazonEc2Client.DeleteSecurityGroup(deleterequest);
        }

        public void AddRulesToSecurityGroup(SecurityGroup securityGroup, string ipProtocol, int fromPort, int toPort, List<string> ipRange)
        {
            var ipPermission = new IpPermission()
            {
                IpProtocol = ipProtocol,
                FromPort = fromPort,
                ToPort = toPort,
                IpRanges = ipRange
            };

            var ingressRequest = new AuthorizeSecurityGroupIngressRequest();
            ingressRequest.GroupId = securityGroup.GroupId;
            ingressRequest.IpPermissions.Add(ipPermission);
            var ingressResponse = amazonEc2Client.AuthorizeSecurityGroupIngress(ingressRequest);
        }
    }
}