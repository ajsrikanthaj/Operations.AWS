using System.Collections.Generic;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class VolumeOperations : AmazonEc2ClientProvider
    {
        public VolumeOperations(string accessKey, string secretKey, string regionSystemName) 
            : base(accessKey, secretKey, regionSystemName)
        {
        }

        public Volume CreateVolume(AvailabilityZone zone, int size, VolumeType volumeType)
        {
            CreateVolumeRequest createRequest = new CreateVolumeRequest(zone.ZoneName, size);
            createRequest.VolumeType = volumeType.Value;
            CreateVolumeResponse createResponse = amazonEc2Client.CreateVolume(createRequest);

            return createResponse.Volume;
        }

        public void AttachVolume(string instanceId, string volumeID, string deviceName)
        {
            AttachVolumeRequest attachRequest = new AttachVolumeRequest(volumeID, instanceId, deviceName);
            AttachVolumeResponse attachResponse = amazonEc2Client.AttachVolume(attachRequest);
        }

        public void DetachVolume(string volumeID, Instance instance = null, bool force = false)
        {
            DetachVolumeRequest detachRequest = new DetachVolumeRequest(volumeID);
            detachRequest.Force = force;
            if (instance != null) detachRequest.InstanceId = instance.InstanceId;
            DetachVolumeResponse detachResponse = amazonEc2Client.DetachVolume(detachRequest);
        }

        public void DeleteVolume(string volumeID)
        {
            DeleteVolumeRequest deleteRequest = new DeleteVolumeRequest(volumeID);
            amazonEc2Client.DeleteVolume(deleteRequest);
        }

        public List<Volume> GetVolumes(List<string> volumeIds)
        {
            DescribeVolumesRequest volumeRequest = new DescribeVolumesRequest(volumeIds);
            DescribeVolumesResponse volumeResponse = amazonEc2Client.DescribeVolumes(volumeRequest);

            return volumeResponse.Volumes;
        }

        public Volume CreateVolumeFromSnapshot(string availabilityZone, string snapshotID)
        {
            CreateVolumeRequest volumeRequest = new CreateVolumeRequest(availabilityZone, snapshotID);
            CreateVolumeResponse volumeResonse = amazonEc2Client.CreateVolume(volumeRequest);

            return volumeResonse.Volume;
        }

        public string GetDeviceNameOfVolume(string volumeID, List<InstanceBlockDeviceMapping> InstanceDeviceMappings)
        {
            foreach (InstanceBlockDeviceMapping mapping in InstanceDeviceMappings)
            {
                if (volumeID == mapping.Ebs.VolumeId) return mapping.DeviceName;
            }

            return string.Empty;
        }

        public string GetDeviceNameOfVolume(string volumeID, Instance instance)
        {
            foreach (InstanceBlockDeviceMapping mapping in instance.BlockDeviceMappings)
            {
                if (volumeID == mapping.Ebs.VolumeId) return mapping.DeviceName;
            }

            return string.Empty;
        }

        public void EnableDeleteOnTerminationForEbsVolumes(Instance instance)
        {
            var req = new ModifyInstanceAttributeRequest
            {
                InstanceId = instance.InstanceId,
                Attribute = new InstanceAttributeName("blockDeviceMapping")
            };

            foreach (var mapping in instance.BlockDeviceMappings)
            {
                var newMapping = new InstanceBlockDeviceMappingSpecification
                {
                    DeviceName = mapping.DeviceName,
                    Ebs = new EbsInstanceBlockDeviceSpecification
                    {
                        VolumeId = mapping.Ebs.VolumeId,
                        DeleteOnTermination = mapping.DeviceName == Operations.RootDeviceName
                    }
                };

                req.BlockDeviceMappings.Add(newMapping);
            }

            var resp = amazonEc2Client.ModifyInstanceAttribute(req);
        }

        public void AssignTag(Volume volume, string key, string value)
        {
            List<string> volumeIDs = new List<string>() { volume.VolumeId };
            List<Tag> tags = new List<Tag>() { new Tag(key, value) };

            CreateTagsRequest tagRequest = new CreateTagsRequest(volumeIDs, tags);
            CreateTagsResponse tagResponse = amazonEc2Client.CreateTags(tagRequest);
        }

        public void DeleteTag(Volume volume, string key)
        {
            DeleteTagsRequest tagRequest = new DeleteTagsRequest(new List<string>() { volume.VolumeId });
            tagRequest.Tags = new List<Tag>() { new Tag(key) };

            DeleteTagsResponse tagResponse = amazonEc2Client.DeleteTags(tagRequest);
        }
    }
}