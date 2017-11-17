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
    public class ImageOpertions : AmazonEc2ClientValidator
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
    }
}
