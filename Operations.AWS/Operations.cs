using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class Operations : AmazonEc2ClientProvider
    {
        public static string RootDeviceName = "/dev/sda1";

        public Operations(string accessKey, string secretKey, string regionSystemName) 
            : base(accessKey, secretKey, regionSystemName)
        {
        }
    }
}