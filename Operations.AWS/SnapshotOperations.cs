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
    public partial class SnapshotOperations : AmazonEc2ClientValidator
    {
        public SnapshotOperations(string accessKey, string secretKey, string regionSystemName) 
            : base(accessKey, secretKey, regionSystemName)
        {
        }

        public SnapshotOperations(string accessKey, string secretKey, RegionEndpoint region) : base(accessKey, secretKey, region)
        {
        }

        public List<Snapshot> GetSnapshots(List<string> snapshotIds)
        {
            DescribeSnapshotsRequest snapshotRequest = new DescribeSnapshotsRequest();
            snapshotRequest.SnapshotIds = snapshotIds;
            DescribeSnapshotsResponse snapsotResponse = amazonEc2Client.DescribeSnapshots(snapshotRequest);
            return snapsotResponse.Snapshots;
        }

        public void DeleteVolumeSnapshots(string snapshotId)
        {
            DeleteSnapshotRequest deleteSnapshotRequest = new DeleteSnapshotRequest(snapshotId);
            amazonEc2Client.DeleteSnapshot(deleteSnapshotRequest);
        }
    }
}
