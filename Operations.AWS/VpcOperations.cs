using System.Collections.Generic;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class VpcOperations : AmazonEc2ClientProvider
    {
        public VpcOperations(string accessKey, string secretKey, string regionSystemName) 
            : base(accessKey, secretKey, regionSystemName)
        {
        }

        public List<Vpc> GetVPCs(List<string> vpcIds = null)
        {
            DescribeVpcsRequest vpcRequest = new DescribeVpcsRequest();
            if (vpcIds != null)
            {
                vpcRequest.VpcIds = vpcIds;
            }
            DescribeVpcsResponse vpcResponse = amazonEc2Client.DescribeVpcs(vpcRequest);

            return vpcResponse.Vpcs;
        }
    }
}