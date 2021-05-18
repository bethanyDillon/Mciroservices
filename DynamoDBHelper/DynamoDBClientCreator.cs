using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using System;

namespace DynamoDBHelper
{
    public class DynamoDBClientCreator
    {
		public static AmazonDynamoDBClient CreateClient()
		{
			var awsCredentials = new BasicAWSCredentials("AKIAWVMMSCQ2HD33T7AR", "60S+HD+V0eKf9XqW/0uHvCRW6C25ur9FEOV/znlB");
			return new AmazonDynamoDBClient(awsCredentials, RegionEndpoint.EUWest2);
		}
	}
}
