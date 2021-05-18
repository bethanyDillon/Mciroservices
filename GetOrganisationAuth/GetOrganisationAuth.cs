using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using DynamoDBHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GetOrganisationAuth
{
    public class GetOrganisationAuth
    {
        public static async Task<Dictionary<string, string>> GetOrganisationAuthCodes()
        {
            var authCodes = await GetCodes().ConfigureAwait(true);

            return authCodes;
        }

        public static async Task<Dictionary<string, string>> GetCodes()
        {
            var authCodes = new Dictionary<string, string>();

            try
            {
                var dynamoDbClient = DynamoDBClientCreator.CreateClient();

                var request = new ScanRequest
                {
                    TableName = TableNames.organisation
                };
                var allDocs = await dynamoDbClient.ScanAsync(request).ConfigureAwait(true);

                foreach (var item in allDocs.Items)
                {
                    if (item.ContainsKey("authCode"))
                    {
                        authCodes.Add(item["organisationId"].S, item["authCode"].S);
                    }
                }
            }
            catch (Exception e)
            {
                LambdaLogger.Log("Unable to retrieve codes: " + e.ToString());
            }            

            return authCodes;
        }
    }
}
