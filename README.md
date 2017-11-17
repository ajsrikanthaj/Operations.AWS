# Operations.AWS
A C#.NET library to perform operations on AWS EC2 resources. It's written as a wrapper around the AWS SDK for .NET provided by Amazon.

Basic useful feature list:

 * Covered all basic operations on the most used resources
 * Operations can be performed just by calling methods in the respective classes.
 * No need of building ec2Client each time
 
 * Resources with detailed operations coverage:
    * Regions
 	* Instances
 	* Volumes
 	* Snapshots
 	* AMIs
 	* Keypairs
 * Resources with basic operations coverage (will be worked on more)
    * VPC
    * Network Interfaces
    * Security Groups
    * Subnets .

And here's few example codes for a client program! :+1:

```C#
RegionsOperations regionsOperation = new RegionsOperations("<AccessKey>", "<SecretKey>", RegionEndPoint.USWest2);
List<regions> regionList = regionsOperations.GetAwsRegionsList();

InstanceOperations instanceOperation = new InstanceOperations("<AccessKey>", "<SecretKey>", RegionEndPoint.USWest2);
List<string> instancesToStart = new List<string> { "<instanceId1>", "<instanceId2>" };
instanceOperation.StartInstance(instancesToStart);
```

This is [on GitHub](https://github.com/ajsrikanthaj/Operations.AWS) so let me know if I've broked it somewhere.


### Stuff used to make this:

 * [AWS SDK for .NET Version 3 API Reference](http://docs.aws.amazon.com/sdkfornet/v3/apidocs/Index.html)
