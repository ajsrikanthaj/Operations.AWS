using System.Collections.Generic;
using System.IO;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class KeypairOperations : AmazonEc2ClientProvider
    {
        public KeypairOperations(string accessKey, string secretKey, string regionSystemName)
            : base(accessKey, secretKey, regionSystemName)
        {
        }

        public List<KeyPairInfo> GetKeypairInfo(string keypairName = "")
        {
            DescribeKeyPairsRequest keyPairRequest = new DescribeKeyPairsRequest();
            if (keypairName != "") keyPairRequest.KeyNames = new List<string> { keypairName };
            DescribeKeyPairsResponse keyPairResponse = amazonEc2Client.DescribeKeyPairs(keyPairRequest);

            return keyPairResponse.KeyPairs;
        }

        public void CreateKeypair(string keyPairName, string keyFileLocation)
        {
            var newKeyRequest = new CreateKeyPairRequest()
            {
                KeyName = keyPairName
            };
            var ckpResponse = amazonEc2Client.CreateKeyPair(newKeyRequest);

            // Save the private key in a .pem file
            string keyFileName = keyFileLocation + keyPairName + ".pem";
            using (FileStream s = new FileStream(keyFileName, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(s))
                {
                    writer.WriteLine(ckpResponse.KeyPair.KeyMaterial);
                }
            }
        }

        public void DeleteKeypair(string keyPairName)
        {
            var deleteKeyRequest = new DeleteKeyPairRequest()
            {
                KeyName = keyPairName
            };
            var deleteResponse = amazonEc2Client.DeleteKeyPair(deleteKeyRequest);
        }
    }
}