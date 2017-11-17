using System.Collections.Generic;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class SnapshotOperations : AmazonEc2ClientProvider
    {
        public SnapshotOperations(string accessKey, string secretKey, string regionSystemName) 
            : base(accessKey, secretKey, regionSystemName)
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
