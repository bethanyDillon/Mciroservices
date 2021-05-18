using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using DynamoDBHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using User;

namespace GetUser
{
    public static class GetUser
    {
        public static async Task<User.User> RetrieveUser(string email)
        {
            var user = new User.User();

            try
            {
                var dynamoDbClient = DynamoDBClientCreator.CreateClient();

                var request = new ScanRequest
                {
                    TableName = TableNames.user,
                    FilterExpression = "email = :email",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    {
                        { ":email", new AttributeValue(email) }
                    }
                };

                var response = await dynamoDbClient.ScanAsync(request).ConfigureAwait(true);

                var userItm = response.Items[0];

                user = User.User.deserialiseAsUser(userItm);
            }
            catch (Exception e)
            {
                LambdaLogger.Log("Unable to retrieve codes: " + e.ToString());
            }

            return user;
        }
    }
}
