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
    public class Operations
    {
        #region Private Members
        protected internal IAmazonEC2 amazonEc2Client = null;
        #endregion

        #region Public Members
        public static string RootDeviceName = "/dev/sda1";
        #endregion

        #region Constructors
        public Operations(string accessKey, string secretKey, RegionEndpoint region)
        {
            ValidateAmazonEc2Client(accessKey, secretKey, region);
        }

        public Operations(string accessKey, string secretKey, string regionSystemName) 
            : this(accessKey, secretKey, RegionEndpoint.GetBySystemName(regionSystemName))
        { }
        #endregion

        #region Public Methods
        public static List<RegionEndpoint> GetStaticListOfRegions()
        {
            return RegionEndpoint.EnumerableAllRegions.ToList();
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

        public List<SecurityGroup> GetSecurityGroups(Vpc vpc = null)
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
            DescribeSecurityGroupsRequest securityGroupRequest = new DescribeSecurityGroupsRequest();
            if (vpcFilter != null) securityGroupRequest.Filters.Add(vpcFilter);
            DescribeSecurityGroupsResponse securityGroupResponse = amazonEc2Client.DescribeSecurityGroups(securityGroupRequest);

            return securityGroupResponse.SecurityGroups;
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

        public List<KeyPairInfo> GetKeypairInfo(string keypairName = "")
        {
            DescribeKeyPairsRequest keyPairRequest = new DescribeKeyPairsRequest();
            if (keypairName != "") keyPairRequest.KeyNames = new List<string> { keypairName };
            DescribeKeyPairsResponse keyPairResponse = amazonEc2Client.DescribeKeyPairs(keyPairRequest);

            return keyPairResponse.KeyPairs;
        }
        
        public List<AvailabilityZone> GetAvailabilityZones()
        {
            return amazonEc2Client.DescribeAvailabilityZones().AvailabilityZones;
        }
        
        public List<RegionEndpoint> GetAwsRegionsList()
        {
            DescribeRegionsResponse regionsResponse = amazonEc2Client.DescribeRegions();
            var regions = regionsResponse.Regions;

            List<RegionEndpoint> resultList = new List<RegionEndpoint>();
            foreach (var region in regions)
            {
                resultList.Add(RegionEndpoint.GetBySystemName(region.RegionName));
            }
            return resultList;
        }
        #endregion

        #region Private Methods
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
        #endregion
    }
}