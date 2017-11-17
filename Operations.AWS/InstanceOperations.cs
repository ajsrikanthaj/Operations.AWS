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
    public class InstanceOperations : AmazonEc2ClientValidator
    {
        public InstanceOperations(string accessKey, string secretKey, string regionSystemName) 
            : base(accessKey, secretKey, regionSystemName)
        {
        }

        public void StopInstance(List<string> instanceIds)
        {
            StopInstancesRequest stopRequest = new StopInstancesRequest(instanceIds);
            StopInstancesResponse stopResponse = amazonEc2Client.StopInstances(stopRequest);
        }

        public void StartInstance(List<string> instanceIds)
        {
            StartInstancesRequest startRequest = new StartInstancesRequest(instanceIds);
            StartInstancesResponse startResponse = amazonEc2Client.StartInstances(startRequest);
        }

        public void RebootInstance(List<string> instanceIds)
        {
            RebootInstancesRequest rebootRequest = new RebootInstancesRequest(instanceIds);
            RebootInstancesResponse rebootResponse = amazonEc2Client.RebootInstances(rebootRequest);
        }

        public void TerminateInstance(List<string> instanceIds)
        {
            TerminateInstancesRequest terminateRequest = new TerminateInstancesRequest(instanceIds);
            TerminateInstancesResponse terminateResponse = amazonEc2Client.TerminateInstances(terminateRequest);
        }

        public InstanceStatus GetInstanceStatus(Instance instance)
        {
            DescribeInstanceStatusRequest instanceRequest = new DescribeInstanceStatusRequest();
            instanceRequest.InstanceIds = new List<string>();
            instanceRequest.InstanceIds.Add(instance.InstanceId);
            DescribeInstanceStatusResponse response = amazonEc2Client.DescribeInstanceStatus(instanceRequest);

            return response.InstanceStatuses[0];

        }

        public Instance GetInstance(string instanceID)
        {
            DescribeInstancesRequest instanceRequest = new DescribeInstancesRequest();
            instanceRequest.InstanceIds = new List<string> { instanceID };
            DescribeInstancesResponse response = amazonEc2Client.DescribeInstances(instanceRequest);

            return response.Reservations[0].Instances[0];
        }

        public string GetNameTag(Instance instance)
        {
            foreach (Tag tag in instance.Tags)
            {
                if (tag.Key == "Name") return tag.Value;
            }

            return string.Empty;
        }

        public InstanceState GetInstanceState(Instance instance)
        {
            DescribeInstancesRequest instanceRequest = new DescribeInstancesRequest();
            instanceRequest.InstanceIds = new List<string> { instance.InstanceId };
            DescribeInstancesResponse response = amazonEc2Client.DescribeInstances(instanceRequest);

            return response.Reservations[0].Instances[0].State;
        }

        public List<InstanceBlockDeviceMapping> GetInstanceBlockDeviceMappings(Instance instance)
        {
            DescribeInstancesRequest instanceRequest = new DescribeInstancesRequest();
            instanceRequest.InstanceIds = new List<string> { instance.InstanceId };
            DescribeInstancesResponse response = amazonEc2Client.DescribeInstances(instanceRequest);

            return response.Reservations[0].Instances[0].BlockDeviceMappings;
        }

        public Instance LaunchNewInstance(SecurityGroup seccurityGroup, Subnet subnet, KeyPairInfo keypairInfo, InstanceType instanceType, List<InstanceNetworkInterfaceSpecification> enis, Image ami)
        {
            InstanceType InstanceType = InstanceType.FindValue(instanceType);

            RunInstancesRequest launchRequest = new RunInstancesRequest()
            {
                ImageId = ami.ImageId,
                InstanceType = InstanceType.Value,
                MinCount = 1,
                MaxCount = 1,
                KeyName = keypairInfo.KeyName,
                NetworkInterfaces = enis
            };
            RunInstancesResponse launchResponse = amazonEc2Client.RunInstances(launchRequest);

            return launchResponse.Reservation.Instances[0];
        }

        public void AssignTag(Instance instance, string key, string value)
        {
            List<string> instanceIDs = new List<string>() { instance.InstanceId };
            List<Tag> tags = new List<Tag>() { new Tag(key, value) };

            CreateTagsRequest tagRequest = new CreateTagsRequest(instanceIDs, tags);
            CreateTagsResponse tagResponse = amazonEc2Client.CreateTags(tagRequest);
        }

        public void DeleteTag(Instance instance, string key)
        {
            DeleteTagsRequest tagRequest = new DeleteTagsRequest(new List<string>() { instance.InstanceId });
            tagRequest.Tags = new List<Tag>() { new Tag(key) };

            DeleteTagsResponse tagResponse = amazonEc2Client.DeleteTags(tagRequest);
        }
    }
}
