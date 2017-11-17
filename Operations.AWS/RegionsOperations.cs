using System.Collections.Generic;
using System.Linq;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class RegionsOperations : AmazonEc2ClientProvider
    {
        public RegionsOperations(string accessKey, string secretKey, string regionSystemName) 
            : base(accessKey, secretKey, regionSystemName)
        {
        }

        public static List<RegionEndpoint> GetStaticListOfRegions()
        {
            return RegionEndpoint.EnumerableAllRegions.ToList();
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
    }
}