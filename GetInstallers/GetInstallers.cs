using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using DynamoDBHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using User;

namespace GetInstallers
{
    public class GetInstallers
    {
        public static async Task<List<string>> RetrieveInstallers(string organisationId)
        {
            var installers = new List<string>();

            try
            {
                var dynamoDbClient = DynamoDBClientCreator.CreateClient();

                var request = new ScanRequest
                {
                    TableName = TableNames.user,
                    FilterExpression = "organisationId = :organisationId AND accountType = :accountType",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    {
                        { ":organisationId", new AttributeValue(organisationId) },
                        { ":accountType", new AttributeValue(UserTypes.installer) }
                    }
                };

                var response = await dynamoDbClient.ScanAsync(request).ConfigureAwait(true);

                LambdaLogger.Log("GetInstallers:: recieved " + response.Items.Count + " installers from table");

                foreach (var item in response.Items)
                {
                    var user = User.User.deserialiseAsUser(item);
                    installers.Add(user.name);
                }
            }
            catch (Exception e)
            {
                LambdaLogger.Log("Unable to retrieve installers: " + e.ToString());
            }

            return installers;
        }
    }
}
