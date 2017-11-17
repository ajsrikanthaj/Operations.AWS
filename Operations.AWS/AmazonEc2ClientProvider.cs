using System;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class AmazonEc2ClientProvider
    {
        protected internal IAmazonEC2 amazonEc2Client = null;

        protected internal AmazonEc2ClientProvider(string accessKey, string secretKey, RegionEndpoint region)
        {
            ValidateAmazonEc2Client(accessKey, secretKey, region);
        }        

        protected internal AmazonEc2ClientProvider(string accessKey, string secretKey, string regionSystemName) 
            : this(accessKey, secretKey, RegionEndpoint.GetBySystemName(regionSystemName))
        { }

        private void ValidateAmazonEc2Client(string accessKey, string secretKey, RegionEndpoint region)
        {
            amazonEc2Client = new AmazonEC2Client(accessKey, secretKey, region);

            // Run a simple request to validate credentials
            try
            {
                var test = amazonEc2Client.DescribeAvailabilityZones();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}