using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using DynamoDBHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using User;

namespace GetInstallManagers
{
    public class GetInstallManagers
    {
        public static async Task<List<string>> RetrieveInstallManagers(string organisationId)
        {
            var installerManagers = new List<string>();

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
                        { ":accountType", new AttributeValue(UserTypes.installManager) }
                    }
                };

                var response = await dynamoDbClient.ScanAsync(request).ConfigureAwait(true);

                LambdaLogger.Log("GetInstallers:: recieved " + response.Items.Count + " installermanagers from table");

                foreach (var item in response.Items)
                {
                    var user = User.User.deserialiseAsUser(item);
                    installerManagers.Add(user.name);
                }
            }
            catch (Exception e)
            {
                LambdaLogger.Log("Unable to retrieve installmanagers: " + e.ToString());
            }

            return installerManagers;
        }
    }
}
