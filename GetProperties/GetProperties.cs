using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using DynamoDBHelper;
using Property;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using User;

namespace GetProperties
{
    public class GetProperties
    {
        public static async Task<Property.Property[]> RetrieveProperties(string[] vars)
        {
            //this.userTypes.admin + "_" + this.user.organisationId;
            //this.userTypes.installManager + "_" + this.user.userId;
            //this.userTypes.installer + "_" + this.user.userId; (and date)
            var userType = vars[0].Replace("%20", " ");
            var id = vars[1].Replace("%20", " ");

            var properties = new Property.Property[0];
            
            try
            {
                var dynamoDbClient = DynamoDBClientCreator.CreateClient();

                var filterExpression = string.Empty;
                var expressionAttributeValues = new Dictionary<string, AttributeValue>();

                switch (userType)
                {
                    case UserTypes.admin:
                        filterExpression = "organisationId = :organisationId";
                        expressionAttributeValues.Add(":organisationId", new AttributeValue(id));
                        break;
                    case UserTypes.installer:
                        filterExpression = "contains(installers, :installer)";
                        expressionAttributeValues.Add(":installer", new AttributeValue(id));
                        break;
                    case UserTypes.installManager:
                        filterExpression = "installManager = :installManager";
                        expressionAttributeValues.Add(":installManager", new AttributeValue(id));
                        break;
                }

                var request = new ScanRequest
                {
                    TableName = TableNames.property,
                    FilterExpression = filterExpression,
                    ExpressionAttributeValues = expressionAttributeValues
                };

                var response = await dynamoDbClient.ScanAsync(request).ConfigureAwait(true);

                LambdaLogger.Log("GetProperties:: recieved " + response.Items.Count + " properties from table");

                var tempProperties = new List<Property.Property>();
                foreach (var item in response.Items)
                {
                    var property = Property.Property.deserialiseAsProperty(item);
                    tempProperties.Add(property);
                }

                LambdaLogger.Log("GetProperties:: deserialised " + tempProperties.Count + " properties");

                if (userType.Equals(UserTypes.installer))
                {
                    var validProperties = new List<Property.Property>();
                    foreach (var tempProperty in tempProperties)
                    {
                        //surveyDate today or before and survey not complete
                        //installDate today or before and install not complete
                        if ((DateTime.Parse(tempProperty.surveyDate, new CultureInfo("en-GB")) <= DateTime.Now && !tempProperty.surveyComplete) ||
                            (DateTime.Parse(tempProperty.installDate, new CultureInfo("en-GB")) <= DateTime.Now && !tempProperty.installComplete))
                        {
                            validProperties.Add(tempProperty);
                        }
                        LambdaLogger.Log("Test:: property " + tempProperty.address + " fine");
                    }
                    properties = validProperties.ToArray();
                    LambdaLogger.Log("GetProperties:: " + properties.Length + " valid properties for installer");
                }
                else
                {
                    properties = tempProperties.ToArray();
                }
            }
            catch (Exception e)
            {
                LambdaLogger.Log("Unable to retrieve properties: " + e.ToString());
            }

            return properties;
        }
    }
}
