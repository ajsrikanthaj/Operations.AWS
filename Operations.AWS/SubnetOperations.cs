using System.Collections.Generic;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class SubnetOperations : AmazonEc2ClientProvider
    {
        public SubnetOperations(string accessKey, string secretKey, string regionSystemName) 
            : base(accessKey, secretKey, regionSystemName)
        {
        }

        public List<Subnet> GetSubnets(Vpc vpc = null)
        {
            Filter vpcFilter = null;
            if (vpc != null)
            {
                vpcFilter = new Filter
                {
                    Name = "vpc-id",
                    Values = new List<string>() { vpc.VpcId }
                };
            }
            DescribeSubnetsRequest subnetRequest = new DescribeSubnetsRequest();
            if (vpcFilter != null) subnetRequest.Filters.Add(vpcFilter);
            DescribeSubnetsResponse subnetResponse = amazonEc2Client.DescribeSubnets(subnetRequest);

            return subnetResponse.Subnets;
        }
    }
}
