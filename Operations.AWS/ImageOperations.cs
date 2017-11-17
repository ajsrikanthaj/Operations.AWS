using System.Collections.Generic;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace Operations.AWS
{
    public class ImageOpertions : AmazonEc2ClientProvider
    {
        public ImageOpertions(string accessKey, string secretKey, string regionSystemName) 
            : base(accessKey, secretKey, regionSystemName)
        {
        }

        public List<Image> GetAMIs(List<string> amiIds)
        {
            DescribeImagesRequest imageRequest = new DescribeImagesRequest();
            imageRequest.ImageIds = amiIds;
            DescribeImagesResponse imageResponse = amazonEc2Client.DescribeImages(imageRequest);

            return imageResponse.Images;
        }

        public List<Image> GetOwnAMIs(List<string> amiIds = null)
        {
            DescribeImagesRequest imageRequest = new DescribeImagesRequest();
            imageRequest.Owners = new List<string> { "self" };
            if (amiIds != null) imageRequest.ImageIds = amiIds;
            DescribeImagesResponse imageResponse = amazonEc2Client.DescribeImages(imageRequest);

            return imageResponse.Images;
        }

        public string CreateAmi(Instance instance, string amiName, string description = "", bool noReboot = false)
        {
            // By default, Amazon EC2 attempts to shut down and reboot the instance before creating the image.
            CreateImageRequest imageRequest = new CreateImageRequest(instance.InstanceId, amiName)
            {
                Description = description,
                NoReboot = noReboot
            };

            // For Amazon EBS-backed instances, 'CreateImage' creates and registers the AMI in a single request, so no need to register the AMI seperately.
            var imageResponse = amazonEc2Client.CreateImage(imageRequest);

            return imageResponse.ImageId;
        }

        public List<Image> GetAmisByTag(string tagname, string value)
        {
            DescribeImagesRequest imagesRequest = new DescribeImagesRequest();
            imagesRequest.Owners = new List<string> { "self" };

            string tagKey = "tag:" + tagname;
            imagesRequest.Filters.Add(new Filter(tagKey, new List<string> { value }));
            imagesRequest.Filters.Add(new Filter("state", new List<string> { ImageState.Available }));

            var imagesResponse = amazonEc2Client.DescribeImages(imagesRequest);

            return imagesResponse.Images.Count > 0 ? imagesResponse.Images : null;
        }

        public void DeregisterAmisByName(string amiName)
        {
            DescribeImagesRequest imagesRequest = new DescribeImagesRequest();
            imagesRequest.Owners = new List<string> { "self" };
            imagesRequest.Filters.Add(new Filter("Name", new List<string> { amiName })); //To Do: name
            imagesRequest.Filters.Add(new Filter("state", new List<string> { ImageState.Available }));
            var imagesResponse = amazonEc2Client.DescribeImages(imagesRequest);

            DeregisterImageRequest imageDeregisterRequest = new DeregisterImageRequest(imagesResponse.Images[0].ImageId);
            amazonEc2Client.DeregisterImage(imageDeregisterRequest);
        }

        public void DeregisterAmi(string amiId)
        {
            DescribeImagesRequest imagesRequest = new DescribeImagesRequest();
            imagesRequest.Owners = new List<string> { "self" };
            imagesRequest.ImageIds = new List<string> { amiId };
            imagesRequest.Filters.Add(new Filter("state", new List<string> { ImageState.Available }));
            var imagesResponse = amazonEc2Client.DescribeImages(imagesRequest);

            DeregisterImageRequest imageDeregisterRequest = new DeregisterImageRequest(imagesResponse.Images[0].ImageId);
            amazonEc2Client.DeregisterImage(imageDeregisterRequest);
        }

        public string CopyAMI(Image ami, RegionEndpoint sourceRegion)
        {
            CopyImageRequest copyRequest = new CopyImageRequest();
            copyRequest.SourceImageId = ami.ImageId;
            copyRequest.SourceRegion = sourceRegion.SystemName;
            copyRequest.Name = ami.Name;
            copyRequest.Description = ami.Description;

            var copyResponse = amazonEc2Client.CopyImage(copyRequest);

            return copyResponse.ImageId;
        }

        public void ModifyAmiPermission(Image ami, bool Public)
        {
            ModifyImageAttributeRequest modifyRequest = new ModifyImageAttributeRequest(ami.ImageId, "launchPermission");

            var launchPermission = new LaunchPermissionModifications();
            if (Public)
            {
                launchPermission.Add.Add(new LaunchPermission { Group = "all" });
            }
            else
            {
                launchPermission.Remove.Add(new LaunchPermission { Group = "all" });
            }

            modifyRequest.LaunchPermission = launchPermission;

            amazonEc2Client.ModifyImageAttribute(modifyRequest);
        }
    }
}