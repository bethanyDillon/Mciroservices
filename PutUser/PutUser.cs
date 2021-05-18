using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using DynamoDBHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using User;

namespace PutUser
{
    public class PutUser
    {
        public static async Task<User.User> CreateNewUser(Dictionary<string, string> input)
        {
            var user = new User.User();
            //Check user exists
            var dynamoDbClient = DynamoDBClientCreator.CreateClient();

            try
            {
                var request = new ScanRequest
                {
                    TableName = TableNames.user,
                    FilterExpression = "email = :email",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    {
                        { ":email", new AttributeValue(input["email"]) }
                    }
                };

                var response = await dynamoDbClient.ScanAsync(request).ConfigureAwait(true);

                var userItm = response.Items[0];

                user = User.User.deserialiseAsUser(userItm);
                return user;
            } 
            catch (Exception e)
            {

            }
            
            var userId = Guid.NewGuid().ToString();

            try
            {

                var item = new Dictionary<string, AttributeValue>();
                
                
                foreach (var value in input)
                {
                    item.Add(value.Key, new AttributeValue(value.Value));
                }

                item.Add("userId", new AttributeValue(userId));

                var request = new PutItemRequest
                {
                    TableName = TableNames.user,
                    Item = item
                };

                await dynamoDbClient.PutItemAsync(request).ConfigureAwait(true);
            }
            catch (Exception e)
            {
                LambdaLogger.Log("Unable to retrieve codes: " + e.ToString());
            }

            user.userId = userId;
            user.accountType = input["accountType"];
            user.email = input["email"];
            user.name = input["name"];
            user.organisationId = input["organisationId"];

            return user;
        }
    }
}
